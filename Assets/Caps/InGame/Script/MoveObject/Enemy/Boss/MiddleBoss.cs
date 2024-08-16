using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBoss : Boss
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

    protected override IEnumerator IAttack()
    {
        //Debug.Log("공격 실행");
        AttackLogic();
        //yield return new WaitForSeconds(attackDelay);

        switch (selectPivot)
        {
            case 1:
                BP1();
                yield return new WaitForSeconds(1);
                break;
            case 2:
                yield return StartCoroutine("BP2");
                break;
            case 3:
                BP3();
                yield return new WaitForSeconds(1);
                break;
            case 5:
                yield return StartCoroutine("BP5");
                break;
            /*
    case 6:
        BP6();
        break;

    case 7:
        BP7();
        break;
        */
            case 9:
                yield return StartCoroutine("BP9");
                break;
        }
        //Debug.Log("공격 끝");

        curAttackDelay = 0;
        isAttack = false;
        agent.isStopped = false;
        isDetect = true;
        curStatus = EnemyStatus.Idle;
        bossAttack = false;
    }

    protected override void SelectBP()
    {
        int[] numbers = { 5 };
        //int[] numbers = { 1, 2, 3, 5, 9 }; // 7해야함
        //selectPivot = Random.Range(1, 14);
        selectPivot = numbers[Random.Range(0, numbers.Length)];
        Debug.Log("보스 스킬 사용");
        muzzle.localRotation = Quaternion.Euler(0, 0, -90);
    }
}