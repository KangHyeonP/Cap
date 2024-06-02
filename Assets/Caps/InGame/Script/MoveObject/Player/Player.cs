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

    // Status
    // Status - Basic
    protected float speed = 3.0f; // ���ǵ�
    [SerializeField]
    protected float rollingSpeed = 1.0f;
    protected float power; // ���ݷ�
    protected int armor; // ��ź

    // ���� �������� attackDelay�˸°� ����
    protected float attackDelay = 1.0f;
    protected float curAttackDelay;
    protected bool avoidCheck;

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

    // Status - curStatus
    protected bool isHit = false; // �ǰݴ���
    protected bool isDead;
    protected bool isRoll = false;
    protected bool rollReverse = false; // �ִϸ��̼� ��Ī

    // InputKey
    protected bool attackKey;
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
    protected int weaponIndex = 0;
    public string[] weapon;
    [SerializeField]
    private GameObject weaponPivot;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    protected virtual void Update()
    {
        if (!InGameManager.Instance.IsPause && !isDead)
        {
            InputKey();
            VectorStatus(curVec);
            Roll();
            Attack();
            Skill();
            // ���� ���� �Ŵ����� ����� ���ɼ� ����
            Interaction();
            EatDrug();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!InGameManager.Instance.IsPause && !isDead)
        {
            Move();
        }
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

        if (isRoll)
        {
            nextVec = rollVec.normalized * rollingSpeed * Time.fixedDeltaTime;
        }
        else
        {
            isWalk = moveVec != Vector2.zero ? true : false;
            nextVec = moveVec.normalized * speed * Time.fixedDeltaTime;
            anim.SetBool("Walk", isWalk);
        }

        rigid.MovePosition(rigid.position + nextVec);
    }

    protected void Roll()
    {
        if (!isRoll && InputVec != Vector2.zero && rollKey)
        {
            rollVec = moveVec;
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

        //yield return new WaitForSeconds()
    }

    protected void Damage(int power)
    {
        if (isDead) return;

        if (avoidCheck) return;

        // ȸ�� ���� üũ
        if (DrugManager.Instance.green2)
        {
            DrugManager.Instance.RunGreenBuff2();
            return; // ���°����ؼ� �����Ұ� ������ �����ϰ� ����
        }

        InGameManager.Instance.Hit(power);
        UIManager.Instance.hpUpdate();

        // ������ ���� üũ
        if (DrugManager.Instance.red2)
        {
            DrugManager.Instance.RunRedBuff2();
        }
    }

    // ���� ��� �����Ͽ� �߰��ϱ�
    protected void Attack()
    {
        curAttackDelay += Time.deltaTime;

        if (attackKey && curAttackDelay >= attackDelay)
        {
            curAttackDelay = 0;
            StartCoroutine(IAttack());
        }
    }

    protected void Swap()
    {
        int curIndex = -1;

        if (!swapKey1 && !swapKey2 && !swapKey3) return;

        if (swapKey1)
            curIndex = 0;

        if (swapKey2)
            curIndex = 1;

        if (swapKey3)
            curIndex = 2;

        if (curIndex != weaponIndex)
        {
            weaponIndex = curIndex;
            Debug.Log(weapon[weaponIndex]);
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

    // ���� ��ȯ
    public void ChangeVector(PlayerVetor pVec, bool checkReverse)
    {
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

    public IEnumerator IAttack()
    {

        Debug.Log("����");
        yield return new WaitForSeconds(1.0f);
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
            if (avoidCheck) return;
            Debug.Log("�Ѿ� ����");
            Destroy(collision.gameObject);
            Damage(1); // ������ ���� ���߿� ����
        }
    }
}
