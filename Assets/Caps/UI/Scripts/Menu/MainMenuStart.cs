using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuStart : MonoBehaviour
{
    
    void Start()
    {
        SoundManager.Instance.PlayBGM(BGM.Main);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}