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

    public void HostHate() // ���� �������
    {
        DrugManager.Instance.hostHateCheck = true;
        Debug.Log("������������");
    }

    public void InfectedBandage() // ������ �ش�
    {
        DrugManager.Instance.bandageNerf = true;
        Debug.Log("�ش� ���� Ȱ��ȭ");
    }

    public void BombMiss() // �ҹ߼���ź
    {      
        DrugManager.Instance.bombMissCheck = true;
        Debug.Log("�ҹ� ����ź Ȱ��ȭ");

        // ����ź ����� �ű⿡ ����ֱ�
        int bombMissCheck = Random.Range(1, 11);
        if (bombMissCheck == 1)
        {
            Debug.Log("����ź �ҹ�");
        }
    }
}
