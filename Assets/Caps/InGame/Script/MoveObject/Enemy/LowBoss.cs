using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowBoss : Boss
{
    [SerializeField]
    private EnemyGun gun;

    bool bossKey;
    int selectPivot = 0;
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
        BossInputKey();
        SelectBP();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void BossInputKey()
    {
        bossKey = Input.GetKeyDown(KeyCode.B);
    }

    public void SelectBP()
    {
        if (bossKey)
        {
            /*
            selectPivot = Random.Range(1, 14);
            Debug.Log("보스 스킬 사용");
            switch (selectPivot)
            {
                case 1:
                    BP1();
                    break;

                case 2:
                    BP2();
                    break;

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


            }
            */
        }
    }
    
    public void BP1() // 중간보스, 최종보스
    {
        Debug.Log("BP1");

        for (int i = 0; i < 36; i++)
        {
            Vector2 BP1bulletDir = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + (10 * i));

            GameObject BP1bulletcopy = Instantiate(bullet, muzzle.position, muzzle.rotation);
            BP1bulletcopy.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;
        }

    }
    
    public void BP2() // 좆밥보스, 최종보스
    {
        //경로는 미리 안나옴
        gun.ShotReady();
        Debug.Log("BP2");
    }

    /*
    public void BP3() // 중간보스
    {
        Debug.Log("BP3");
    }

    public void BP4() // 최종보스
    {
        Debug.Log("BP4");
    }

    public void BP5() // 중간보스, 최종보스
    {
        //일단 되긴했는데 뭔가 애매

        Debug.Log("BP5");

        transform.position = Vector2.MoveTowards(boss.transform.position, target.position, 800f * Time.deltaTime);
    }

    public void BP6() // 최종보스
    {
        //위치 조정 필요할 듯
        Instantiate(load, new Vector2(boss.transform.position.x - 5, boss.transform.position.y), boss.transform.rotation);
        Instantiate(load, new Vector2(boss.transform.position.x, boss.transform.position.y + 5), boss.transform.rotation);
        Instantiate(load, new Vector2(boss.transform.position.x, boss.transform.position.y - 5), boss.transform.rotation);
        Debug.Log("BP6");
    }

    public void BP7() // 중간보스
    {
        //수류탄 받아오기

        Debug.Log("BP7");
    }

    public void BP8() // 최종보스
    {
        //총알들이 일직선이 아닌 동시에 나감

        Debug.Log("BP8");

        for (int i = 0; i < 4; i++)
        {
            Vector2 BP1bulletDir_1 = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir_1;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z);

            GameObject BP1bulletcopy_1 = Instantiate(bullet, muzzle.position, muzzle.rotation);
            BP1bulletcopy_1.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

            Vector2 BP1bulletDir_2 = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir_2;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + 60);

            GameObject BP1bulletcopy_2 = Instantiate(bullet, muzzle.position, muzzle.rotation);
            BP1bulletcopy_2.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

            Vector2 BP1bulletDir_3 = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir_3;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + 120);

            GameObject BP1bulletcopy_3 = Instantiate(bullet, muzzle.position, muzzle.rotation);
            BP1bulletcopy_3.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

            Vector2 BP1bulletDir_4 = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir_4;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + 180);

            GameObject BP1bulletcopy_4 = Instantiate(bullet, muzzle.position, muzzle.rotation);
            BP1bulletcopy_4.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

            Vector2 BP1bulletDir_5 = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir_5;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + 240);

            GameObject BP1bulletcopy_5 = Instantiate(bullet, muzzle.position, muzzle.rotation);
            BP1bulletcopy_5.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

            Vector2 BP1bulletDir_6 = (target.position - muzzle.position).normalized;
            muzzle.up = BP1bulletDir_6;
            muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + 300);

            GameObject BP1bulletcopy_6 = Instantiate(bullet, muzzle.position, muzzle.rotation);
            BP1bulletcopy_6.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

            i++;
            Debug.Log("for문 돌아감");
        }



    }

    public void BP9() // 좆밥보스
    {
        Debug.Log("BP9");

        Vector2 BP9bulletDir_1 = (target.position - muzzle.position).normalized;
        muzzle.up = BP9bulletDir_1;
        muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + 0);

        GameObject BP9bulletcopy_1 = Instantiate(bullet, muzzle.position, muzzle.rotation);
        BP9bulletcopy_1.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

        Vector2 BP9bulletDir_2 = (target.position - muzzle.position).normalized;
        muzzle.up = BP9bulletDir_2;
        muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + 10);

        GameObject BP9bulletcopy_2 = Instantiate(bullet, muzzle.position, muzzle.rotation);
        BP9bulletcopy_2.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

        Vector2 BP9bulletDir_3 = (target.position - muzzle.position).normalized;
        muzzle.up = BP9bulletDir_3;
        muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z - 10);

        GameObject BP9bulletcopy_3 = Instantiate(bullet, muzzle.position, muzzle.rotation);
        BP9bulletcopy_3.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

        Vector2 BP9bulletDir_4 = (target.position - muzzle.position).normalized;
        muzzle.up = BP9bulletDir_4;
        muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z - 30);

        GameObject BP9bulletcopy_4 = Instantiate(bullet, muzzle.position, muzzle.rotation);
        BP9bulletcopy_4.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;

    }
    */
    public void BP10() // 최종보스
    {
        Debug.Log("BP10");
    }

    public void BP11() // 최종보스
    {
        Debug.Log("BP11");
    }

    public void BP12() // 최종보스
    {
        Debug.Log("BP12");
    }

    public void BP13() // 최종보스
    {
        Debug.Log("BP13");
    }
    protected override void AttackLogic()
    {

        //Vector2 bulletDir = (target.position - muzzle.position).normalized;
        //muzzle.up = bulletDir;
        //muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + Random.Range(-attackRecoil, attackRecoil + 1));

        //GameObject bulletcopy = Instantiate(bullet, muzzle.position, muzzle.rotation);
        //bulletcopy.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;
    }

}
