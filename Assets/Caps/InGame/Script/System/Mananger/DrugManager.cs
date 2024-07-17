using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public int[] tempStackDrug = { 0, 0, 0, 0, 0 }; // 현재 락에서 먹은 마약 개수 체크
    public int[] stackDrug = { 0, 0, 0, 0, 0 };// red, orange, yellow, green, blue 순, 현재 락 전까지 먹은 마약 개수 
    public int fullStackDrug = 0; // 이전 락까지의 마약 총 누적량
    public int curStackDrug = 0; // 현재 락의 마약 개수

    private int duffIndex = -1;

    [SerializeField]
    private FirstNerf firstNerf;
    [SerializeField]
    private SecondNerf secondNerf;
    [SerializeField]
    private ThirdNerf thirdNerf;

    // 추후에 redLevel로 묶고 BuffSteps와 연동
    //Red 
    public bool red1;
    public bool red2;
    public bool red3;

    public bool MaxHPUp;
    public int power;
    public int powerUpValue;

    //Orange
    public bool orange1;
    public bool orange2;
    public bool orange3;

    public float aim;
    public bool isBulletSizeUp = false;
    public bool isBulletPass = false;
    public bool isBulletChase = false;

    //Yellow
    public bool yellow1;    
    public bool yellow2;
    public bool yellow3;
    
    public float playerAttackRange;
    public bool isDistanceDamage = false;
    public bool isBleeding = false;
    public bool isBomb = false;

    //Green
    public bool green1;
    public bool green2;
    public bool green3;

    public float speed;
    public bool isRollSpeedUp = false;
    public bool isBulletAvoid = false;
    public float timeValue = 1.0f;

    //Blue
    public bool blue1;
    public bool blue2;
    public bool blue3;

    public float playerAttackDelay;
    public bool lucianPassive;
    public float reloadSpeed;
    public int maxBullet;

    // 보류
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
        //Debug.Log("1 / 2.0f 값 : " + (1 / 2.0f)); // 변수 테스트
        //Debug.Log("1 * 0.5f 값 : " + (1 * 0.5f)); // 변수 테스트
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // DrugGague Check
    public void LockCheck(float gauge)
    {
        Debug.Log("락 체크");

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
            Debug.Log("락 실행");

            for (int i=0; i<tempStackDrug.Length; i++)
            {
                curStackDrug += tempStackDrug[i];   
            }

            LockActive();
        }
    }

    // 락 활성화, 만약 숫자가 겹침(빨 0~10까지, 주황 10~20까지, 값이 10이면 순차적으로 빨 실행)
    private void LockActive()
    {
        float curGauge = 0;// 시작 게이지 값
        float stackGauge = 0; // 현재 누적된 게이지 양
        float value = Random.Range(0.1f, 99.9f);

        Debug.Log("현재 랜덤 값 : " + value);

        for (int i=0; i<stackDrug.Length; i++)
        {
            stackGauge += 100 * ((tempStackDrug[i] + stackDrug[i] * 0.5f) / (curStackDrug + fullStackDrug * 0.5f));
            Debug.Log(i + " 인덱스의 범위 값 :  " + stackGauge);
            Debug.Log("curStackDrug + fullStackDrug * 0.5f : " + (curStackDrug + fullStackDrug * 0.5f));
            Debug.Log("tempStackDrug[i] + stackDrug[i] * 0.5f : " + (tempStackDrug[i] + stackDrug[i] * 0.5f));
 
            if (curGauge <= value && value <= stackGauge)
            {
                buffSteps[duffIndex] = (EDrugColor)i;
                colorBuffs[i].ExcuteBuff(duffIndex);
                Debug.Log("실행한 마약 :  " + (EDrugColor)i);
                break;
            }
            curGauge = stackGauge;
        }

        for(int i=0; i<stackDrug.Length; i++)
        {
            stackDrug[i] += tempStackDrug[i];
            tempStackDrug[i] = 0;
        }

        fullStackDrug += curStackDrug;
        curStackDrug = 0;
    }
    
    // 락 해제기능 나올 시 UnBuff기능도 만들어야함

    // redbuffs (if문 추가 해야됨)
    public void RunRedBuff1()
    {
        InGameManager.Instance.MaxHPUpdate();
    }

    //체력이 깍이거나 회복할 때 로직 실행하게 구현
    public void RunRedBuff2()
    {
        switch (InGameManager.Instance.Hp)
        {
            case 4:
                powerUpValue = 5;
                break;
            case 3:
                powerUpValue = 15;
                break;
            case 2:
                powerUpValue = 30;
                break;
            case 1:
                powerUpValue = 50;
                break;
            default: powerUpValue = 0;
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
        if (orange2) isBulletPass = true;
    }

    public void RunOrangeBuff3()
    {
        if (orange3) isBulletChase = true;
    }

    //yellowBuff
    public void RunYellowBuff1()
    {
        if (yellow1) isDistanceDamage = true;
   
    }

    public void RunYellowBuff2()
    {
        if (yellow2) isBleeding = true;
    }
    public void RunYellowBuff3()
    {
        if (yellow3) isBomb = true;
    }

    //greenBuff
    public void RunGreenBuff1()
    {
        if (green1) isRollSpeedUp = true;
    }

    public void RunGreenBuff2()
    {
        if (green2) isBulletAvoid = true;

    }

    public void RunGreenBuff3()
    {
        if (green3)
        {
            Time.timeScale = 0.8f;
            //InGameManager.Instance.player.rollingSpeed *= 1.25f;
            //InGameManager.Instance.player.speedApply *= 1.25f;
            //InGameManager.Instance.player.speed = InGameManager.Instance.player.speedApply;
        }
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
