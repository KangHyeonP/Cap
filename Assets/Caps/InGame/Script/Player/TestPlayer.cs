using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hp같은 정보는 어차피 싱글게임이라 게임매니저에서 관리해도 좋을듯

// 플레이어 방향 상태, 왼쪽은 오른쪽에서 뒤집기
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
    private int hp = 10; // 체력
    public int Hp => hp;

    private float speed = 3.0f; // 스피드
    private float power; // 공격력
    private float armor; // 방어력

    // 무기 구현이후 attackDelay알맞게 수정
    public float attackDelay = 1.0f;
    public float curAttackDelay;

    // Status - movement
    private PlayerVetor curVec = PlayerVetor.Down;

    private Vector2 inputVec;
    public Vector2 InputVec => inputVec;

    // Status - animation
    private PlayerAnimator curAnim = PlayerAnimator.Idle;

    private bool isReverse = false; // 캐릭터를 뒤집는 상태
    public bool IsReverse => isReverse;
    private bool isWalk = false;

    // Status - curStatus
    
    private bool isHit = false; // 피격당함
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
        // 만약 inputVec 0이면 Idle, 아니면 Run으로 체인지
        isWalk = InputVec != Vector2.zero ? true : false;
        
        Vector2 nextVect = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVect);

        anim.SetBool("Walk", isWalk);
    }

    public void Damage(int power)
    {
        if (!isDead) return;

        hp -= power;
        Debug.Log("피격당함");
        Debug.Log("현재 체력 : " + hp);
    }

    // 무기 기능 구현하여 추가하기
    private void Attack()
    {
        curAttackDelay += Time.deltaTime;

        if (isAttack && curAttackDelay >= attackDelay)
        {
            curAttackDelay = 0;
            StartCoroutine(IAttack());
        }
    }

    // 방향 전환
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
        
        Debug.Log("공격");
        yield return new WaitForSeconds(1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("플레이어에 들어온 트리거");
    }
}
