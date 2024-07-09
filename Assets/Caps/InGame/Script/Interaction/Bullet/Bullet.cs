using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EUsers
{
    Player, Enemy
}
public enum EBullets
{
    Rifle, Shotgun, Sniper, Revolver
}

// 분류하기
public abstract class Bullet : MonoBehaviour
{
    public float eraseSpeed;
    protected Rigidbody2D rb;
    [SerializeField]
    protected EUsers eUsers;
    [SerializeField]
    protected EBullets eBullets;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (DrugManager.Instance.isBulletSizeUp) gameObject.transform.localScale *= 1.5f;
    }
    protected void OnEnable()
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

    protected IEnumerator Erase()
    {
        yield return new WaitForSeconds(eraseSpeed + DrugManager.Instance.playerAttackRange);
        PoolManager.Instance.ReturnBullet(this, eUsers, eBullets);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Agent")
        {
            if (DrugManager.Instance.isBulletPass) return;
            
            PoolManager.Instance.ReturnBullet(this, eUsers, eBullets);
        }
        else if (collision.tag == "Wall") PoolManager.Instance.ReturnBullet(this, eUsers, eBullets);
    }
}
