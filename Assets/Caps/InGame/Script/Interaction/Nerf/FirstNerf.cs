using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EFirstNerf
{
    HostHate, InfectedBandage, BombMiss
}

public class FirstNerf : MonoBehaviour
{
    private bool isActive = false;
    private EFirstNerf firstNerfValue;

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
        firstNerfValue = (EFirstNerf)Random.Range(0, System.Enum.GetValues(typeof(EFirstNerf)).Length);

        switch (firstNerfValue)
        {
            case EFirstNerf.HostHate:
                HostHate();
                break;
            case EFirstNerf.InfectedBandage:
                InfectedBandage();
                break;
            case EFirstNerf.BombMiss:
                BombMiss();
                break;
        }

        isActive = true;
    }

    public void HostHate() // 상점 비용증가
    {
        DrugManager.Instance.hostHateCheck = true;
        Debug.Log("상점가격증가");
    }

    public void InfectedBandage() // 오염된 붕대
    {
        DrugManager.Instance.bandageNerf = true;
        Debug.Log("붕대 오염 활성화");
    }

    public void BombMiss() // 불발수류탄
    {      
        DrugManager.Instance.bombMissCheck = true;
        Debug.Log("불발 수류탄 활성화");

        // 수류탄 만들고 거기에 집어넣기
        int bombMissCheck = Random.Range(1, 11);
        if (bombMissCheck == 1)
        {
            Debug.Log("수류탄 불발");
        }
    }
}
