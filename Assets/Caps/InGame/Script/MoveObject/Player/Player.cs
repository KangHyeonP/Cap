using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hp���� ������ ������ �̱۰����̶� ���ӸŴ������� �����ص� ������

// �÷��̾� ���� ����, ������ �����ʿ��� ������
public enum PlayerVetor
{
    Front, Cross, Side, Back
}

public enum PlayerAnimator
{
    Idle, Run, Roll
}

public abstract class Player : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    [SerializeField]
    protected SpriteRenderer silhouette;//�ǰ� ���� �Ƿ翧

    // Status
    // Status - Basic
    public float rollingSpeed = 2.5f;

    [SerializeField]
    protected float skillDelay = 15f;
    public float SkillDelay => skillDelay;

    // Status - movement
    protected PlayerVetor curVec = PlayerVetor.Front;

    protected Vector2 inputVec;
    public Vector2 InputVec => inputVec;
    protected Vector2 moveVec;
    protected Vector2 rollVec;
    protected Vector2 nextVec;

    // Status - animation
    protected PlayerAnimator curAnim = PlayerAnimator.Idle;

    public bool isReverse = false; // ĳ���͸� ������ ����, ���Ŀ� ������ϰ� ����
    public bool IsReverse => isReverse;
    protected bool isWalk = false;
    public bool isAttack = false;

    // Status - curStatus
    protected bool avoidCheck;
    public bool AvoidCheck => avoidCheck;
    public int avoidChance;

    public float speedApply; // ���� ���ǵ�
    public float speed;
    public bool rollCnt = false; // ������ �������� ���� üũ
    // ���� �ʷ� ���� �Ծ ������ �� ���� ���� ���� �����̾ȵ� �̰� �����ؾ���

    protected bool isHit = false; // �ǰݴ���
    public bool IsHit => isHit;
    protected bool isDead;
    protected bool isRoll = false;
    protected bool rollReverse = false; // �ִϸ��̼� ��Ī

    // InputKey
    protected bool attackKey;
    public bool AttackKey => attackKey;
    protected bool skillKey;
    protected bool rollKey;
    protected bool swapKey1;
    protected bool swapKey2;
    protected bool swapKey3;
    protected bool swapKey4;
    protected bool interaction;
    protected bool qKey;
    protected bool shiftKey;

    // Weapon
    //[SerializeField] // Ȯ�ο�
    //protected int weaponIndex = 2;
    public int tempWeaponIndex = 2;
    public int gunCnt = 0; // �Ķ� ���� ���� �ε��� ī����
    public bool gunCheck = false;

    public GameObject[] mainWeapon; // ������, ����, ���� �� -> ���� ���Ⱑ �߰��ȴٸ� ����ó�� �ѱ⺰�� �з��ϰų� �״�� �߰�
    public Weapons tempGun; // ��ü�� ����

    public GameObject[] subWeapon; // �̱�, �Ƴ��ܴ� ��
    public GameObject knife;
    [SerializeField]
    private GameObject weaponPivot;
    [SerializeField]
    public GameObject fireEffect;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        speedApply = InGameManager.Instance.Speed;
        speed = speedApply;
    }


    protected virtual void Update()
    {
        if (InGameManager.Instance.IsPause || isDead) return;

        InputKey();
        VectorStatus(curVec);
        Roll();
        Swap();
        Skill();
        Interaction();
        EatDrug();
        UserGenade();
    }

    protected virtual void FixedUpdate()
    {
        if (InGameManager.Instance.IsPause || isDead) return;

        Move();
    }

    protected void InputKey()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        attackKey = Input.GetButton("Fire1");
        skillKey = Input.GetButtonDown("Jump");
        rollKey = Input.GetButton("Fire2");
        swapKey1 = Input.GetButtonDown("Swap1");
        swapKey2 = Input.GetButtonDown("Swap2");
        swapKey3 = Input.GetButtonDown("Swap3");
        swapKey4 = Input.GetButtonDown("Swap4");
        interaction = Input.GetButtonDown("Interaction");
        qKey = Input.GetKeyDown(KeyCode.Q);
        shiftKey = Input.GetKey(KeyCode.LeftShift);
    }

    protected void Move()
    { 
        // ���� inputVec 0�̸� Idle, �ƴϸ� Run���� ü����
        moveVec = inputVec.normalized;

        if(isAttack) InGameManager.Instance.knifeEffect.transform.position = weaponPivot.transform.position;

        if (isRoll)
        {
            nextVec = rollVec.normalized * rollingSpeed * Time.fixedUnscaledDeltaTime;
        }
        else
        {
            isWalk = moveVec != Vector2.zero ? true : false;
            nextVec = moveVec.normalized * Time.fixedUnscaledDeltaTime *
                (speed);
            anim.SetBool("Walk", isWalk);
        }

        rigid.MovePosition(rigid.position + nextVec);
    }

    protected void Roll()
    {
        if (!isRoll && InputVec != Vector2.zero && rollKey && !isAttack)
        {
            rollVec = InputVec;
            StartCoroutine(IRoll());
        }
    }

    private IEnumerator IRoll()
    {
        string rollStatus = null;
        rollReverse = false;
        weaponPivot.SetActive(false);
        isRoll = true;

        // ������ ���� ���
        if (rollVec.x > 0)
        {
            if (rollVec.y > 0) rollStatus = "RollCross";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollSide";
        }
        else if(rollVec.x ==0)
        {
            if (rollVec.y > 0) rollStatus = "RollBack";
            else if(rollVec.y < 0) rollStatus = "RollFront";
        }
        else
        {
            rollReverse = true;
            if (rollVec.y > 0) rollStatus = "RollCross";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollSide";
        }

        if (rollReverse) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);

        rollingSpeed *= 2;
        anim.SetTrigger("Roll");
        anim.SetTrigger(rollStatus);

        avoidCheck = true;
      
        yield return new WaitForSeconds(0.7f);

        avoidCheck = false;
        nextVec = Vector2.zero;

        yield return new WaitForSeconds(0.3f);

        weaponPivot.SetActive(true);
        isRoll = false;
        rollingSpeed *= 0.5f;

        if (!rollCnt && DrugManager.Instance.isRollSpeedUp)
        {
            speed *= 1.5f;
            rollCnt = true;
            yield return new WaitForSeconds(2.5f);
            speed = speedApply;
            rollCnt = false;
        }
    }


    protected void Damage(int power)
    {
        if (avoidCheck || isHit) return;
        else if(DrugManager.Instance.isBulletAvoid)
        {
            Debug.Log("���� ����");

            avoidChance = Random.Range(1, 101);
            if (avoidChance <= 25)
            {
                Debug.Log("ȸ�� ����");
                return;
            }
        }
        else if(UIManager.Instance.isBulletProof)
        {
            UIManager.Instance.BulletproofUpdate(false);

            return;
        }

        InGameManager.Instance.Hit(power);
        UIManager.Instance.hpUpdate();

        if(InGameManager.Instance.IsDead)
        {
            isDead = true;
            return;
        }
        else
        {
            isHit = true;
            StartCoroutine(HitTime());
        }

        // ������ ���� üũ
        if (DrugManager.Instance.red2)
        {
            DrugManager.Instance.RunRedBuff2();
        }
    }

    private IEnumerator HitTime()
    {
        int i = 0;
        for(i =0; i<3; i++)  // ���� �Ƿ翧 ��������Ʈ�� ����
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.4f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
        isHit = false;
    }

    public void KnifeAttack(bool check)
    {
        InGameManager.Instance.knifePivot.transform.localScale = new Vector3(1, 1, 1);

        if (isReverse)
        {
            InGameManager.Instance.knifePivot.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (check)
        {
            InGameManager.Instance.knifeEffect.transform.position = weaponPivot.transform.position;
            InGameManager.Instance.knifeEffect.transform.rotation = weaponPivot.transform.rotation;
            InGameManager.Instance.knifeEffect.SetActive(true);
        }
        else InGameManager.Instance.knifeEffect.SetActive(false);
    }

    // �ϴ� ��������� ���� �̱����̹Ƿ� ��� ���
    protected void Swap()
    {
        if (isRoll || isAttack) return;

        if (!swapKey1 && !swapKey2 && !swapKey3) return;

        if (swapKey1 && InGameManager.Instance.gunInven != null)
        {
            if(DrugManager.Instance.isManyWeapon && tempWeaponIndex == 0) gunCheck = true;
            tempWeaponIndex = 0;
        }
        else if (swapKey2 && InGameManager.Instance.pistolInven != null)
            tempWeaponIndex = 1;
        else if (swapKey3)
            tempWeaponIndex = 2;

        WeaponSwap(tempWeaponIndex);
    }

    public void WeaponSwap(int idx)
    {
        // ���⸦ ���� ��Ȱ��ȭ, ����� ���� ��ٸ� ���� Ȱ��ȭ ���⸸ üũ�Ͽ� ��Ȱ��ȭ�� ������ �������� ����
        foreach (GameObject g in mainWeapon)
            g.SetActive(false);
        foreach (GameObject g in subWeapon)
            g.SetActive(false);
        knife.SetActive(false);

        // �ش� ���⸸ Ȱ��ȭ
        if (idx == 0)
        {   
            if(gunCheck)
            {
                tempGun = InGameManager.Instance.gunInven;
                InGameManager.Instance.gunInven = InGameManager.Instance.blueGunInven;
                InGameManager.Instance.blueGunInven = tempGun;
                gunCheck = false;
            }

            mainWeapon[InGameManager.Instance.gunInven.index].SetActive(true);
            UIManager.Instance.inGameUI.WeaponInven(InGameManager.Instance.gunInven.index);
        }
        else if(idx == 1)
        {
            subWeapon[InGameManager.Instance.pistolInven.index].SetActive(true);
            UIManager.Instance.inGameUI.WeaponInven(InGameManager.Instance.pistolInven.index + 3);
        }
        else
        {
            knife.SetActive(true);
            UIManager.Instance.inGameUI.WeaponInven(5);
        }
    }

    protected void Interaction()
    {
        if (interaction && InGameManager.Instance.isItem == true)
        {
            InGameManager.Instance?.tempItem.GetItem();
        }
    }
    protected void EatDrug()
    {
        if (qKey)
        {
            // �κ� ����
            if (shiftKey && InGameManager.Instance.drugInven != null)
            {
                InGameManager.Instance.drugInven.UseItem();
                UIManager.Instance.inGameUI.DrugInven(null);
            }
            // �����ִ� ����
            else if (InGameManager.Instance.tempDrug != null && !shiftKey)
            {
                InGameManager.Instance.tempDrug.UseItem();
            }
        }
    }

    protected void UserGenade()
    {
        if(swapKey4)
        {
            InGameManager.Instance.UseGrenade();
        }
    }

    // ���� ��ȯ
    public void ChangeVector(PlayerVetor pVec, bool checkReverse)
    {
        if (isAttack) return;

        isReverse = checkReverse;
        VectorStatus(pVec);
    }

    protected void VectorStatus(PlayerVetor pVec)
    {
        if (isRoll) return;

        if (isReverse) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);

        if (pVec != curVec)
            AnimationChange(pVec);
        curVec = pVec;
    }

    public void AnimStatus(PlayerAnimator pAnim)
    {
        curAnim = pAnim;
    }

    public void AnimationChange(PlayerVetor pVec)
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerVetor p = (PlayerVetor)i;
            anim.SetBool(p.ToString(), false);
        }

        anim.SetBool("Walk", false);

        anim.SetBool(pVec.ToString(), true);
    }

    protected void Skill()
    {
        if (!skillKey || !UIManager.Instance.inGameUI.skillUI.CanUseSkill || isRoll) return;

        StartCoroutine(ESkill());
    }
    
    // ��ų ���۰� ���� ���
    private IEnumerator ESkill()
    {
        yield return null;
        // ī�޶� ���� �� ��ġ �̵�
        CameraController.Instance.CameraActive(false);
        CameraController.Instance.CameraPos(transform.position.x, transform.position.y);
        anim.SetTrigger("Skill");

        // �Ͻ� ���� ����
        InGameManager.Instance.Pause(true);

        UIManager.Instance.inGameUI.skillUI.UseSkill();
        PlayerSkill();

        yield return new WaitForSecondsRealtime(1.0f); // Ÿ�ӽ����� ���� x
        InGameManager.Instance.Pause(false);
        CameraController.Instance.CameraActive(true);
    }

    protected abstract void PlayerSkill();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet") //����
        {
            if (avoidCheck || isHit) return;
           
            Damage(1); // ������ ���� ���߿� ����
        }
    }
}
