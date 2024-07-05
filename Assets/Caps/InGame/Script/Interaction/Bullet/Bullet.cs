using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float eraseSpeed;
    private Rigidbody2D rb;

	private void Awake()
	{
        rb = GetComponent<Rigidbody2D>();
		if (DrugManager.Instance.isBulletSizeUp) gameObject.transform.localScale *= 2;
	}
	private void OnEnable()
	{
		if (eraseSpeed > 0)
		{
			StartCoroutine(Erase());
		}
	}

	public void MoveBullet(Vector2 dir)
    {
        rb.AddForce(dir, ForceMode2D.Impulse);
	}

    IEnumerator Erase()
    {
		yield return new WaitForSeconds(eraseSpeed);
        ObjectPool.Instance.ReturnObject(this);
	}
}
