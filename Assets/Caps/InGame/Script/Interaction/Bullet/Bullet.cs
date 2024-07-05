using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 분류하기
public class Bullet : MonoBehaviour
{
    public float eraseSpeed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (DrugManager.Instance.isBulletSizeUp) gameObject.transform.localScale *= 1.5f;
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
        yield return new WaitForSeconds(eraseSpeed + DrugManager.Instance.playerAttackRange);
        PoolManager.Instance.ReturnObject(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Agent")
        {
            if (DrugManager.Instance.isBulletPass) return;
            
            PoolManager.Instance.ReturnObject(this);
        }
        else if (collision.tag == "Wall") PoolManager.Instance.ReturnObject(this);
    }
}
