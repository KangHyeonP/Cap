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

    // ÀÓ½Ã ÇÃ·¹ÀÌ¾î µî·Ï
    public Player player;

<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/InGameManager.cs
    public Slider drugBar;
=======
    [SerializeField]
    private Slider drugBar;
    public float DrugGague => drugBar.value;
>>>>>>> feature/TES-18_ë°ì´í„°_ì‹œìŠ¤í…œ_ê°œë°œí•˜ê¸°:Assets/Caps/InGame/Script/System/Mananger/InGameManager.cs

    private bool isPause = false; //ÀÏ´Ü ÇöÀç´Â ÀÎ°ÔÀÓ »óÅÂÀÌ¹Ç·Î ÀÏ½ÃÁ¤Áö ÇØÁ¦
    public bool IsPause => isPause;

    [SerializeField]
    private int hp;
    public int Hp => hp;

    private int maxHp;
    public int MaxHp => maxHp;

    // Item
    public bool isItem;
    //public bool IsItem => isItem;

    // ¼öÁ¤ÇØ¾ßÇÔ
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
<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/InGameManager.cs
       
        drugGuage += value;
        drugBar.value = drugGuage;
        
=======
        // UI ¸Å´ÏÀú¿¡¼­ ¼öÁ¤
        drugGuage += value;
        drugBar.value = drugGuage;

        DrugManager.Instance.LockCheck(DrugGague);
>>>>>>> feature/TES-18_ë°ì´í„°_ì‹œìŠ¤í…œ_ê°œë°œí•˜ê¸°:Assets/Caps/InGame/Script/System/Mananger/InGameManager.cs
    }
    public void UpdateKey()
    {
        // ÀÌ°Íµµ UI
        //numKeyUI.text = "Key: " + numKey;
    }

    public void UpdateBomb()
    {
        // ÀÌ°Íµµ UI
        //numBombUI.text = "Bomb: " + numBomb;
    }
    
    public void UpdateDrugType(Sprite s)
    {
        UIManager.Instance.inGameUI.DrugInven(s);
        // ÀÌ°Íµµ UI
        //drugTypeUI.sprite = drugInven.drugSprite.sprite;
    }
<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/InGameManager.cs

=======
>>>>>>> feature/TES-18_ë°ì´í„°_ì‹œìŠ¤í…œ_ê°œë°œí•˜ê¸°:Assets/Caps/InGame/Script/System/Mananger/InGameManager.cs
    public void MaxHPUpdate()
    {
        if (DrugManager.Instance.MaxHPUp)
        {
            maxHp += 2;
            DrugManager.Instance.red1 = false;
        }
    }
<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/InGameManager.cs

=======
>>>>>>> feature/TES-18_ë°ì´í„°_ì‹œìŠ¤í…œ_ê°œë°œí•˜ê¸°:Assets/Caps/InGame/Script/System/Mananger/InGameManager.cs
    public void HealHp(int value)
    {
        if (hp + value > maxHp)
        {
            hp = maxHp;
        }
        else hp += value;
    }
<<<<<<< HEAD:Assets/Caps/InGame/Script/Mananger/InGameManager.cs
=======

    public void Hit(int value)
    {
        if (hp - value <= 0)
        {
            hp = 0;
            Debug.Log("»ç¸ÁÇÔ"); // ÀÌ°Ç ³ªÁß¿¡
        }
        else hp -= value;
    }
>>>>>>> feature/TES-18_ë°ì´í„°_ì‹œìŠ¤í…œ_ê°œë°œí•˜ê¸°:Assets/Caps/InGame/Script/System/Mananger/InGameManager.cs
}
