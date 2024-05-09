using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	private static UIManager instance;
	public static UIManager Instance => instance;

	public GameObject PauseUI;
	public GameObject TabUI;
	public GameObject DictUI;
	public GameObject ExitUI;
	public InGameUI inGameUI;

	[HideInInspector]
	public bool IsPause = false;
	[HideInInspector]
	public bool IsTab = false;
	[HideInInspector]
	public bool IsDict = false;
	[HideInInspector]
	public bool IsExit = false;

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
		InputKey();

        OpenPause();
		OpenTab();
		OpenDict();
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
		if (!isInventoryKey) return;

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
		if (!isTabKey) return;

		if (!IsTab)
		{
			TabUI.SetActive(true);
		}
		else
		{
			TabUI.GetComponent<TabUI>().Close();
		}
	}

	private void OpenPause() //일시정지
	{
		if (!isPauseKey) return;

		if(!IsPause)
		{
			PauseTime(true);
			PauseUI.SetActive(true);
		}
		else
		{
			if(IsExit)
			{
				ExitUI.SetActive(false);
			}
			else
			{
				PauseTime(false);
				PauseUI.GetComponent<PauseUI>().Close();
			}
		}
	}

	private void PauseTime(bool IsPause)
	{
		if(IsPause)
		{
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
