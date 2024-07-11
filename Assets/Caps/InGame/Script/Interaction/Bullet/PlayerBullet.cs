using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public int triggerCount = 0;
    protected Vector2 startPos;
    protected float distanceDamage;

    protected float rotateSpeed = 3.0f;
    //protected Vector2 moveDir;

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (DrugManager.Instance.isBulletChase) DetectAgent();
    } // 만약 유도탄일때는 맞추기 전까지 안사라진다고 하면, 로직 수정(유도탄에서 적 찾을 시 추격 값 무한으로 올리기)

    protected override void FixedUpdate()
    {
        // 유도탄 구현중 제대로 적용이 안됨
       /* if (target != null && DrugManager.Instance.isBulletChase)
        {
            // 목표의 방향 계산
            Vector2 direction = (Vector2)target.transform.position - rigid.position;
            direction.Normalize();

            // 회전 방향 계산
            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            // 유도탄 회전
            rigid.angularVelocity = -rotateAmount * rotateSpeed;

            // 유도탄 이동
            rigid.velocity = transform.up * 3.0f;
        }*/
        base.FixedUpdate();
    }

    private void DetectAgent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.localPosition, 7.0f);
        float nearTarget = 999;

        Debug.Log("실행은 하니1?");
        foreach (Collider2D c in colliders)
        {
                Debug.Log("실행은 하니2?");
            if (c.gameObject.CompareTag("Agent"))
            {
                Debug.Log("실행은 하니3?");
                float value = Vector2.Distance(c.gameObject.transform.position, transform.position);
                if (value < nearTarget)
                {
                    nearTarget = value;
                    target = c.gameObject;
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Agent")
        {
            triggerCount++;
            if (DrugManager.Instance.isBulletPass && triggerCount < 2) return;

            TrrigerLogic();
        }
    }
}
