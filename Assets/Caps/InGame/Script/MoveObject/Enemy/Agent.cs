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
    protected float attackMoveDelay; // ���� �� ���� �ð�
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

    protected Vector3 moveVec; // lean���¿��� ������ ����
    protected Vector3 tableVec; // table ��ġ

    // AI �ӽ� ������
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
        // anim = GetComponent<Animator>(); AI �׸� ������ �����Ұ���
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

        // ���� �ִϸ��̼����� �����Ұ���
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
                Debug.Log("�̱��� ���");
                break;
        }
    }

    // ���⺰ ��������Ʈ ����, ���� �ִϸ��̼��� ������ �Ŷ� �ڵ� �����ʿ���
    protected void ChangeSprite()
    {
        if (agentAngleIndex == 3) agentAngleIndex = 2;
        spritesRenderer.sprite = basicSprites[agentAngleIndex];
    }

    // �÷��̾��� ���� ���
    protected void AgentAngle()
    {
        agentAngleValue = AgentVector();
        agentAngleIndex = AngleCalculate(agentAngleValue); // up(�ĸ�), down(����), left(����), right(������)
    }

    protected float AgentVector()
    {
        Vector3 value = InGameManager.Instance.player.transform.position - transform.position;
        // ���� �ڱ� �ڽ��� �������� �÷��̾��� ��ġ�� ����Ͽ� ��� ������ �ٶ�����ϴ��� ������
        float angle;
        angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg; // ������ 180 ~ -180

        return angle;
    }

    protected int AngleCalculate(float angleValue)
    {
        // �Ƹ� �ش� �̹����� �־������ �˵�

        int Index = -1;
        // 1��и�, �� �� �밢���� �켱����
        if (angleValue <= 135f && angleValue > 45f) Index = 0; // up
        // 2��и�, ���� �� �밢���� �켱����
        else if (angleValue <= 45f && angleValue > -45f) Index = 3; // right
        // 3��и�, ���� �Ʒ��밢���� �켱
        else if (angleValue <= -45f && angleValue > -135f) Index = 1; // down 
        // 4��и�, ���� �Ʒ��밢���� �켱
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
        }*/ //�� �����̰� �������� �ʿ� ������?

        if (isDetect)
        {
            curStatus = EnemyStatus.Chase;
            return;
        }


        // ���ڸ� �ִϸ��̼� ����
        
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
        
        agent.SetDestination(target.position);
        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay)) curStatus = EnemyStatus.Attack;
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
        yield return new WaitForSeconds(attackMoveDelay);
        // yield return new WaitForSeconds(0.5f); // ����� ��� ����

        AttackLogic();

        //yield return new WaitForSeconds(attackMoveDelay);

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

        hp--;
        Debug.Log("���� ���� ü��: " + hp);

        if (hp == 0)
        {
            RoomController.Instance.ClearRoomCount();
            Destroy(gameObject);
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


    /*private void UpdateLean()
    {
        if (!isMoveLean) return;
        transform.localPosition = Vector3.MoveTowards(transform.position, moveVec, 1.0f);
    }*/


    // ���߿� �׾����� ��� ����
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
