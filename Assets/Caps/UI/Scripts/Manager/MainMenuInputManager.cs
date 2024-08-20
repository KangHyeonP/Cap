using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInputManager : MonoBehaviour
{
    public GameObject Dictionary;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dictionary.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            Dictionary.SetActive(false);
        }
    }
}
