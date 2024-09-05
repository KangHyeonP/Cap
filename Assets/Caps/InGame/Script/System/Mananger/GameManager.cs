using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

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

    [SerializeField]
    private bool[] diaryDataCheck = new bool[57];
    public bool[] DiaryDataCheck => diaryDataCheck;

    public DictionaryUI dictionaryUI = null;

    // PlayerInformation
    private int playTime = 0;
    public int PlayTime => playTime;

    private int killCount = 0;
    public int KillCount => killCount;

    private int diaryCount = 0;
    public int DiaryCount => diaryCount;

    private int characterCount = 0;
    public int CharacterCount => characterCount;

    private int deathCount = 0;
    public int DeathCount => deathCount;

    private int clearCount = 0;
    public int ClearCount => clearCount;


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
        else Destroy(this.gameObject);

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

    public void GetDiaryData(bool[] d)
    {
        for(int i=0; i<diaryDataCheck.Length; i++)
        {
            diaryDataCheck[i] = d[i];
        }
    }

    public void GetPlayerInformation(int time, int killCnt, int deathCnt, int clearCnt)
    {
        playTime = time;
        killCount = killCnt;
        /*foreach(bool d in diaryDataCheck)
        {
            if (d)
            {
                diaryCount++;
                // 디버그용
                Debug.Log()
            }
        }*/
        // Scene로드 시 GameManager 초기화
        diaryCount = 0;
        characterCount = 0;

        for(int i=0; i <diaryDataCheck.Length; i++)
        {
            if (diaryDataCheck[i])
            {
                diaryCount++;
                // 디버그용
                Debug.Log("다이어리 인덱스 + 1 : " + i);
            }
        }

        for (int i=21; i<24; i++)
        {
            if (diaryDataCheck[i]) characterCount++;
        }
        deathCount = deathCnt;
        clearCount = clearCnt;
    }

    public void UpdateDiaryDate(int index)
    {
        Debug.Log("들어온 값 : " + index);

        // GameManager에는 57개임 58개가아닌
        if (DiaryDataCheck[index-1]) return;

        diaryDataCheck[index-1] = true;
        dictionaryUI.UpdateContent(index);
    }

    public void UpdateKill()
    {
        killCount++;
    }

    // 죽을때 or 게임 종료할 때
    public void SaveData()
    {
        DataManager.Instacne.JsonClass.SavePlayerData();
    }
}
