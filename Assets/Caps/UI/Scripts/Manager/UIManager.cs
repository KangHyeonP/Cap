using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	private static UIManager instance;
	public static UIManager Instance => instance;

	public PauseUI pauseUI;
	public TabUI TabUI;
	public GameObject DictUI;
	public GameObject ExitUI;
	public InGameUI inGameUI;
    public GameObject TempUI;

    [HideInInspector]
    public int IsPopup = 0;
    [HideInInspector]
	public bool IsTab = false;
	[HideInInspector]
	public bool IsDict = false;

	//InputKey
	private bool isEscKey;
	private bool isPauseKey;
	private bool isTabKey;
	private bool isInventoryKey;

    private void Awake()
	{
		Init();
	}

	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		//InputKey();

        //OpenPause();
		//OpenTab();
		//OpenDict();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
			Debug.Log(IsPopup);
            OpenPause();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !IsTab)
        {
            OpenTab();
            Debug.Log("Open");
        }

        if (Input.GetKeyUp(KeyCode.Tab) && IsTab)
        {
            OpenTab();
            Debug.Log("Close");
        }

        if (Input.GetKeyDown(KeyCode.I) && IsPopup == 0)
        {
            OpenDict();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("IsTab: " + IsTab);
            Debug.Log("IsDict: " + IsDict);
        }

    }

    private void Init()
	{
		if (Instance == null)
		{
			instance = this;
		}
	}

	//public void InitPlayer

	private void InputKey()
	{
		isEscKey = Input.GetKeyDown(KeyCode.Escape);
		isPauseKey = Input.GetKeyDown(KeyCode.P);
		isTabKey = Input.GetKeyDown(KeyCode.Tab);
		isInventoryKey = Input.GetKeyDown(KeyCode.I);
    }

	private void OpenDict() //도감
	{
		//if (!isInventoryKey) return;

		if (!IsDict)
		{
			DictUI.SetActive(true);
			PauseTime(true);
		}
		else
		{
			PauseTime(false);
			DictUI.GetComponent<DictionaryUI>().Close();
		}
	}

	private void OpenTab() //탭
	{
		//if (!isTabKey) return;

		if (!IsTab)
		{
			IsTab = true;
			TabUI.gameObject.SetActive(true);
		}
		else
		{
			TabUI.Close();
		}
	}


    private void OpenPause() //일시정지
	{
		
        switch (IsPopup)
        {
            case 0:
                PauseTime(true);
                pauseUI.gameObject.SetActive(true);
                break;
            case 1:
                PauseTime(false);
                pauseUI.gameObject.GetComponent<PauseUI>().Close();
                IsPopup--;
                break;
            case 2:
                TempUI.SetActive(false);
                break;
        }

    }

    private void PauseTime(bool IsPause)
	{
		if(IsPause)
		{
            if (IsTab)
            {
                TabUI.Close();
            }
            InGameManager.Instance.Pause(true);
			Time.timeScale = 0f;
		}
		else
		{
            InGameManager.Instance.Pause(false);
            Time.timeScale = 1f;
		}
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
}
