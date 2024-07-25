using System.Collections;
using System.Collections.Generic;
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

public abstract class AI : MonoBehaviour
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
    protected Transform muzzle;


    // 현재 AI 활성화 상태인지 고려, 랜덤생성 or 다음 칸 배치 같은 문제에서 적용함
    protected bool activeRoom = false;
    public bool ActiveRoom => activeRoom;

    // AI Stats
    [SerializeField]
    protected int hp = 100;
    [SerializeField]
    protected int maxHp = 100;

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

    protected virtual void Awake()
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

    protected virtual void UpdateState(EnemyStatus enemy)
    {
        
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
        //Debug.Log("추격 실행");

        anim.SetBool("Chase", true);

        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay))
        {
            curStatus = EnemyStatus.Attack;
            anim.SetBool("Chase", false);
            //Debug.Log("추격 종료");
        }
        agent.SetDestination(target.position);
    }

    protected void Attack()
    {
        if (IsAttack) return;

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

    public void Damage(int damage, WeaponValue value)
    {
       // Debug.Log("맞았어" + value);

        if (value == WeaponValue.Gun)
        {
            damage += InGameManager.Instance.bulletPower;

            Debug.Log("총알 포함 데미지 : " + damage);
        }

        if (DrugManager.Instance.red2)
        {
            damage = damage + damage * DrugManager.Instance.powerUpValue / 100;

            Debug.Log("광전사 포함 데미지 : " + damage);
        }
        else if (DrugManager.Instance.isBleeding && damage >= maxHp * 0.3f)
        {
            StartCoroutine(Bleed());
        }

        if (DrugManager.Instance.isAnger)
        {
            // 데미지를 주고 카운팅, 첫 타격일 땐 분노가 없으니 0으로 카운팅 되어 계산
            damage += (int)(damage * DrugManager.Instance.angerPower * 0.4f);
            DrugManager.Instance.AngryCount();

            Debug.Log("분노 포함 데미지 : " + damage);
        }

        hp -= damage;
        Debug.Log("들어온 데미지 : " + damage);
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

        for (int i = 0; i < 5; i++)
        {
            hp -= maxHp / 100;
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


    // 나중에 죽었을때 기능 구현
    protected void Die()
    {
        isDie = true;
        isDetect = false;
        cirCollider2D.enabled = false;
        agent.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Bullet 테그별로 나누기(총알이 다 다를경우)
        if (collision.CompareTag("PlayerBullet"))
        {
            Damage(InGameManager.Instance.Power + DrugManager.Instance.power, WeaponValue.Gun);
        }
        else if (collision.CompareTag("Knife"))
        {
            Damage(InGameManager.Instance.Power + DrugManager.Instance.power, WeaponValue.Knife);
        }
    }
}
