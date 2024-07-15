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

    // 임시 플레이어 등록
    public Player player;

    [SerializeField]
    private Slider drugBar;
    public float DrugGague => drugBar.value;

    private bool isPause = false; //일단 현재는 인게임 상태이므로 일시정지 해제
    public bool IsPause => isPause;

    // Player Status
    [SerializeField]
    private int hp;
    public int Hp => hp;

    private int maxHp;
    public int MaxHp => maxHp;

    private float speed;
    public float Speed => speed;

    private int power;
    public int Power => power;

    public int[] weaponDamage; // 라이플, 샷건, 스나, 권총 순
    public int weaponIndex = 0; // 스왑 및 데미지 적용으로 사용 예정 변수들

    private float aim;
    public float Aim => aim;

    private float attackDelay;
    public float AttackDelay => attackDelay;

    public int bulletPower;

    private bool isDead;
    public bool IsDead => isDead;

    // Item
    public bool isItem;

    // 수정해야함
    public Drug drugInven = null;

    public Item tempItem = null;

    // 나중에 Stack형식으로 바꿔야할듯?
    public Drug tempDrug = null;

    public int drugGuage;

    public int grenadeCount;
    public Stack<Item> grenades = new Stack<Item>();
    public GameObject grenadeObj;


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
        hp = maxHp;
        speed = GameManager.Instance.PlayerSpeed;
        power = GameManager.Instance.PlayerAttackPower;
        aim = GameManager.Instance.PlayerAimAccuracy;
        attackDelay = GameManager.Instance.PlayerAttackDelay;
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
        // UI 매니저에서 수정
        drugGuage += value;
        drugBar.value = drugGuage;

        DrugManager.Instance.LockCheck(DrugGague);
    }
    public void UpdateKey()
    {
        // 이것도 UI
        //numKeyUI.text = "Key: " + numKey;
    }

    public void UpdateGrenade()
    {
        // 이것도 UI
        //numBombUI.text = "Bomb: " + numBomb;
    }

    public void UseGrenade()
    {
        if(grenadeCount > 0)
        {
            grenadeCount--;
            grenades.Pop().UseItem();
            //grenades[grenadeCount].UseItem();
        }
    }
    
    /*public void UpdateDrugType(Sprite s)
    {
        UIManager.Instance.inGameUI.DrugInven(s);
        // 이것도 UI
        //drugTypeUI.sprite = drugInven.drugSprite.sprite;
    }*/
    public void MaxHPUpdate()
    {
        // 만약 락 해제 기능 나오면 로직 수정해야함
        maxHp += 2;
        UIManager.Instance.hpInit();
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
            GameOver();
        }
        else hp -= value;
    }

    // 추후 사망로직 추가하기
    private void GameOver()
    {
        player.GetComponent<BoxCollider2D>().enabled = false;
    }
}
