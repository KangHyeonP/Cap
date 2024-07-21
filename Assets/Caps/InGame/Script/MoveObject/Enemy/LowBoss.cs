using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowBoss : Boss
{

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

            //selectPivot = Random.Range(1, 14);
            selectPivot = Random.Range(1, 3);
            Debug.Log("보스 스킬 사용");
            switch (selectPivot)
            {
                case 1:
                    BP1();
                    break;

                case 2:
                    BP2();
                    break;
                    /*
                case 3:
                    BP3();
                    break;

                case 4:
                    BP4();
                    break;

                case 5:
                    BP5();
                    break;

                case 6:
                    BP6();
                    break;

                case 7:
                    BP7();
                    break;

                case 8:
                    BP8();
                    break;

                case 9:
                    BP9();
                    break;

                case 10:
                    BP10();
                    break;

                case 11:
                    BP11();
                    break;

                case 12:
                    BP12();
                    break;

                case 13:
                    BP13();
                    break;
                    */

            }
            
        }
    }
    
}
