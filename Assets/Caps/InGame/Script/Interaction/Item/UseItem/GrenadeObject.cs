using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeObject : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;

    public float distance = 1.5f;
    public int damage = 100;

    public Vector2 moveVec;
    private Vector2 lastVec;
        
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveVec = new Vector2(transform.localPosition.x, transform.localPosition.y);
        if (DrugManager.Instance.green3)
            rigid.AddForce(CameraController.Instance.MouseVecValue.normalized * 5f * 1.25f, ForceMode2D.Impulse);
        else
            rigid.AddForce(CameraController.Instance.MouseVecValue.normalized * 5f, ForceMode2D.Impulse);
        
        if(DrugManager.Instance.bombMissCheck)
        {
            int bombMissCheck = Random.Range(1, 11);
            Debug.Log("값 : " + bombMissCheck);
            if (bombMissCheck == 1)
            {
                Debug.Log("수류탄 불발");
                Destroy(gameObject);
                return;
            }
        }
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    private void Update()
    {
        lastVec = rigid.velocity;
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.5f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.localPosition, distance);

        foreach(Collider2D c in colliders)
        {
            c.GetComponent<AI>()?.Damage(damage, WeaponValue.Knife);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") // 벽에 부딪히면 반대로 가면서 힘을 반으로 감소
        {
            Debug.Log("벽에 닿은지");

            var speed = lastVec.magnitude * 0.5f;
            if (DrugManager.Instance.green3) speed *= 1.25f;
            var dir = Vector2.Reflect(lastVec.normalized, collision.contacts[0].normal);

            rigid.velocity = dir * Mathf.Max(speed, 0f);
        }
    }
}
