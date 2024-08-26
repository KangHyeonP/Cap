using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabUI : MonoBehaviour
{
	private Animator animator;

	// 공격력, 정확성, 사거리, 기동력, 연사력
	public TextMeshProUGUI attackDamage;
    public TextMeshProUGUI bulletAccuracy;
    public TextMeshProUGUI bulletDistance;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI attackSpeed;

    private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void Close()
	{
		animator.SetBool("Open", false);
	}

	public void TabOff()
	{
		UIManager.Instance.IsTab = false;
		gameObject.SetActive(false);
	}

	public void TabOn()
	{
        gameObject.SetActive(true);

		int power = InGameManager.Instance.Power;
		int buffPower = InGameManager.Instance.PlayerBuffPower();

        float aim = InGameManager.Instance.Aim + DrugManager.Instance.aim;
		float distance = InGameManager.Instance.BulletDistance + DrugManager.Instance.playerAttackRange;
		float playerSpeed = InGameManager.Instance.Speed + DrugManager.Instance.speed;
		float playerAttackSpeed = InGameManager.Instance.AttackDelay + DrugManager.Instance.playerAttackDelay;

        attackDamage.text = (power+buffPower) + "( " + power
            + " + " + buffPower + " )";
        bulletAccuracy.text = aim + "( " + InGameManager.Instance.Aim
			+ " + " + DrugManager.Instance.aim + " )";
        bulletDistance.text = distance + "( " + InGameManager.Instance.BulletDistance
            + " + " + DrugManager.Instance.playerAttackRange + " )";
        speed.text = playerSpeed + "( " + InGameManager.Instance.Speed
            + " + " + DrugManager.Instance.speed + " )";
        attackSpeed.text = playerAttackSpeed + "( " + InGameManager.Instance.AttackDelay
            + " + " + DrugManager.Instance.playerAttackDelay + " )";
    }
}
