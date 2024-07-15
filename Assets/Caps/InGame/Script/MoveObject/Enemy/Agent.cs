using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStatus
{ 
    Idle,
    Chase,
    Attack,
    Stun,
    Lean,
    Die
}

public enum EnemyVetor
{
    Front, Cross, Side, Back
}

public abstract class Agent : MonoBehaviour
{
    // 추후 gameManager의 player를 받아올것
    [SerializeField]
    protected Transform target;

    // Component
    protected NavMeshAgent agent;
    protected Rigidbody2D rigid;
    protected CircleCollider2D cirCollider2D;
    protected SpriteRenderer spritesRenderer;
    protected Animator anim;

    // 추후 애니메이션으로 변경
    [SerializeField]
    protected Sprite[] basicSprites;
    [SerializeField]
    protected Transform muzzle;


    // 현재 AI 활성화 상태인지 고려, 랜덤생성 or 다음 칸 배치 같은 문제에서 적용함
    protected bool activeRoom = false;
    public bool ActiveRoom => activeRoom;

    // AI Stats
    [SerializeField]
    private int hp = 100;
    [SerializeField]
    private int maxHp = 100;

    // AI State
    [SerializeField]
    protected EnemyStatus curStatus;
    public EnemyStatus CurStatus => curStatus;
    protected EnemyVetor curVec = EnemyVetor.Front; // 추가1

    protected bool isDie = false;
    [SerializeField]
    protected bool isDetect = false;
    protected bool isMoveLean = false;
    protected bool isLean = false;
    protected bool isAttack = false;
    public bool IsAttack => isAttack;

    // AI Attack
    [SerializeField]
    protected float attackDelay; // 공격 딜레이
    [SerializeField]
    protected float curAttackDelay; // 현재 공격 딜레이 시간
    [SerializeField]
    protected float attackMoveDelay; // 공격 전 제동 시간
    [SerializeField]
    protected float attackSpeed; // 총알 속도
    [SerializeField]
    protected float attackDistance; // 공격 사거리
    [SerializeField]
    protected float attackRecoil; // 공격 반동

    // Object Interaction
    protected bool tableMove;
    protected TableArrow curTableArrow;

    // Agent Vector;
    protected float agentAngleValue;
    protected int agentAngleIndex;
    protected bool isReverse = false;

    protected Vector3 moveVec; // lean상태에서 움직일 방향
    protected Vector3 tableVec; // table 위치

    // AI 임시 삭제용
    public bool TestAgent = false;

    protected virtual  void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        rigid = GetComponent<Rigidbody2D>();
        curStatus = EnemyStatus.Idle;
        cirCollider2D = GetComponent<CircleCollider2D>();
        spritesRenderer = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>(); AI 그림 나오면 적용할거임
    }

    protected virtual void Start()
    {
        target = InGameManager.Instance.player.transform;
        hp = maxHp;
    }

    protected virtual void Update()
    {
        if (!InGameManager.Instance.IsPause)
        {
            //if (isDetect) UpdateState(curStatus);
            UpdateState(curStatus);
        }
        // AI 삭제 임시용
        if (TestAgent) Destroy(this.gameObject);
    }

    // 떨림 방지
    protected virtual void FixedUpdate()
    {
        if (!InGameManager.Instance.IsPause)
        {
            rigid.velocity = Vector2.zero;
        }
    }

    protected void UpdateState(EnemyStatus enemy)
    {
        if (isDie) return;

        AgentAngle();

        if (isReverse) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);

        curAttackDelay += Time.deltaTime;

        switch (enemy)
        {
            case EnemyStatus.Idle:
                Idle();
                break;
            case EnemyStatus.Chase:
                Chase();
                break;
            case EnemyStatus.Attack:
                Attack();
                break;
            case EnemyStatus.Lean:
                UpLean();
                //UpdateLean();
                break;
            case EnemyStatus.Die:
                Die();
                break;
            default:
                Debug.Log("미구현 기능");
                break;
        }
    }

    // 플레이어의 방향 계산
    protected void AgentAngle()
    {
        agentAngleValue = AgentVector();
        AngleCalculate(agentAngleValue);
    }

    protected float AgentVector()
    {
        Vector3 value = InGameManager.Instance.player.transform.position - transform.position;
        // 현재 자기 자신을 기점으로 플레이어의 위치를 계산하여 어느 방향을 바라봐야하는지 보여줌
        float angle;
        angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg; // 범위가 180 ~ -180

        return angle;
    }

    protected void AngleCalculate(float angleValue)
    {
        // 후면(윗 방향)
        if (angleValue < 120 && angleValue > 60)
            curVec = EnemyVetor.Back;
        // 오른 대각
        else if (angleValue <= 60 && angleValue >= 10)
            curVec = EnemyVetor.Cross;
        // 오른
        else if (angleValue < 10 && angleValue >= -60)
            curVec = EnemyVetor.Side;
        // 정면(아랫 방향)
        else if (angleValue < -60 && angleValue > -120)
            curVec = EnemyVetor.Front;
        // 왼쪽(오른쪽에서 뒤집기)
        else if (angleValue <= -120 || angleValue > 170)
            curVec = EnemyVetor.Side;
        // 왼쪽 대각(오른쪽에서 뒤집기)
        else if (angleValue <= 170 || angleValue >= 120)
            curVec = EnemyVetor.Cross;

        if (angleValue <= -90 || angleValue > 90) isReverse = true;
        else isReverse = false;

        //return index;

        if (isDetect && curStatus != EnemyStatus.Lean)
        {
            for (int i = 0; i < 4; i++)
            {
                EnemyVetor a = (EnemyVetor)i;
                anim.SetBool(a.ToString(), false);
            }

            anim.SetBool(curVec.ToString(), true);
        }
    }

    protected void Idle()
    {
        if (isDetect)
        {
            curStatus = EnemyStatus.Chase;
            return;
        }
    }

    // 플레이어가 들어온지 탐지
    public void PlayerRoom()
    {
        isDetect = true;
    }

    protected void Chase()
    {
        //if (!isDetect) return;

        if (!isDetect)
        {
            curStatus = EnemyStatus.Idle;
            return;
        }
        Debug.Log("추격 실행");

        anim.SetBool("Chase", true);

        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay))
        {
            curStatus = EnemyStatus.Attack;
            anim.SetBool("Chase", false);
            Debug.Log("추격 종료");
        }
        agent.SetDestination(target.position);
    }

    protected void Attack()
    {
        if(IsAttack) return;

        //Debug.Log("코루틴 시작 1");
        isAttack = true;
        isDetect = false;
        agent.isStopped = true;
        curAttackDelay = 0;

        StartCoroutine(IAttack());
    }

    protected virtual IEnumerator IAttack()
    {
        //Debug.Log("공격 실행");
        AttackLogic();
        yield return new WaitForSeconds(attackDelay);


        //Debug.Log("공격 끝");

        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
    }

    protected abstract void AttackLogic();

    // 연호가 해당 프레임까지 그려주기 힘들면 다른 방면 생각
    // 그려준다면 방향별로 고개만 빙글빙글 3프레임정도
    protected void Stun()
    {

    }

    public void Damage(int damage)
    {
        Debug.Log("맞았어");

        damage += InGameManager.Instance.bulletPower;

        if(DrugManager.Instance.red2)
        {
            damage = damage + damage * DrugManager.Instance.powerUpValue / 100;
        }
        else if(DrugManager.Instance.isBleeding && damage >= maxHp * 0.3f)
        {
            StartCoroutine(Bleed());
        }

        hp -= damage;
        Debug.Log("몬스터 남은 체력: " + hp);

        if (hp <= 0)
        {
            RoomController.Instance.ClearRoomCount();
            Destroy(gameObject);
        }
    }

    public IEnumerator Bleed()
    {
        yield return new WaitForSeconds(0.5f);

        for(int i=0; i<5; i++)
        {
            hp -= maxHp/100;
            Debug.Log("출혈 중, 몬스터 남은 체력: " + hp);
            yield return new WaitForSeconds(1f);

            if (hp <= 0)
            {
                RoomController.Instance.ClearRoomCount();
                Destroy(gameObject);
                break;
            }
        }
    }
    
    public void UpLean() // 테이블 이동 및 저격까지
    {
        // 기대기
        if (curTableArrow != TableArrow.none && isLean)
            transform.position = Vector3.MoveTowards(transform.position, tableVec, 1.5f * Time.deltaTime);
        // 조준
        else
        {
            LeanAiming();
        }
        
    }

    public void TableValue(Vector3 vec, TableArrow arrow)
    {
        tableVec = vec;
        curTableArrow = arrow;
        curStatus = EnemyStatus.Lean;
        isLean = true;

        agent.isStopped = true;
        StartCoroutine(LeanCount());
    }

    private IEnumerator LeanCount()
    {
        Debug.Log("기대다.");
        anim.SetTrigger("Lean");

        Vector3 playerVec = InGameManager.Instance.player.transform.position - transform.position;

        int index = -1; // 애니메이션 방향
        moveVec = Vector3.zero; // 움직일 방향

        switch (curTableArrow)
        {
            case TableArrow.up:
                if (playerVec.x <= 0)
                {
                    index = 0;
                    moveVec = new Vector3(-5, 0);
                }
                else
                {
                    index = 1;
                    moveVec = new Vector3(5, 0);
                }
                break;
            case TableArrow.down:
                if (playerVec.x <= 0)
                {
                    index = 2;
                    moveVec = new Vector3(-5, 0);
                }
                else
                {
                    index = 3;
                    moveVec = new Vector3(5, 0);
                }
                break;
            case TableArrow.left:
                if (playerVec.y <= 0)
                {
                    index = 4;
                    moveVec = new Vector3(0, -5);
                }
                else
                {
                    index = 5;
                    moveVec = new Vector3(0, 5);
                }
                break;
            case TableArrow.right:
                if (playerVec.y <= 0)
                {
                    index = 6;
                    moveVec = new Vector3(0, -5);
                }
                else
                {
                    index = 7;
                    moveVec = new Vector3(0, 5);
                }
                break;
        }

        yield return new WaitForSeconds(1.0f);

        isLean = false;

        yield return new WaitForSeconds(0.75f); // 조준까지 걸어가는 시간
        agent.isStopped = false;
        curStatus = EnemyStatus.Chase;
    }
    private void LeanAiming()
    {
        transform.localPosition = Vector3.MoveTowards(transform.position, moveVec, 3.0f * Time.deltaTime);
    }


    // 나중에 죽었을때 기능 구현
    private void Die()
    {
        isDie = true;
        isDetect = false;
        cirCollider2D.enabled = false;
        agent.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Bullet 테그별로 나누기(총알이 다 다를경우)
        if (collision.CompareTag("PlayerBullet"))
        {
            Damage(InGameManager.Instance.Power + DrugManager.Instance.power);
        }
    }
}
