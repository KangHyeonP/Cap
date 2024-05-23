using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance;
    public static InGameManager Instance => instance;

    // �ӽ� �÷��̾� ���
    public Player player;

    private bool isPause = false; //�ϴ� ����� �ΰ��� �����̹Ƿ� �Ͻ����� ����
    public bool IsPause => isPause;

    [Header("�÷��̾�")]
    private int hp;
    public int Hp => hp;

    private int maxHp;

    // Item
    public bool isItem;
    //public bool IsItem => isItem;

    // �����ؾ���
    public Drug drugInven = null;
    //public Drug DrugInven => drugInven;

    public Item tempItem = null;
    //public Item TempItem => tempItem;

    public Drug tempDrug = null;
    //public Drug TempDrug => tempDrug;

    public int drugGuage;
    //public int DrugGuage => drugGuage;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        if (Instance == null)
        {
            instance = this;
        }
    }

    public void Pause(bool check)
    {
        isPause = check;

        if (isPause) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    public void UpdateDrug(int value)
    {
        // UI �Ŵ������� ����
        /*
        drugGuage += value;
        drugBar.value = drugGuage;
        */
    }
    public void UpdateKey()
    {
        // �̰͵� UI
        //numKeyUI.text = "Key: " + numKey;
    }

    public void UpdateBomb()
    {
        // �̰͵� UI
        //numBombUI.text = "Bomb: " + numBomb;
    }
    
    public void UpdateDrugType(Sprite s)
    {
        UIManager.Instance.inGameUI.DrugInven(s);
        // �̰͵� UI
        //drugTypeUI.sprite = drugInven.drugSprite.sprite;
    }
}
