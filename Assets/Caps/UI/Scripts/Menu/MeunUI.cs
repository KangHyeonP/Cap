using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeunUI : MonoBehaviour
{
	public DictionaryUI dictUi;

    private void Start()
    {
		GameManager.Instance.dictionaryUI = dictUi;
    }

    public void GameStart()
	{
		if (GameManager.Instance.playerCheck)
		{
			GameManager.Instance.UpdateDiaryDate((int)GameManager.Instance.selectCharacter + 14); // �⺻ �� ���̾ ���
			SceneManager.LoadScene(1);
		}
	}
}
