using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;


public class Agent : AI
{
    
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

    // ���� ����
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
                Debug.Log("�̱��� ���");
                break;
        }
    }

    protected override IEnumerator IAttack()
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

    protected override void AttackLogic()
    {

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

    protected IEnumerator LeanCount()
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
    protected void LeanAiming()
    {
        transform.localPosition = Vector3.MoveTowards(transform.position, moveVec, 3.0f * Time.deltaTime);
    }
}
