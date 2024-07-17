using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hp같은 정보는 어차피 싱글게임이라 게임매니저에서 관리해도 좋을듯

// 플레이어 방향 상태, 왼쪽은 오른쪽에서 뒤집기
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
    protected SpriteRenderer silhouette;//피격 전용 실루엣

    // Status
    // Status - Basic
    [SerializeField]
    protected float rollingSpeed = 2.5f;

    // 무기 구현이후 attackDelay알맞게 수정
    protected bool avoidCheck;
    public bool AvoidCheck => avoidCheck;

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

    public bool isReverse = false; // 캐릭터를 뒤집는 상태, 추후에 변경못하게 수정
    public bool IsReverse => isReverse;
    protected bool isWalk = false;

    // Status - curStatus
    public float speedApply; // 실제 스피드
    public float speed;
    public bool rollCnt = false; // 구른후 빨라지는 버프 체크
    // 지금 초록 버프 먹어도 구르기 전 까진 마약 버프 적요이안됨 이거 수정해야함

    protected bool isHit = false; // 피격당함
    public bool IsHit => isHit;
    protected bool isDead;
    protected bool isRoll = false;
    protected bool rollReverse = false; // 애니메이션 대칭

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
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        speedApply = InGameManager.Instance.Speed;
        speed = speedApply;
    }


    protected virtual void Update()
    {
        if (!InGameManager.Instance.IsPause && !isDead)
        {
            InputKey();
            VectorStatus(curVec);
            Roll();
            Skill();
            // 추후 게임 매니저로 변경될 가능성 있음
            Interaction();
            EatDrug();
            UserGenade();
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
        // 만약 inputVec 0이면 Idle, 아니면 Run으로 체인지
        moveVec = inputVec.normalized;

        if (isRoll)
        {
            nextVec = rollVec.normalized * rollingSpeed * Time.fixedDeltaTime;
        }
        else
        {
            isWalk = moveVec != Vector2.zero ? true : false;
            nextVec = moveVec.normalized * Time.fixedDeltaTime *
                (speed);
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

        // 구르기 방향 계산
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

        // 회피 여부 체크
        if (DrugManager.Instance.green2)
        {
            DrugManager.Instance.RunGreenBuff2();
            return; // 상태관련해서 수정할게 있으면 수정하고 리턴
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

        // 광전사 여부 체크
        if (DrugManager.Instance.red2)
        {
            DrugManager.Instance.RunRedBuff2();
        }
    }

    private IEnumerator HitTime()
    {
        int i = 0;
        for(i =0; i<3; i++)  // 추후 실루엣 스프라이트로 변경
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.4f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
        isHit = false;
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
            // 인벤 마약
            if (shiftKey && InGameManager.Instance.drugInven != null)
            {
                InGameManager.Instance.drugInven.UseItem();
                UIManager.Instance.inGameUI.DrugInven(null);
            }
            // 땅에있는 마약
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

    // 방향 전환
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

    protected void Skill()
    {
        if (!skillKey || !UIManager.Instance.inGameUI.skillUI.CanUseSkill || isRoll) return;

        StartCoroutine(ESkill());
    }
    
    // 스킬 시작과 끝을 계산
    private IEnumerator ESkill()
    {
        yield return null;
        // 카메라 고정 및 위치 이동
        CameraController.Instance.CameraActive(false);
        CameraController.Instance.CameraPos(transform.position.x, transform.position.y);
        anim.SetTrigger("Skill");

        // 일시 동작 정지
        InGameManager.Instance.Pause(true);

        UIManager.Instance.inGameUI.skillUI.UseSkill();
        PlayerSkill();

        yield return new WaitForSecondsRealtime(1.0f); // 타임스케일 영향 x
        InGameManager.Instance.Pause(false);
        CameraController.Instance.CameraActive(true);
    }

    protected abstract void PlayerSkill();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet") //수정
        {
            if (avoidCheck || isHit) return;
           
            Damage(1); // 데미지 로직 나중에 수정
        }
    }
}
