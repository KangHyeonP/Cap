using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScript : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.clearEnemyCount = InGameManager.Instance.killCount;
            GameManager.Instance.clearDrugCount = DrugManager.Instance.fullStackDrug + DrugManager.Instance.curStackDrug;
            GameManager.Instance.clearMoney = InGameManager.Instance.money;
            GameManager.Instance.clearCheck = true;

            SceneManager.LoadScene(1);
        }
    }
}
