using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponValue
{
    Knife, Gun
}

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

    [SerializeField]
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

    public Weapons gunInven = null;
    public Weapons blueGunInven = null; // 파랑 마약 활성화 전용 인벤

    public Weapons pistolInven = null;

    public int[] magazines = { 0, 0 }; // 주무기, 보조무기 탄창

    public int[] bulletMagazine = { 30, 12, 10, 15 }; // 고정된 탄창
    public int[] magazineInven = { 0, 0, 0, 0 }; // Rifle, Shotgun, Sniper, Revolver순 / 변호나 후 남은 탄창
    public int[] curBullet = { 0, 0, 0, 0 }; // 현재 총에 있는 탄창

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

    // Effect
    public GameObject knifePivot;
    public GameObject knifeEffect;

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
        else Time.timeScale = DrugManager.Instance.timeValue;
    }

    public void UpdateDrug(int value)
    {
        // UI 매니저에서 수정
        drugGuage += value;
        drugBar.value = drugGuage;

        DrugManager.Instance.LockCheck(DrugGague);
    }

    public void UpdateWeapon(EWeapons value, Weapons weapon)
    {
        int idx = 0;

        if(value == EWeapons.Revolver)
        {
            if(pistolInven != null)
            {
                pistolInven.PutWeapon();
            }
            pistolInven = weapon;
            idx = 1;
        }
        else
        {
            if (blueGunInven == null && DrugManager.Instance.isManyWeapon) // 1회용 로직
            {
                blueGunInven = gunInven;
            }
            else if (gunInven != null)
            {
                gunInven.PutWeapon();
            }
            gunInven = weapon;
            idx = 0;
        }

        player.tempWeaponIndex = (int)weapon.index;
        player.WeaponSwap(idx);
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
    
    public void GetBullet(EWeapons value)
    {
        if(value == EWeapons.Revolver)
        {
            magazines[1]++;
            Debug.Log("보조무기 탄 얻음 : " + magazines[0]);
        }
        else
        {
            magazines[0]++;

            Debug.Log("주무기 탄 얻음 : " + magazines[0]); 
        }
    }

    public bool CheckReload(int a)
    {
        if (a == 0) return (magazines[a] <= 0 && magazineInven[(int)gunInven.eWeapons] <= 0);
        else return (magazines[a] <= 0 && magazineInven[3] <= 0);
    }

    public void RequestReloadBullet(EWeapons value, int count)
    {
        if (DrugManager.Instance.blue3) count *= 2;

        int gunIndex = (int)value;

        if (count > curBullet[gunIndex])
        {
            Debug.Log("총알 부족");
            int magazineType = gunIndex == 3 ? 1 : 0;

            if (magazines[magazineType] <= 0) // 탄창 없음
            {
                if(magazineInven[gunIndex] + curBullet[gunIndex] < count)
                {
                    Debug.Log("탄창 없음");
                    curBullet[gunIndex] += magazineInven[gunIndex];
                    magazineInven[gunIndex] = 0;
                }
                else ReLoadBullet(gunIndex, count);
            }
            else // 탄창 있음
            {
                if (magazineInven[gunIndex] + curBullet[gunIndex] < count)
                {
                    Debug.Log("탄창 있음");
                    magazines[magazineType]--;
                    magazineInven[gunIndex] += bulletMagazine[gunIndex];
                    ReLoadBullet(gunIndex, count);
                }
                else
                {
                    ReLoadBullet(gunIndex, count);
                }
            }

        }
    }

    public void ReLoadBullet(int gunIndex, int count)
    {
        magazineInven[gunIndex] -= count - curBullet[gunIndex];
        curBullet[gunIndex] = count;

        Debug.Log("장전 : " + magazineInven[gunIndex]);
    }

    public void ChangeGun(EWeapons value, int count)//총을 교체할 때 총알 다른부분을 수정
    {
        if (DrugManager.Instance.blue3) count *= 2;

        int gunIndex = (int)value;

        if (count < curBullet[gunIndex])
        {
            magazineInven[gunIndex] += curBullet[gunIndex] - count;
            curBullet[gunIndex] = count;
        }
    }

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

        UIManager.Instance.hpUpdate();
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
