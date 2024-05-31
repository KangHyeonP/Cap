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

<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/DrugManager.cs
=======
    [SerializeField]
    private FirstNerf firstNerf;
    [SerializeField]
    private SecondNerf secondNerf;
    [SerializeField]
    private ThirdNerf thirdNerf;

>>>>>>> feature/TES-18_데이터_시스템_개발하기:Assets/Caps/InGame/Script/System/Mananger/DrugManager.cs
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

<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/DrugManager.cs
    //1st Nerf
    public bool firstNerf1;
    public bool firstNerf2;
    public bool firstNerf3;
    
    public bool isBandage;
    public int BandageNerf;

    //2nd Nerf
    public bool secondNerf1;
    public bool secondNerf2;
    public bool secondNerf3;
    public bool guageUp;

    //3rd Nerf
    public bool thirdNerf1;
    public bool thirdNerf2;
    public bool thirdNerf3;
    public bool isRollBan;
    public int doubleDamagePivot = 1;
=======
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

>>>>>>> feature/TES-18_데이터_시스템_개발하기:Assets/Caps/InGame/Script/System/Mananger/DrugManager.cs

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

<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/DrugManager.cs
=======
    // DrugGague Check
    public void LockCheck(float gauge)
    {

    }

>>>>>>> feature/TES-18_데이터_시스템_개발하기:Assets/Caps/InGame/Script/System/Mananger/DrugManager.cs
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

<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/DrugManager.cs
    public void RunFirstNerf1() //1�ܰ� ����, ���� ���� ���� hosthate
    {
        if(firstNerf1)
        {
            Debug.Log("������������");
        }
        
    }

    public void RunFirstNerf2() // 1�ܰ� ����, ������ �ش� InfectedBandage
    {
        if(firstNerf2)
        {
            int infect = Random.Range(1, 11);
            if (infect == 1)
                Debug.Log("�ش� ��� ����");
        }
        
    }

    public void RunFirstNerf3() //1�ܰ� ����, �ҹ� ����ź BombMiss
    {
        if(firstNerf3)
        {
            int bombMissCheck = Random.Range(1, 11);
            if (bombMissCheck == 1)
            {
                Debug.Log("����ź �ҹ�");
            }
        }
    }

    public void RunSecondNerf1() //2�ܰ� ����, ���ູ�� ������ ���� GuageIncrease
    {
        if(secondNerf1)
        {
            guageUp = true;
        }    
    }

    public void RunSecondNerf2() //2�ܰ� ����, ���� ���� 
    {
        if(secondNerf2)
        {
            Debug.Log("���� ����");
        }
    }

    public void RunSecondNerf3() //2�ܰ� ����, ���� �̽� 
    {
        if (firstNerf2)
        {
            int miss = Random.Range(1, 101);
            if (miss >= 1 && miss <=15)
            {
                Debug.Log("�Ѿ� ������");
            }
                
        }
    }

    public void RunThirdNerf1() //3�ܰ� ����, ������ ����
    {
        if(thirdNerf1)
        {
            isRollBan = true;
        }
    }

    public void RunThirdNerf2() //���� ���� �� ��ü �̿� ������ ���� �Ұ���
    {
        if(thirdNerf2)
        {
            Debug.Log("������ �Ұ�");
        }
    }

    public void RunThirdNerf3() //������ 2��
    {
        if(thirdNerf3)
        {
            doubleDamagePivot = 2;
        }
    }
=======
>>>>>>> feature/TES-18_데이터_시스템_개발하기:Assets/Caps/InGame/Script/System/Mananger/DrugManager.cs

}
