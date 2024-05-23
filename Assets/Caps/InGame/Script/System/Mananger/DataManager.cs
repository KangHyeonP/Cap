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

    // Generacte Value
    public DataManager()
    {
        jsonClass = new JsonClass();
        defaultData = new DefaultData();
    }

    private void Awake()
    {
        Init();

        jsonClass.StartPlayerData();
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
