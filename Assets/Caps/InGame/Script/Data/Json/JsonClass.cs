/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
*/

using System.IO;
using UnityEditor.Playables;
using UnityEngine;

//public enum 

public class JsonClass
{
    // JsonClasses
    private PlayerData _playerData;
    public PlayerData _PlayerData => _playerData;

    // DefaultPath
    private string defaultPath = Application.persistentDataPath + '/';
    private const string pDJsonName = "PlayerData.json";

    private string pdPath;

    private FileInfo fileInfo;
    // Default Data
    public JsonClass()
    {
        _playerData = new PlayerData();
        //FileCheck(playerData.jsonName)

        pdPath = defaultPath + pDJsonName;
    }

    // Data 실행 로직
    public void StartPlayerData()
    {
        fileInfo = new FileInfo(pdPath);

        if (!fileInfo.Exists)
        {
            File.WriteAllText(pdPath, JsonUtility.ToJson(_playerData));
            return;
        }

        string readJsonData = File.ReadAllText(pdPath);
        _playerData = JsonUtility.FromJson<PlayerData>(pDJsonName);


    }

    // PlayerData 저장
    public void SavePlayerData()
    {

        File.WriteAllText(pdPath, JsonUtility.ToJson(_playerData));
    }


    /*
    // 해당 경로에 파일 존재여부 체크
    private FileInfo FileCheck(string pathName)
    {
        return new FileInfo(defaultPath + pathName);
    }

    private void loadFile(FileInfo f)
    {
        if (!f.Exists) return; // 파일이 존재 안하면 return


    }*/

    private void DataSetting()
    {
        //string playerData = JsonUtility.ToJson(PlayerData);
    }

    private void PlayerDataCheck()
    {

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