using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hp���� ������ ������ �̱۰����̶� ���ӸŴ������� �����ص� ������

// �÷��̾� ���� ����, ������ �����ʿ��� ������
public enum PlayerVetor
{
    Up,UpRight,Right,Down,
}

public enum PlayerAnimator
{
    Idle, Run
}

public class TestPlayer : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    // Status
    // Status - Basic
    [SerializeField]
    private int hp = 10; // ü��
    public int Hp => hp;

    private float speed = 3.0f; // ���ǵ�
    private float power; // ���ݷ�
    private float armor; // ����

    // ���� �������� attackDelay�˸°� ����
    public float attackDelay = 1.0f;
    public float curAttackDelay;

    // Status - movement
    private PlayerVetor curVec = PlayerVetor.Down;

    private Vector2 inputVec;
    public Vector2 InputVec => inputVec;

    // Status - animation
    private PlayerAnimator curAnim = PlayerAnimator.Idle;

    private bool isReverse = false; // ĳ���͸� ������ ����
    public bool IsReverse => isReverse;
    private bool isWalk = false;

    // Status - curStatus
    
    private bool isHit = false; // �ǰݴ���
    public bool isDead;


    // InputKey
    public bool isAttack;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        InputKey();
        VectorStatus(curVec);
    }

    private void FixedUpdate()
    {
        Move();
        Attack();
    }
    private void InputKey()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        isAttack = Input.GetButton("Fire1");
    }

    private void Move()
    {
        // ���� inputVec 0�̸� Idle, �ƴϸ� Run���� ü����
        isWalk = InputVec != Vector2.zero ? true : false;
        
        Vector2 nextVect = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVect);

        anim.SetBool("Walk", isWalk);
    }

    public void Damage(int power)
    {
        if (!isDead) return;

        hp -= power;
        Debug.Log("�ǰݴ���");
        Debug.Log("���� ü�� : " + hp);
    }

    // ���� ��� �����Ͽ� �߰��ϱ�
    private void Attack()
    {
        curAttackDelay += Time.deltaTime;

        if (isAttack && curAttackDelay >= attackDelay)
        {
            curAttackDelay = 0;
            StartCoroutine(IAttack());
        }
    }

    // ���� ��ȯ
    public void ChancgVector(PlayerVetor pVec, bool checkReverse)
    {
        isReverse = checkReverse;
        VectorStatus(pVec);
    }

    private void VectorStatus(PlayerVetor pVec)
    {
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
        for(int i=0;i<4;i++)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�÷��̾ ���� Ʈ����");
    }
}
