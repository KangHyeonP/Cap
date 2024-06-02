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

    public  void GuageIncrease() // ���� ������ ����
    {
        DrugManager.Instance.gaugeUp = true;
        Debug.Log("���� ������ ����");
    }

    public  void BlackDrug() // ���� ����
    {
        DrugManager.Instance.colorBlindCheck = true;
        Debug.Log("���� ���� Ȱ��ȭ");
    }

    public void AimMiss() // ���� �̽�
    {
        DrugManager.Instance.aimMissCheck = true;
        Debug.Log("���� �̽� Ȱ��ȭ");
    }

}
