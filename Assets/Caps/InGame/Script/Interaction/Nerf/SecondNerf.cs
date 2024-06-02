using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ESecondNerf
{
    GuageIncrease, BlackDrug, AimMiss
}

public class SecondNerf : MonoBehaviour
{
    private bool isActive = false;
    private ESecondNerf secondNerfValue;

    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    public void RunFirstNerf()
    {
        if (!isActive) return;
        secondNerfValue = (ESecondNerf)Random.Range(0, System.Enum.GetValues(typeof(ESecondNerf)).Length);

        switch (secondNerfValue)
        {
            case ESecondNerf.GuageIncrease:
                GuageIncrease();
                break;
            case ESecondNerf.BlackDrug:
                BlackDrug();
                break;
            case ESecondNerf.AimMiss:
                AimMiss();
                break;
        }

        isActive = true;
    }

    public  void GuageIncrease() // 마약 게이지 증가
    {
        DrugManager.Instance.gaugeUp = true;
        Debug.Log("마약 게이지 증가");
    }

    public  void BlackDrug() // 마약 색맹
    {
        DrugManager.Instance.colorBlindCheck = true;
        Debug.Log("마약 색맹 활성화");
    }

    public void AimMiss() // 조준 미스
    {
        DrugManager.Instance.aimMissCheck = true;
        Debug.Log("조준 미스 활성화");
    }

}
