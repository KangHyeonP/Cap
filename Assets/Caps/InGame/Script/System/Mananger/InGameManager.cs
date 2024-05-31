using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance;
    public static InGameManager Instance => instance;

    [SerializeField]
    private GameObject[] prefabs;

    // �ӽ� �÷��̾� ���
    public Player player;

    [SerializeField]
    private Slider drugBar;
    public float DrugGague => drugBar.value;

    private bool isPause = false; //�ϴ� ����� �ΰ��� �����̹Ƿ� �Ͻ����� ����
    public bool IsPause => isPause;

    [SerializeField]
    private int hp;
    public int Hp => hp;

    private int maxHp;
    public int MaxHp => maxHp;

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
        GeneratePlayer();
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

        maxHp = GameManager.Instance.PlayerHp;
    }

    private void GeneratePlayer()
    {
        int index = 0;
        GameObject playerObj;

        switch (GameManager.Instance.selectCharacter)
        {
            case ECharacters.main:
                index = (int)ECharacters.main;
                break;
                /*case ECharacters.sub:

                    break;
                case ECharacters.c1:

                    break;
                case ECharacters.c2:

                    break;
                case ECharacters.c3:

                    break;*/      
        }

        playerObj = Instantiate(prefabs[index], transform.position, transform.rotation);
        player = playerObj.GetComponent<Player>();
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
        drugGuage += value;
        drugBar.value = drugGuage;

        DrugManager.Instance.LockCheck(DrugGague);
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

    public void Hit(int value)
    {
        if (hp - value <= 0)
        {
            hp = 0;
            Debug.Log("�����"); // �̰� ���߿�
        }
        else hp -= value;
    }
}
