using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hp���� ������ ������ �̱۰����̶� ���ӸŴ������� �����ص� ������

// �÷��̾� ���� ����, ������ �����ʿ��� ������
public enum PlayerVetor
{
    Up, UpRight, Right, Down,
}

public enum PlayerAnimator
{
    Idle, Run
}

public abstract class Player : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;

    // Status
    // Status - Basic
    [SerializeField]
    protected int hp = 10; // ü��
    public int Hp => hp;

    protected float speed = 3.0f; // ���ǵ�
    protected float power; // ���ݷ�
    protected float armor; // ����

    // ���� �������� attackDelay�˸°� ����
    protected float attackDelay = 1.0f;
    protected float curAttackDelay;

    [SerializeField]
    protected float skillDelay = 15f;
    public float SkillDelay => skillDelay;

    // Status - movement
    protected PlayerVetor curVec = PlayerVetor.Down;

    protected Vector2 inputVec;
    public Vector2 InputVec => inputVec;

    // Status - animation
    protected PlayerAnimator curAnim = PlayerAnimator.Idle;

    public bool isReverse = false; // ĳ���͸� ������ ����, ���Ŀ� ������ϰ� ����
    public bool IsReverse => isReverse;
    protected bool isWalk = false;

    // Status - curStatus

    protected bool isHit = false; // �ǰݴ���
    protected bool isDead;


    // InputKey
    protected bool isAttack;
    protected bool isSkill;

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
        if (!InGameManager.Instance.IsPause)
        {
            InputKey();
            VectorStatus(curVec);
            Attack();
            Skill();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!InGameManager.Instance.IsPause)
        {
            Move();
        }
    }

    protected void InputKey()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
        isAttack = Input.GetButton("Fire1");
        isSkill = Input.GetButtonDown("Jump");
    }

    protected void Move()
    {
        // ���� inputVec 0�̸� Idle, �ƴϸ� Run���� ü����
        isWalk = InputVec != Vector2.zero ? true : false;

        Vector2 nextVect = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVect);

        anim.SetBool("Walk", isWalk);
    }

    protected void Damage(int power)
    {
        if (!isDead) return;

        hp -= power;
        Debug.Log("�ǰݴ���");
        Debug.Log("���� ü�� : " + hp);
    }

    // ���� ��� �����Ͽ� �߰��ϱ�
    protected void Attack()
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

    protected void VectorStatus(PlayerVetor pVec)
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
        if (!isSkill || !UIManager.Instance.inGameUI.skillUI.CanUseSkill) return;

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
        Debug.Log("�÷��̾ ���� Ʈ����");
    }
}
