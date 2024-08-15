using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    // Player Status
    [SerializeField]
    private int playerHp = 0;
    public int PlayerHp => playerHp;

    [SerializeField]
    private int playerAttackPower;
    public int PlayerAttackPower => playerAttackPower;

    [SerializeField]
    private float playerBulletDistance;
    public float PlayerBulletDistance => playerBulletDistance;

    [SerializeField]
    private float playerAimAccuracy;
    public float PlayerAimAccuracy => playerAimAccuracy;

    [SerializeField]
    private float playerSpeed;
    public float PlayerSpeed => playerSpeed;

    [SerializeField]
    private float playerAttackDelay;
    public float PlayerAttackDelay => playerAttackDelay;

    public ECharacters selectCharacter;

    public bool playerCheck = false;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    private void Init()
    {
        if(Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void GetPlayerValue(int hp, int power, float aim, float distance, float speed, float attackDelay)
    {
        playerHp = hp;
        playerAttackPower = power;
        playerAimAccuracy = aim;
        playerBulletDistance = distance;
        playerSpeed = speed;
        playerAttackDelay = attackDelay;
    }


    // 죽을때 or 게임 종료할 때
    public void SaveData()
    {
        DataManager.Instacne.JsonClass.SavePlayerData();
    }

    
}
