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
    public ColorBuff[] colorBuffs;
    public bool[] isBuffStepActive = { false, false, false};
    public int[] stackDrug = { 0, 0, 0, 0, 0 };// red, orange, yellow, green, blue ��
    public int[] tempStackDrug = { 0, 0, 0, 0, 0 }; // ���� ������ ���� ���� ���� üũ
    public int fullStackDrug = 0; // ���� �� ������

    private int duffIndex = -1;

    [SerializeField]
    private FirstNerf firstNerf;
    [SerializeField]
    private SecondNerf secondNerf;
    [SerializeField]
    private ThirdNerf thirdNerf;

    // ���Ŀ� redLevel�� ���� BuffSteps�� ����
    //Red 
    public bool red1;
    public bool red2;
    public bool red3;

    public bool MaxHPUp;   
    public int power;
    public int redBuffAttackDamagePivot;

    //Orange
    public bool orange1;
    public bool orange2;
    public bool orange3;

    public float aim;
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

    public float playerAttackDelay;
    public int firstGreenBuffMoveSpeed;

    //Blue
    public bool blue1;
    public bool blue2;
    public bool blue3;

    public bool lucianPassive;
    public float speed;
    public float reloadSpeed;
    public int maxBullet;

    // ����
    public bool hostHateCheck;
    public bool isBandage;
    public bool bandageNerf;
    public bool bombMissCheck;
    public bool gaugeUp;
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
        Debug.Log("�� üũ");

        if (gauge >= 75)
        {
            duffIndex = 2;
        }
        else if (gauge >= 50)
        {
            duffIndex = 1;
        }
        else if (gauge >= 25)
        {
            duffIndex = 0;
        }
        else duffIndex = -1;

        if(duffIndex != -1 && !isBuffStepActive[duffIndex])
        {
            isBuffStepActive[duffIndex] = true;
            Debug.Log("�� ����");

            for (int i=0; i<tempStackDrug.Length; i++)
            {
                stackDrug[i] += tempStackDrug[i];
                fullStackDrug += stackDrug[i];
            }

            LockActive();
        }
    }

    // �� Ȱ��ȭ, ���� ���ڰ� ��ħ(�� 0~10����, ��Ȳ 10~20����, ���� 10�̸� ���������� �� ����)
    private void LockActive()
    {
        float curGauge = 0;// ���� ������ ��
        float stackGauge = 0; // ���� ������ ������ ��
        float value = Random.Range(0.1f, 100.0f);

        Debug.Log("���� ���� �� : " + value);

        for (int i=0; i<stackDrug.Length; i++)
        {
            stackGauge += 100 * (stackDrug[i] / (float)fullStackDrug);
            Debug.Log(i + " �ε����� ���� �� :  " + stackGauge);
            if (curGauge <= value && value <= stackGauge)
            {
                buffSteps[duffIndex] = (EDrugColor)i;
                colorBuffs[i].ExcuteBuff(duffIndex);
                Debug.Log("������ ���� :  " + (EDrugColor)i);
                break;
            }
            curGauge = stackGauge;
        }
    }
    
    // �� ������� ���� �� UnBuff��ɵ� ��������

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
