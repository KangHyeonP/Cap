using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EDrugColor
{
    red, orange, yellow, green, blue
}

public class DrugManager : MonoBehaviour
{
    private static DrugManager instance;
    public static DrugManager Instance => instance;

    public EDrugColor[] buffSteps= {EDrugColor.red,EDrugColor.red,EDrugColor.red};
    public bool[] isBuffStepActive = { false, false, false};

    [SerializeField]
    private FirstNerf firstNerf;
    [SerializeField]
    private SecondNerf secondNerf;
    [SerializeField]
    private ThirdNerf thirdNerf;

    //Red
    public bool red1;
    public bool red2;
    public bool red3;

    public bool MaxHPUp;   
    public int playerAttackDamage;
    public int redBuffAttackDamagePivot;

    //Orange
    public bool orange1;
    public bool orange2;
    public bool orange3;

    public float playerAttackCorrectness;
    public bool isBulletSizeUp = false; //���� �� �ý��ۿ��� �籸��(��)

    //Yellow
    public bool yellow1;    
    public bool yellow2;
    public bool yellow3;
    
    public float playerAttackRange;
    public float firstYellowBuffDamage;
    float yellowDist;

    //Green
    public bool green1;
    public bool green2;
    public bool green3;

    public float playerAttackSpeed;
    public int firstGreenBuffMoveSpeed;

    //Blue
    public bool blue1;
    public bool blue2;
    public bool blue3;

    public bool lucianPassive;
    public float playerMoveSpeed;
    public float reloadSpeed;
    public int maxBullet;

    // ����
    public bool hostHateCheck;
    public bool isBandage;
    public bool bandageNerf;
    public bool bombMissCheck;
    public bool guageUp;
    public bool colorBlindCheck;
    public bool aimMissCheck;
    public bool isRollBan;
    public bool itemBanCheck;
    public float doubleDamagePivot;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // DrugGague Check
    public void LockCheck(float gauge)
    {

    }

    // redbuffs (if�� �߰� �ؾߵ�)
    public void RunRedBuff1()
    {
        InGameManager.Instance.MaxHPUpdate();
    }

    //ü���� ���̰ų� ȸ���� �� ���� �����ϰ� ����
    public void RunRedBuff2()
    {
        switch (InGameManager.Instance.Hp)
        {
            case 4:
                redBuffAttackDamagePivot = 5;
                break;
            case 3:
                redBuffAttackDamagePivot = 15;
                break;
            case 2:
                redBuffAttackDamagePivot = 30;
                break;
            case 1:
                redBuffAttackDamagePivot = 50;
                break;
            default: redBuffAttackDamagePivot = 0;
                break;
        }
    }

    public void RunRedBuff3()
    {

    }

    // orangeBuff
    public void RunOrangeBuff1()
    {
        if(orange1)
            isBulletSizeUp = true;
    }

    public void RunOrangeBuff2()
    {

    }

    public void RunOrangeBuff3()
    {

    }

    //yellowBuff
    public void RunYellowBuff1(Vector3 enemyPos)
    {
        if(yellow1)
        {
            yellowDist = Vector3.Distance(InGameManager.Instance.player.transform.position, enemyPos);
            if (yellowDist >= 10)
            {
                firstYellowBuffDamage = 15;
            }

            else if (yellowDist < 10 && yellowDist >= 7)
            {
                firstYellowBuffDamage = 10;
            }

            else if (yellowDist < 7 && yellowDist >= 4)
            {
                firstYellowBuffDamage = 5;
            }
            else
            {
                firstYellowBuffDamage= 0;
            }
        }
        
    }

    public void RunYellowBuff2()
    {

    }
    public void RunYellowBuff3()
    {

    }

    //greenBuff
    public void RunGreenBuff1()
    {
        if(green1)
        {
            if(firstGreenBuffMoveSpeed == 0)
            {
                firstGreenBuffMoveSpeed = 5;
                Invoke("RunGreenBuff1", 3);
            }
            else
            {
                firstGreenBuffMoveSpeed= 0;                
            }            
        }
        
    }

    public void RunGreenBuff2()
    {
        if (green2)
        {
            int avoidChance = Random.Range(1, 101);
            if (avoidChance <= 15)
            {
                Debug.Log("ȸ��");
            }
        }

    }

    public void RunGreenBuff3()
    {

    }

    public void RunBlueBuff1()
    {
        if(blue1)
        {
            lucianPassive = true;
        }
    }
    public void RunBlueBuff2()
    {

    }
    public void RunBlueBuff3()
    {
        if(blue3)
        {
            reloadSpeed = 0.5f;
            maxBullet = 2;
        }
    }


}
