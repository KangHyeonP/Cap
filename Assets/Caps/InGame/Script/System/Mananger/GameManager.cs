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

    [SerializeField]
    private bool[] diaryDataCheck = new bool[57];
    public bool[] DiaryDataCheck => diaryDataCheck;

    public DictionaryUI dictionaryUI = null;

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

    public void GetDiaryDate(bool[] d)
    {
        for(int i=0; i<diaryDataCheck.Length; i++)
        {
            diaryDataCheck[i] = d[i];
        }
    }

    public void UpdateDiaryDate(int index)
    {
        Debug.Log("���� �� : " + index);

        // GameManager���� 57���� 58�����ƴ�
        if (DiaryDataCheck[index-1]) return;

        diaryDataCheck[index-1] = true;
        dictionaryUI.UpdateContent(index);
    }

    // ������ or ���� ������ ��
    public void SaveData()
    {
        DataManager.Instacne.JsonClass.SavePlayerData();
    }

    
}
