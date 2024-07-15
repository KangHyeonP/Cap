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
    // ���� gameManager�� player�� �޾ƿð�
    [SerializeField]
    protected Transform target;

    // Component
    protected NavMeshAgent agent;
    protected Rigidbody2D rigid;
    protected CircleCollider2D cirCollider2D;
    protected SpriteRenderer spritesRenderer;
    protected Animator anim;

    // ���� �ִϸ��̼����� ����
    [SerializeField]
    protected Sprite[] basicSprites;
    [SerializeField]
    protected Transform muzzle;


    // ���� AI Ȱ��ȭ �������� ���, �������� or ���� ĭ ��ġ ���� �������� ������
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
    protected EnemyVetor curVec = EnemyVetor.Front; // �߰�1

    protected bool isDie = false;
    [SerializeField]
    protected bool isDetect = false;
    protected bool isMoveLean = false;
    protected bool isLean = false;
    protected bool isAttack = false;
    public bool IsAttack => isAttack;

    // AI Attack
    [SerializeField]
    protected float attackDelay; // ���� ������
    [SerializeField]
    protected float curAttackDelay; // ���� ���� ������ �ð�
    [SerializeField]
    protected float attackMoveDelay; // ���� �� ���� �ð�
    [SerializeField]
    protected float attackSpeed; // �Ѿ� �ӵ�
    [SerializeField]
    protected float attackDistance; // ���� ��Ÿ�
    [SerializeField]
    protected float attackRecoil; // ���� �ݵ�

    // Object Interaction
    protected bool tableMove;
    protected TableArrow curTableArrow;

    // Agent Vector;
    protected float agentAngleValue;
    protected int agentAngleIndex;
    protected bool isReverse = false;

    protected Vector3 moveVec; // lean���¿��� ������ ����
    protected Vector3 tableVec; // table ��ġ

    // AI �ӽ� ������
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
        // anim = GetComponent<Animator>(); AI �׸� ������ �����Ұ���
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
        // AI ���� �ӽÿ�
        if (TestAgent) Destroy(this.gameObject);
    }

    // ���� ����
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
                Debug.Log("�̱��� ���");
                break;
        }
    }

    // �÷��̾��� ���� ���
    protected void AgentAngle()
    {
        agentAngleValue = AgentVector();
        AngleCalculate(agentAngleValue);
    }

    protected float AgentVector()
    {
        Vector3 value = InGameManager.Instance.player.transform.position - transform.position;
        // ���� �ڱ� �ڽ��� �������� �÷��̾��� ��ġ�� ����Ͽ� ��� ������ �ٶ�����ϴ��� ������
        float angle;
        angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg; // ������ 180 ~ -180

        return angle;
    }

    protected void AngleCalculate(float angleValue)
    {
        // �ĸ�(�� ����)
        if (angleValue < 120 && angleValue > 60)
            curVec = EnemyVetor.Back;
        // ���� �밢
        else if (angleValue <= 60 && angleValue >= 10)
            curVec = EnemyVetor.Cross;
        // ����
        else if (angleValue < 10 && angleValue >= -60)
            curVec = EnemyVetor.Side;
        // ����(�Ʒ� ����)
        else if (angleValue < -60 && angleValue > -120)
            curVec = EnemyVetor.Front;
        // ����(�����ʿ��� ������)
        else if (angleValue <= -120 || angleValue > 170)
            curVec = EnemyVetor.Side;
        // ���� �밢(�����ʿ��� ������)
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

    // �÷��̾ ������ Ž��
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
        Debug.Log("�߰� ����");

        anim.SetBool("Chase", true);

        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay))
        {
            curStatus = EnemyStatus.Attack;
            anim.SetBool("Chase", false);
            Debug.Log("�߰� ����");
        }
        agent.SetDestination(target.position);
    }

    protected void Attack()
    {
        if(IsAttack) return;

        //Debug.Log("�ڷ�ƾ ���� 1");
        isAttack = true;
        isDetect = false;
        agent.isStopped = true;
        curAttackDelay = 0;

        StartCoroutine(IAttack());
    }

    protected virtual IEnumerator IAttack()
    {
        //Debug.Log("���� ����");
        AttackLogic();
        yield return new WaitForSeconds(attackDelay);


        //Debug.Log("���� ��");

        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
    }

    protected abstract void AttackLogic();

    // ��ȣ�� �ش� �����ӱ��� �׷��ֱ� ����� �ٸ� ��� ����
    // �׷��شٸ� ���⺰�� ���� ���ۺ��� 3����������
    protected void Stun()
    {

    }

    public void Damage(int damage)
    {
        Debug.Log("�¾Ҿ�");

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
        Debug.Log("���� ���� ü��: " + hp);

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
            Debug.Log("���� ��, ���� ���� ü��: " + hp);
            yield return new WaitForSeconds(1f);

            if (hp <= 0)
            {
                RoomController.Instance.ClearRoomCount();
                Destroy(gameObject);
                break;
            }
        }
    }
    
    public void UpLean() // ���̺� �̵� �� ���ݱ���
    {
        // ����
        if (curTableArrow != TableArrow.none && isLean)
            transform.position = Vector3.MoveTowards(transform.position, tableVec, 1.5f * Time.deltaTime);
        // ����
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
        Debug.Log("����.");
        anim.SetTrigger("Lean");

        Vector3 playerVec = InGameManager.Instance.player.transform.position - transform.position;

        int index = -1; // �ִϸ��̼� ����
        moveVec = Vector3.zero; // ������ ����

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

        yield return new WaitForSeconds(0.75f); // ���ر��� �ɾ�� �ð�
        agent.isStopped = false;
        curStatus = EnemyStatus.Chase;
    }
    private void LeanAiming()
    {
        transform.localPosition = Vector3.MoveTowards(transform.position, moveVec, 3.0f * Time.deltaTime);
    }


    // ���߿� �׾����� ��� ����
    private void Die()
    {
        isDie = true;
        isDetect = false;
        cirCollider2D.enabled = false;
        agent.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Bullet �ױ׺��� ������(�Ѿ��� �� �ٸ����)
        if (collision.CompareTag("PlayerBullet"))
        {
            Damage(InGameManager.Instance.Power + DrugManager.Instance.power);
        }
    }
}
