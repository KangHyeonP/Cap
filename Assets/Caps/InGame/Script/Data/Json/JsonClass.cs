/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
*/

using System.IO;
using UnityEngine;

//public enum 

public class JsonClass
{
    // JsonClasses
    private PlayerData _playerData;
    public PlayerData _PlayerData => _playerData;

    // DefaultPath
    private string defaultPath;
    private const string pDJsonName = "PlayerData.json";

    private string pdPath;

    private FileInfo fileInfo;

    // Init Data Settings
    public JsonClass()
    {
        _playerData = new PlayerData();
    }

    // Check Data
    public void StartPlayerData()
    {
        defaultPath = Application.persistentDataPath + '/';
        pdPath = defaultPath + pDJsonName;
        fileInfo = new FileInfo(pdPath);

        //Debug.Log(Application.persistentDataPath);
        //Debug.Log(pdPath);

        if (!fileInfo.Exists) return;

        string readJsonData = File.ReadAllText(pdPath);
        _playerData = JsonUtility.FromJson<PlayerData>(readJsonData);


    }

    // Save PlayerData 저장(죽을때, 게임을 종료할 때 해당 조건을 게임매니저에서 실행)
    public void SavePlayerData()
    {
        File.WriteAllText(pdPath, JsonUtility.ToJson(_playerData));
    }

    public void UpdateLock(int index)
    {
        _playerData.playerLock[index] = true;
    }
}

public class PlayerData
{
    public bool[] playerLock = { true, false, false, false, false }; // true : 해제, false : 잠금
    public float playTime = 0;
    public float killEnemy = 0;
}

public class DiaryData
{

}