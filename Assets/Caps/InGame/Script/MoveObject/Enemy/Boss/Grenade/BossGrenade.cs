using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrenade : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;

    public float distance = 1.5f;

    //public Vector2 moveVec;
    private Vector2 lastVec;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        //transform.position = InGameManager.Instance.player.transform.position;
        //transform.rotation = InGameManager.Instance.player.transform.rotation;

        //rigid.AddForce(CameraController.Instance.MouseVecValue.normalized * 5f, ForceMode2D.Impulse);

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // X와 Y 성분 계산 (단위 벡터)
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        Vector2 moveVec = new Vector2(x, y);
        

        rigid.AddForce(moveVec.normalized * 7.5f, ForceMode2D.Impulse);
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

        foreach (Collider2D c in colliders)
        {
            c.GetComponent<Player>()?.Hit(1);
        }

        PoolManager.Instance.ReturnBossGrenade(this);
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
        else if(collision.gameObject.tag == "Player")
        {
            rigid.velocity *= 0.5f;
        }
    }
}
