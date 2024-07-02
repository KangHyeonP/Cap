using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float eraseSpeed;
    public int hitCount = 0;

	private void Start()
	{
        if (DrugManager.Instance.isBulletSizeUp) gameObject.transform.localScale *= 1.5f;

        if (eraseSpeed > 0)
        {
            StartCoroutine(Erase());
        }
	}

	IEnumerator Erase()
    {
        yield return new WaitForSeconds(eraseSpeed + DrugManager.Instance.playerAttackRange);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Agent")
        {
            hitCount++;
            if (DrugManager.Instance.isBulletPass)
            {
                if (hitCount == 2) Destroy(gameObject);
            }
            else Destroy(gameObject);
        }
        else if (collision.tag == "Wall") Destroy(gameObject);
    }
}
