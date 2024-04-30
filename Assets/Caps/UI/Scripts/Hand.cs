using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Hand : MonoBehaviour
{
	float angle;
	Vector2 target, mouse;

	void Start()
    {
		target = transform.position;
	}

    void Update()
    {
		LookMouse();
	}

	void LookMouse()
	{
		mouse = CameraController.Instance.Pointer;
		angle = CameraController.Instance.PlayerAngle;
		if (GameManager.Instance.player.IsReverse)
		{
			angle *= -1;
            this.transform.rotation = Quaternion.AngleAxis(180 - angle, Vector3.forward);
        }
		else this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
