using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;
using UnityEngine.UIElements;

public enum EThirdNerf
{
    RollBan, ItemBan, DoubleDamage
}

public class ThirdNerf : MonoBehaviour
{
    private bool isActive = false;
    private EThirdNerf thirdNerfValue;

    private void Awake()
    {
       
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    public void RunThirdNerf()
    {
        if (!isActive) return;

        thirdNerfValue = (EThirdNerf)Random.Range(0, System.Enum.GetValues(typeof(EThirdNerf)).Length);

        switch (thirdNerfValue)
        {
            case EThirdNerf.RollBan:
                RollBan();
                break;
            case EThirdNerf.ItemBan:
                ItemBan();
                break;
            case EThirdNerf.DoubleDamage:
                DoubleDamage();
                break;
        }

        isActive = true;
    }

    public void RollBan() // ������ ����
    {
        DrugManager.Instance.isRollBan = true;
        Debug.Log("������ ���� Ȱ��ȭ");
    }

    public void ItemBan() // ���� ���⸸ ��ü �� ���밡��
    {
        DrugManager.Instance.itemBanCheck = true;
        Debug.Log("������ �Ұ� Ȱ��ȭ");
    }

    public void DoubleDamage() // ������ 2��
    {
        DrugManager.Instance.doubleDamagePivot = 2;
        Debug.Log("������ 2�� Ȱ��ȭ");
    }
}
