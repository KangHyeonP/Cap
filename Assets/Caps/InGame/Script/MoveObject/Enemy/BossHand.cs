using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BossHand : MonoBehaviour
{
	[SerializeField]
	private Boss boss;
	private Transform bossTransform;

	Vector2 playerVec2;
	float angle;

	void Start()
	{
        bossTransform = boss.transform;
	}

	void Update()
	{
		LookMouse();
	}

	void LookMouse()
	{
		if (boss.IsAttack) return;
		
		playerVec2 = (Vector2)InGameManager.Instance.player.transform.localPosition - (Vector2)bossTransform.localPosition;
		angle = Mathf.Atan2(playerVec2.y, playerVec2.x) * Mathf.Rad2Deg;

		if (bossTransform.localScale.x == -1)
		{
			angle *= -1;
			this.transform.rotation = Quaternion.AngleAxis(180 - angle, Vector3.forward);
		}
		else this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
