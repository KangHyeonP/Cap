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
    private int hp = 5;

    // AI State
    [SerializeField]
    protected EnemyStatus curStatus;
    public EnemyStatus CurStatus => curStatus;

    protected bool isDie = false;
    [SerializeField]
    protected bool isDetect = false;
    protected bool isMoveLean = false;
    protected bool isLean = false;
    protected bool isAttack = false;
    public bool IsAttack => isAttack;

    // AI Attack
    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    protected float curAttackDelay;
    [SerializeField]
    protected float attackMoveDelay; // 공격 후 제동 시간
    [SerializeField]
    protected float attackSpeed;
    [SerializeField]
    protected float attackDistance;
    [SerializeField]
    protected float attackRecoil;

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

        // 추후 애니메이션으로 수정할거임
        if (isDetect && enemy != EnemyStatus.Lean) ChangeSprite();

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

    // 방향별 스프라이트 수정, 추후 애니메이션을 적용할 거라서 코드 수정필요함
    protected void ChangeSprite()
    {
        if (agentAngleIndex == 3) agentAngleIndex = 2;
        spritesRenderer.sprite = basicSprites[agentAngleIndex];
    }

    // 플레이어의 방향 계산
    protected void AgentAngle()
    {
        agentAngleValue = AgentVector();
        agentAngleIndex = AngleCalculate(agentAngleValue); // up(후면), down(정면), left(왼쪽), right(오른쪽)
    }

    protected float AgentVector()
    {
        Vector3 value = InGameManager.Instance.player.transform.position - transform.position;
        // 현재 자기 자신을 기점으로 플레이어의 위치를 계산하여 어느 방향을 바라봐야하는지 보여줌
        float angle;
        angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg; // 범위가 180 ~ -180

        return angle;
    }

    protected int AngleCalculate(float angleValue)
    {
        // 아마 해당 이미지를 넣어봐야지 알듯

        int Index = -1;
        // 1사분면, 왼 윗 대각까진 우선순위
        if (angleValue <= 135f && angleValue > 45f) Index = 0; // up
        // 2사분면, 오른 윗 대각까진 우선순위
        else if (angleValue <= 45f && angleValue > -45f) Index = 3; // right
        // 3사분면, 오른 아랫대각까진 우선
        else if (angleValue <= -45f && angleValue > -135f) Index = 1; // down 
        // 4사분면, 왼쪽 아랫대각까진 우선
        else if (angleValue <= -135f || angleValue > 135f) Index = 2; // left

        if (angleValue <= -90 || angleValue > 90) isReverse = true;
        else isReverse = false;

        return Index;
    }

    protected void Idle()
    {
       /* if (!activeRoom)
        {
            isDetect = false;
        }*/ //다 안죽이곤 못나가서 필요 없을듯?

        if (isDetect)
        {
            curStatus = EnemyStatus.Chase;
            return;
        }


        // 재자리 애니메이션 적용
        
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
        
        agent.SetDestination(target.position);
        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay)) curStatus = EnemyStatus.Attack;
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
        yield return new WaitForSeconds(attackMoveDelay);
        // yield return new WaitForSeconds(0.5f); // 쏘기전 잠깐 제동

        AttackLogic();

        //yield return new WaitForSeconds(attackMoveDelay);

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

        hp--;
        Debug.Log("몬스터 남은 체력: " + hp);

        if (hp == 0)
        {
            RoomController.Instance.ClearRoomCount();
            Destroy(gameObject);
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


    /*private void UpdateLean()
    {
        if (!isMoveLean) return;
        transform.localPosition = Vector3.MoveTowards(transform.position, moveVec, 1.0f);
    }*/


    // 나중에 죽었을때 기능 구현
    private void Die()
    {
        //if (isDetect) isDetect = false;
        //agent.enabled = false;

        isDie = true;
        isDetect = false;
        cirCollider2D.enabled = false;
        agent.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            Damage(1);
            
        }
    }
}
