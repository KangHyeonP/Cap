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
    protected Transform muzzle;


    // ���� AI Ȱ��ȭ �������� ���, �������� or ���� ĭ ��ġ ���� �������� ������
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

    protected virtual void UpdateState(EnemyStatus enemy)
    {
        
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
        //Debug.Log("�߰� ����");

        anim.SetBool("Chase", true);

        float distance = Vector3.Distance(target.position, transform.position);
        if ((distance <= attackDistance) && (attackDelay <= curAttackDelay))
        {
            curStatus = EnemyStatus.Attack;
            anim.SetBool("Chase", false);
            //Debug.Log("�߰� ����");
        }
        agent.SetDestination(target.position);
    }

    protected void Attack()
    {
        if (IsAttack) return;

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

    public void Damage(int damage, WeaponValue value)
    {
       // Debug.Log("�¾Ҿ�" + value);

        if (value == WeaponValue.Gun)
        {
            damage += InGameManager.Instance.bulletPower;

            Debug.Log("�Ѿ� ���� ������ : " + damage);
        }

        if (DrugManager.Instance.red2)
        {
            damage = damage + damage * DrugManager.Instance.powerUpValue / 100;

            Debug.Log("������ ���� ������ : " + damage);
        }
        else if (DrugManager.Instance.isBleeding && damage >= maxHp * 0.3f)
        {
            StartCoroutine(Bleed());
        }

        if (DrugManager.Instance.isAnger)
        {
            // �������� �ְ� ī����, ù Ÿ���� �� �г밡 ������ 0���� ī���� �Ǿ� ���
            damage += (int)(damage * DrugManager.Instance.angerPower * 0.4f);
            DrugManager.Instance.AngryCount();

            Debug.Log("�г� ���� ������ : " + damage);
        }

        hp -= damage;
        Debug.Log("���� ������ : " + damage);
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

        for (int i = 0; i < 5; i++)
        {
            hp -= maxHp / 100;
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


    // ���߿� �׾����� ��� ����
    protected void Die()
    {
        isDie = true;
        isDetect = false;
        cirCollider2D.enabled = false;
        agent.enabled = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Bullet �ױ׺��� ������(�Ѿ��� �� �ٸ����)
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
