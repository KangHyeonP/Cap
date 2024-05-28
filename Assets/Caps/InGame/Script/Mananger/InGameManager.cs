using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance;
    public static InGameManager Instance => instance;

    // 임시 플레이어 등록
    public Player player;

    public Slider drugBar;

    private bool isPause = false; //일단 현재는 인게임 상태이므로 일시정지 해제
    public bool IsPause => isPause;

    [Header("플레이어")]
    private int hp;
    public int Hp => hp;

    private int maxHp;
    public int MaxHp => maxHp;

    // Item
    public bool isItem;
    //public bool IsItem => isItem;

    // 수정해야함
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
       
        drugGuage += value;
        drugBar.value = drugGuage;
        
    }
    public void UpdateKey()
    {
        // 이것도 UI
        //numKeyUI.text = "Key: " + numKey;
    }

    public void UpdateBomb()
    {
        // 이것도 UI
        //numBombUI.text = "Bomb: " + numBomb;
    }
    
    public void UpdateDrugType(Sprite s)
    {
        UIManager.Instance.inGameUI.DrugInven(s);
        // 이것도 UI
        //drugTypeUI.sprite = drugInven.drugSprite.sprite;
    }

    public void MaxHPUpdate()
    {
        if (DrugManager.Instance.MaxHPUp)
        {
            maxHp += 2;
            DrugManager.Instance.red1 = false;
        }
    }

    public void HealHp(int value)
    {
        if (hp + value > maxHp)
        {
            hp = maxHp;
        }
        else hp += value;
    }
}
