using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowBoss : Boss
{
    //nt cnt = 0;
    protected override void Awake()
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Mikazuki);
    }

    protected override IEnumerator IAttack()
    {
        //Debug.Log("���� ����");
        AttackLogic();
        //yield return new WaitForSeconds(attackDelay);

        switch (selectPivot)
        {
            case 2:
                yield return StartCoroutine("BP2");
                break;
            case 9:
                yield return StartCoroutine("BP9");
                break;
        }
        //Debug.Log("���� ��");

        curAttackDelay = 0;
        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
        bossAttack = false;
    }

    protected override void SelectBP()
    {
        int[] numbers = { 2 };
        //selectPivot = Random.Range(1, 14);
        selectPivot = numbers[Random.Range(0, numbers.Length)];
        Debug.Log("���� ��ų ���");
        muzzle.localRotation = Quaternion.Euler(0, 0, -90);
        
        /*switch (selectPivot)
        {
            case 2:
                StartCoroutine("BP2");
                break;
            case 9:
                StartCoroutine("BP9");
                break;
        }*/

    }
}
