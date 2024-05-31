using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeunUI : MonoBehaviour
{
	public void GameStart()
	{
		if (GameManager.Instance.playerCheck)
			SceneManager.LoadScene(1);
	}
}
