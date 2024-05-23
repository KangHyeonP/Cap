using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    // Player Status
    private int playerHp = 0;
    public int PlayerHp => playerHp;


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
}
