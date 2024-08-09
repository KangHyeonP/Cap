using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;


public class Agent : AI
{
    public float[] tableValue = { 0, 0 };


    protected override  void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    // 떨림 방지
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void UpdateState(EnemyStatus enemy)
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

    protected override IEnumerator IAttack()
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

    protected override void AttackLogic()
    {

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

        isDetect = false; //추가함
        agent.isStopped = true;
        StartCoroutine(LeanCount());
    }

    protected IEnumerator LeanCount()
    {
        //Debug.Log("기대다.");
        anim.SetTrigger("Lean");

        Vector3 playerVec = InGameManager.Instance.player.transform.position - transform.position;

        moveVec = Vector3.zero; // 움직일 방향

        switch (curTableArrow)
        {
            case TableArrow.up:
                if (playerVec.x <= 0)
                {
                    moveVec = new Vector3(-tableValue[0], 0,1);
                }
                else
                {
                    moveVec = new Vector3(tableValue[0], 0,1);
                }
                break;
            case TableArrow.down:
                if (playerVec.x <= 0)
                {
                    moveVec = new Vector3(-tableValue[0], 0,1);
                }
                else
                {
                    moveVec = new Vector3(tableValue[0], 0,1);
                }
                break;
            case TableArrow.left:
                if (playerVec.y <= 0)
                {
                    moveVec = new Vector3(0, -tableValue[1],1);
                }
                else
                {
                    moveVec = new Vector3(0, tableValue[1],1);
                }
                break;
            case TableArrow.right:
                if (playerVec.y <= 0)
                {
                    moveVec = new Vector3(0, -tableValue[1],1);
                }
                else
                {
                    moveVec = new Vector3(0, tableValue[1],1);
                }
                break;
        }

        yield return new WaitForSeconds(1.0f);
        isLean = false;

        yield return new WaitForSeconds(0.75f); // 조준까지 걸어가는 시간
        isDetect = true; //추가함
        agent.isStopped = false;
        curStatus = EnemyStatus.Chase;
    }
    protected void LeanAiming()
    {
        //transform.Translate(moveVec * Time.unscaledDeltaTime);
        /*if(isDetect) return;
        transform.position += moveVec;
        isDetect = true;*/
        transform.position = Vector3.MoveTowards(transform.position, (transform.position + moveVec), 5.0f * Time.unscaledDeltaTime);
        Debug.Log("나가는 방향 moveVec" + moveVec);
        Debug.Log("계산 방향 :" + (transform.position + moveVec));
        Debug.Log("현재 좌표 :" + transform.position);
    }
}
