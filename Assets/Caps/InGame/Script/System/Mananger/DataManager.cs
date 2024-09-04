using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 매니저로 JsonClass의 데이터를 넘겨줌
public class DataManager : MonoBehaviour
{
    private static DataManager instacne;
    public static DataManager Instacne => instacne;

    // JsonData
    private JsonClass jsonClass;
    public JsonClass JsonClass => jsonClass;

    // DefaultData
    private DefaultData defaultData;
    public DefaultData DefaultData => defaultData;

    // DateValue
    /// Cursor
    private int mouseIndex = -1;
    public int MouseIndex => mouseIndex;

    public Sprite[] pointerSprites;
    public Texture2D[] pointerTextures;
    Vector2 hotSpot;

    // Generacte Value
    public DataManager()
    {
        if(jsonClass == null) jsonClass = new JsonClass();
        if(defaultData == null) defaultData = new DefaultData();
    }

    private void Awake()
    {
        Init();

        jsonClass.StartPlayerData();

        for (int i = 0; i < jsonClass._PlayerData.mousePointer.Length; i++)
        {
            if (jsonClass._PlayerData.mousePointer[i] == true)
            {
                UpdatePointer(i);
                break;
            }
        }
    }

    private void Init()
    {
        if (Instacne == null)
        {
            instacne = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        GameManager.Instance.GetDiaryDate(jsonClass._DiaryData.checkDiary);
        GameManager.Instance.GetPlayerInformation(
            jsonClass._PlayerData.playTime, jsonClass._PlayerData.killEnemy,
            jsonClass._PlayerData.deathCount, jsonClass._PlayerData.clearCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 락 해제, 추후 로직 변동 필요할듯

    public void UpdateLock(int index)
    {
        if (jsonClass._PlayerData.playerLock[index]) return;

        jsonClass.UpdateLock(index);
    }

    public void UpdatePointer(int index)
    {
        mouseIndex = index;
        jsonClass.UpdatePointer(mouseIndex);

        // 만약 크기가 동일하면 awake에서 그냥 사용
        hotSpot.x = pointerTextures[mouseIndex].width / 2;
        hotSpot.y = pointerTextures[mouseIndex].height / 2;

        Cursor.SetCursor(pointerTextures[mouseIndex], hotSpot, CursorMode.Auto);
    }

}
