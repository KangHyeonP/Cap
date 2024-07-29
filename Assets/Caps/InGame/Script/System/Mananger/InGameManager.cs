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

    // �ӽ� �÷��̾� ���
    public Player player;

    [SerializeField]
    private Slider drugBar;
    public float DrugGague => drugBar.value;

    [SerializeField]
    private bool isPause = false; //�ϴ� ����� �ΰ��� �����̹Ƿ� �Ͻ����� ����
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

    public int[] weaponDamage; // ������, ����, ����, ���� ��
    //public int weaponIndex = 0; // ���� �� ������ �������� ��� ���� ������

    private float aim;
    public float Aim => aim;

    private float attackDelay;
    public float AttackDelay => attackDelay;

    public int bulletPower;

    public Weapons gunInven = null;
    public Weapons blueGunInven = null; // �Ķ� ���� Ȱ��ȭ ���� �κ�

    public Weapons pistolInven = null;

    public int curWeaponIndex = 4; // ���� ���� �ε���
    public int lastWeaponIndex = 4; // ���� ���� �ε���
    public int lastPistolIndex = -1; // ���� ���� �ε���
    public int curPistolIndex = -1; // ���� ���� �ε���

    public int[] magazines = { 0, 0 }; // �ֹ���, �������� źâ

    public int[] bulletMagazine = { 30, 12, 10, 15 }; // ������ źâ
   // public int[] magazineInven = { 0, 0, 0, 0 }; // Rifle, Shotgun, Sniper, Revolver�� / ��ȣ�� �� ���� źâ
    public int[] curBullet = { 0, 0, 0, 0 }; // ���� �ѿ� �ִ� źâ

    private bool isDead;
    public bool IsDead => isDead;

    // Item
    public bool isItem;

    // �����ؾ���
    public Drug drugInven = null;

    public Item tempItem = null;

    // ���߿� Stack�������� �ٲ���ҵ�?
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
        // UI �Ŵ������� ����
        drugGuage += value;
        drugBar.value = drugGuage;

        DrugManager.Instance.LockCheck(DrugGague);
    }

    public void UpdateWeapon(EWeapons value, Weapons weapon)
    {
        int idx;
        lastWeaponIndex = curWeaponIndex;
        lastPistolIndex = curPistolIndex;

        if(value == EWeapons.Revolver)
        {
            idx = 1;
            player.tempWeaponIndex = idx;
            pistolInven = weapon;
            curWeaponIndex = 3;
            curPistolIndex = pistolInven.index;
        }
        else
        {
            Debug.Log("�ֹ��� ȹ�� ����");
            if(gunInven == null)
            {
                Debug.Log("ù ���� ȹ��"); // �ش� �ڵ尡 ���ٸ� ���Ⱑ ���� ���¿��� ��� ���� 3�ܰ� ���� �� ������ �߻�
            }
            else if (blueGunInven == null && DrugManager.Instance.isManyWeapon) // 1ȸ�� ����
            {
                PutBullet(gunInven.eWeapons);
                blueGunInven = gunInven;
                gunInven.gameObject.SetActive(false);
                blueGunInven.gameObject.SetActive(false);

                idx = 0;
                player.tempWeaponIndex = idx;
                gunInven = weapon;

                GetBullet(gunInven.eWeapons, weapon.bulletCount); // �ϴ� Ȥ�� ���� �߰��� �̵� ���� ����� �ٽ� �ּ�
                player.mainWeapon[gunInven.index].SetActive(true);
                UIManager.Instance.inGameUI.WeaponInven(gunInven.index);
                UIManager.Instance.inGameUI.BulletTextInput(gunInven.bulletCount, bulletMagazine[gunInven.index]);
                curWeaponIndex = gunInven.index;

                player.gunValue = 0;
                player.gunCheck = true;

                return;
            }
  
            idx = 0;
            player.tempWeaponIndex = idx;
            gunInven = weapon;
            curWeaponIndex = (int)value;
        }


        GetBullet(value, weapon.bulletCount);
        player.WeaponSwap(idx);
    }

    public void UpdateKey()
    {
        // �̰͵� UI
        //numKeyUI.text = "Key: " + numKey;
    }

    public void UpdateGrenade()
    {
        // �̰͵� UI
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
    
    public void GetMagazine(EWeapons value)
    {
        if(value == EWeapons.Revolver)
        {
            magazines[1]++;
            Debug.Log("�������� ź ���� : " + magazines[0]);
        }
        else
        {
            magazines[0]++;

            Debug.Log("�ֹ��� ź ���� : " + magazines[0]); 
        }
    }

    public void GetBullet(EWeapons value, int count) // UI ź�� ���� ���� ���� �����ؾ���
    {
        curBullet[(int)value] = count;
    }

    public void PutBullet(EWeapons value)
    {
        if (value == EWeapons.Revolver)
        {
            Debug.Log("���� �Ѿ� ���� : " + curBullet[3]);

            pistolInven.gameObject.SetActive(true);
            pistolInven.bulletCount = curBullet[3];
            Debug.Log("���� �Ѿ� ��� : " + curBullet[3]);
            curBullet[3] = 0;
        }
        else
        {
            gunInven.gameObject.SetActive(true);
            gunInven.bulletCount = curBullet[(int)gunInven.eWeapons];
            Debug.Log("�ֹ��� �Ѿ� ��� : " + curBullet[(int)gunInven.eWeapons]);
            curBullet[(int)gunInven.eWeapons] = 0;
        }
    }    


    public bool CheckReload(int a)
    {
        if (magazines[a] > 0)
        {
            Debug.Log("źâ�� ����");
            if(a==0) return bulletMagazine[(int)gunInven.eWeapons] > curBullet[(int)gunInven.eWeapons];
            else return bulletMagazine[(int)pistolInven.eWeapons] > curBullet[(int)pistolInven.eWeapons];
        }
        else return false;
    }

    public void ReloadBullet(int a)
    {
        if (a == 0)
        {
            GetBullet(gunInven.eWeapons, bulletMagazine[(int)gunInven.eWeapons]);
        }
        else GetBullet(EWeapons.Revolver, bulletMagazine[3]);

        magazines[a]--;
    }

    public void MaxHPUpdate()
    {
        // ���� �� ���� ��� ������ ���� �����ؾ���
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

    // ���� ������� �߰��ϱ�
    private void GameOver()
    {
        player.GetComponent<BoxCollider2D>().enabled = false;
    }
}
