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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall") // 벽에 부딪히면 반대로 가면서 힘을 반으로 감소
        {
            Vector2 newVec = -rigid.velocity.normalized;
            float power = rigid.velocity.magnitude * 0.5f;
            rigid.velocity = Vector2.zero;
            rigid.AddForce(newVec * power, ForceMode2D.Impulse); 
        }
    }
}
