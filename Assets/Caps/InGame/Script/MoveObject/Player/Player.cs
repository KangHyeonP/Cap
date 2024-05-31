using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// hp∞∞¿∫ ¡§∫∏¥¬ æÓ¬˜«« ΩÃ±€∞‘¿”¿Ã∂Û ∞‘¿”∏≈¥œ¿˙ø°º≠ ∞¸∏Æ«ÿµµ ¡¡¿ªµÌ

// «√∑π¿ÃæÓ πÊ«‚ ªÛ≈¬, øﬁ¬ ¿∫ ø¿∏•¬ ø°º≠ µ⁄¡˝±‚
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

    protected float speed = 3.0f; // Ω∫««µÂ
    protected float rollingSpeed = 1.0f;
    protected float power; // ∞¯∞›∑¬
    protected int armor; // πÊ≈∫

    // π´±‚ ±∏«ˆ¿Ã»ƒ attackDelayæÀ∏¬∞‘ ºˆ¡§
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

    public bool isReverse = false; // ƒ≥∏Ø≈Õ∏¶ µ⁄¡˝¥¬ ªÛ≈¬, √ﬂ»ƒø° ∫Ø∞Ê∏¯«œ∞‘ ºˆ¡§
    public bool IsReverse => isReverse;
    protected bool isWalk = false;

    // Status - curStatus
    protected bool isHit = false; // ««∞›¥Á«‘
    protected bool isDead;
    protected bool isRoll = false;
    protected bool rollReverse = false; // æ÷¥œ∏ﬁ¿Ãº« ¥Îƒ™

    // InputKey
    protected bool attackKey;
    protected bool skillKey;
    protected bool rollKey;
    protected bool swapKey1;
    protected bool swapKey2;
    protected bool swapKey3;
    protected bool swapKey4;
    protected bool interaction;
    protected bool qKey;
    protected bool shiftKey;

    // Weapon
    protected int weaponIndex = 0;
    public string[] weapon;

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
            // √ﬂ»ƒ ∞‘¿” ∏≈¥œ¿˙∑Œ ∫Ø∞Êµ… ∞°¥…º∫ ¿÷¿Ω
            Interaction();
            EatDrug();
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
        swapKey1 = Input.GetButtonDown("Swap1");
        swapKey2 = Input.GetButtonDown("Swap2");
        swapKey3 = Input.GetButtonDown("Swap3");
        swapKey4 = Input.GetButtonDown("Swap4");
        interaction = Input.GetButtonDown("Interaction");
        qKey = Input.GetKeyDown(KeyCode.Q);
        shiftKey = Input.GetKey(KeyCode.LeftShift);
    }

    protected void Move()
    {
        // ∏∏æ‡ inputVec 0¿Ã∏È Idle, æ∆¥œ∏È Run¿∏∑Œ √º¿Œ¡ˆ
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
        if (DrugManager.Instance.isRollBan)
        {
            return;
        }

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

        // ±∏∏£±‚ πÊ«‚ ∞ËªÍ
        if(rollVec.x > 0)
        {
            if (rollVec.y > 0) rollStatus = "RollCross";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollSide";
        }
        else if(rollVec.x ==0)
        {
            if (rollVec.y > 0) rollStatus = "RollBack";
            else if(rollVec.y < 0) rollStatus = "RollFront";
        }
        else
        {
            rollReverse = true;
            if (rollVec.y > 0) rollStatus = "RollCross";
            else if (rollVec.y == 0) rollStatus = "RollSide";
            else rollStatus = "RollSide";
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
<<<<<<< HEAD:Assets/Caps/InGame/Script/Player/Player.cs
        //if (!isDead) return;
        power *= DrugManager.Instance.doubleDamagePivot;

        if (DrugManager.Instance.green2)
        {
            DrugManager.Instance.RunGreenBuff2();
        }

        if(DrugManager.Instance.red2)
        {
            DrugManager.Instance.RunRedBuff2();
        }


        
=======
        if (isDead) return;

        // »∏«« ø©∫Œ √º≈©
        if (DrugManager.Instance.green2)
        {
            DrugManager.Instance.RunGreenBuff2();
            return; // ªÛ≈¬∞¸∑√«ÿº≠ ºˆ¡§«“∞‘ ¿÷¿∏∏È ºˆ¡§«œ∞Ì ∏Æ≈œ
        }

        InGameManager.Instance.Hit(power);
        UIManager.Instance.hpUpdate();

        // ±§¿¸ªÁ ø©∫Œ √º≈©
        if (DrugManager.Instance.red2)
        {
            DrugManager.Instance.RunRedBuff2();
        }
>>>>>>> feature/TES-18_Îç∞Ïù¥ÌÑ∞_ÏãúÏä§ÌÖú_Í∞úÎ∞úÌïòÍ∏∞:Assets/Caps/InGame/Script/MoveObject/Player/Player.cs
    }

    // π´±‚ ±‚¥… ±∏«ˆ«œø© √ﬂ∞°«œ±‚
    protected void Attack()
    {
        curAttackDelay += Time.deltaTime;

        if (attackKey && curAttackDelay >= attackDelay)
        {
            curAttackDelay = 0;
            StartCoroutine(IAttack());
        }
    }

    protected void Swap()
    {
        int curIndex = -1;

        if (!swapKey1 && !swapKey2 && !swapKey3) return;

        if (swapKey1)
            curIndex = 0;

        if (swapKey2)
            curIndex = 1;

        if (swapKey3)
            curIndex = 2;

        if (curIndex != weaponIndex)
        {
            weaponIndex = curIndex;
            Debug.Log(weapon[weaponIndex]);
        }
    }

    protected void Interaction()
    {
        if (interaction && InGameManager.Instance.isItem == true)
        {
            InGameManager.Instance?.tempItem.GetItem();
        }
    }
    protected void EatDrug()
    {
        if (qKey)
        {
            // ¿Œ∫• ∏∂æ‡
            if (shiftKey && InGameManager.Instance.drugInven != null)
            {
                InGameManager.Instance.drugInven.UseItem();
                UIManager.Instance.inGameUI.DrugInven(null);
            }
            // ∂•ø°¿÷¥¬ ∏∂æ‡
            else if (InGameManager.Instance.tempDrug != null && !shiftKey)
            {
                InGameManager.Instance.tempDrug.UseItem();
            }
        }
    }

    // πÊ«‚ ¿¸»Ø
    public void ChangeVector(PlayerVetor pVec, bool checkReverse)
    {
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

        Debug.Log("∞¯∞›");
        yield return new WaitForSeconds(1.0f);
    }

    protected void Skill()
    {
        if (!skillKey || !UIManager.Instance.inGameUI.skillUI.CanUseSkill) return;

        StartCoroutine(ESkill());
    }
    
    // Ω∫≈≥ Ω√¿€∞˙ ≥°¿ª ∞ËªÍ
    private IEnumerator ESkill()
    {
        yield return null;
        // ƒ´∏ﬁ∂Û ∞Ì¡§ π◊ ¿ßƒ° ¿Ãµø
        CameraController.Instance.CameraActive(false);
        CameraController.Instance.CameraPos(transform.position.x, transform.position.y);
        anim.SetTrigger("Skill");

        // ¿œΩ√ µø¿€ ¡§¡ˆ
        InGameManager.Instance.Pause(true);

        UIManager.Instance.inGameUI.skillUI.UseSkill();
        PlayerSkill();

        yield return new WaitForSecondsRealtime(1.0f); // ≈∏¿”Ω∫ƒ…¿œ øµ«‚ x
        InGameManager.Instance.Pause(false);
        CameraController.Instance.CameraActive(true);
    }

    protected abstract void PlayerSkill();

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet") //ºˆ¡§
        {
            Debug.Log("√—æÀ ¥Í¿Ω");
            Destroy(collision.gameObject);
            Damage(1); // µ•πÃ¡ˆ ∑Œ¡˜ ≥™¡ﬂø° ºˆ¡§
        }
    }
}
