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

    // Status
    // Status - Basic
    [SerializeField]
    protected int hp = 10; // 체력
    public int Hp => hp;

    protected float speed = 3.0f; // 스피드
    protected float rollingSpeed = 1.0f;
    protected float power; // 공격력
    protected float armor; // 방어력

    // 무기 구현이후 attackDelay알맞게 수정
    protected float attackDelay = 1.0f;
    protected float curAttackDelay;

    [SerializeField]
    protected float skillDelay = 15f;
    public float SkillDelay => skillDelay;

    // Status - movement
    protected PlayerVetor curVec = PlayerVetor.Front;

    protected Vector2 inputVec;
    public Vector2 InputVec => inputVec;
    protected Vector2 moveVec;
    protected Vector2 rollVec;

    // Status - animation
    protected PlayerAnimator curAnim = PlayerAnimator.Idle;

    public bool isReverse = false; // 캐릭터를 뒤집는 상태, 추후에 변경못하게 수정
    public bool IsReverse => isReverse;
    protected bool isWalk = false;

    // Status - curStatus
    protected bool isHit = false; // 피격당함
    protected bool isDead;
    protected bool isRoll = false;
    protected bool rollReverse = false; // 애니메이션 대칭

    // InputKey
    protected bool attackKey;
    protected bool skillKey;
    protected bool rollKey;

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
    }

    protected void Move()
    {
        // 만약 inputVec 0이면 Idle, 아니면 Run으로 체인지
        moveVec = inputVec.normalized;
        Vector2 nextVec;

        if (isRoll)
        {
            nextVec = rollVec.normalized * rollingSpeed * 1.5f * Time.fixedDeltaTime;
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

        // 구르기 방향 계산
        if(rollVec.x > 0)
        {
            if (rollVec.y > 0) rollStatus = "RollCrossUp";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollCrossDown";
        }
        else if(rollVec.x ==0)
        {
            if (rollVec.y > 0) rollStatus = "RollBack";
            else if(rollVec.y < 0) rollStatus = "RollFront";
        }
        else
        {
            rollReverse = true;
            if (rollVec.y > 0) rollStatus = "RollCrossUp";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollCrossDown";
        }

        if (rollReverse) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);

        isRoll = true;
        rollingSpeed *= 2;
        anim.SetTrigger("Roll");
        anim.SetTrigger(rollStatus);

        yield return new WaitForSeconds(0.75f);

        isRoll = false;
        rollingSpeed *= 0.5f;
    }

    protected void Damage(int power)
    {
        //if (!isDead) return;

        hp -= power;
        Debug.Log("피격당함");
        Debug.Log("현재 체력 : " + hp);
    }

    // 무기 기능 구현하여 추가하기
    protected void Attack()
    {
        curAttackDelay += Time.deltaTime;

        if (attackKey && curAttackDelay >= attackDelay)
        {
            curAttackDelay = 0;
            StartCoroutine(IAttack());
        }
    }

    // 방향 전환
    public void ChangeVector(PlayerVetor pVec, bool checkReverse)
    {
        Debug.Log("실행 됨?");
        Debug.Log("그럼 리버스는 ? :" + checkReverse);
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

        Debug.Log("공격");
        yield return new WaitForSeconds(1.0f);
    }

    protected void Skill()
    {
        if (!skillKey || !UIManager.Instance.inGameUI.skillUI.CanUseSkill) return;

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
        Debug.Log("플레이어에 들어온 트리거");
    }
}
