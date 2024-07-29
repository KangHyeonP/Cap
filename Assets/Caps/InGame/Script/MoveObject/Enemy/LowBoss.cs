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

    protected override void SelectBP()
    {
        if (bossKey)
        {
            int[] numbers = { 13 }; // 6, 7, 11�ؾ���
            //selectPivot = Random.Range(1, 14);
            selectPivot = numbers[Random.Range(0, numbers.Length)];
            Debug.Log("���� ��ų ���");
            muzzle.localRotation = Quaternion.Euler(0, 0, -90);
            switch (selectPivot)
            {
                case 1:
                    BP1();
                    break;

                case 2:
                    StartCoroutine("BP2");
                    break;

                case 3:
                    BP3();
                    break;

                case 4:
                    StartCoroutine("BP4");
                    break;

                case 5:
                    StartCoroutine("BP5");
                    break;
                /*
            case 6:
                BP6();
                break;

            case 7:
                BP7();
                break;
                */
                case 8:
                    StartCoroutine("BP8");
                    break;


                case 9:
                    StartCoroutine("BP9");
                    break;


                case 10:
                    StartCoroutine("BP10");
                    break;

                case 11:
                    BP11();
                    break;

                case 12:
                    StartCoroutine("BP12");
                    break;

                case 13:
                    StartCoroutine("BP13");
                    break;

            }

        }
    }
}
