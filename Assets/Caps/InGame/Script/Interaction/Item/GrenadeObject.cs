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
        
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveVec = new Vector2(transform.localPosition.x, transform.localPosition.y);
        StartCoroutine(Explode());
        rigid.AddForce(CameraController.Instance.MouseVecValue.normalized * 5f, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        lastVec = rigid.velocity;
        Debug.Log(lastVec);
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1.5f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.localPosition, distance);

        foreach(Collider2D c in colliders)
        {
            c.GetComponent<Agent>()?.Damage(damage);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") // ���� �ε����� �ݴ�� ���鼭 ���� ������ ����
        {
            Debug.Log("���� ������");

            var speed = lastVec.magnitude * 0.5f;
            var dir = Vector2.Reflect(lastVec.normalized, collision.contacts[0].normal);

            rigid.velocity = dir * Mathf.Max(speed, 0f);
        }
    }
}