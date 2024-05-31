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

    public void RollBan() // 구르기 금지
    {
        DrugManager.Instance.isRollBan = true;
        Debug.Log("구르기 금지 활성화");
    }

    public void ItemBan() // 마약 무기만 교체 및 복용가능
    {
        DrugManager.Instance.itemBanCheck = true;
        Debug.Log("아이템 불가 활성화");
    }

    public void DoubleDamage() // 데미지 2배
    {
        DrugManager.Instance.doubleDamagePivot = 2;
        Debug.Log("데미지 2배 활성화");
    }
}
