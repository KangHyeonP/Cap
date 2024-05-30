using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float eraseSpeed;

	private void Start()
	{
        if (DrugManager.Instance.isBulletSizeUp) gameObject.transform.localScale *= 2;

        if (eraseSpeed > 0)
        {
            StartCoroutine(Erase());
        }
	}

	IEnumerator Erase()
    {
        yield return new WaitForSeconds(eraseSpeed);
        Destroy(gameObject);
    }
}
