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
    } // ���� ����ź�϶��� ���߱� ������ �Ȼ�����ٰ� �ϸ�, ���� ����(����ź���� �� ã�� �� �߰� �� �������� �ø���)

    protected override void FixedUpdate()
    {
        // ����ź ������ ����� ������ �ȵ�
       /* if (target != null && DrugManager.Instance.isBulletChase)
        {
            // ��ǥ�� ���� ���
            Vector2 direction = (Vector2)target.transform.position - rigid.position;
            direction.Normalize();

            // ȸ�� ���� ���
            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            // ����ź ȸ��
            rigid.angularVelocity = -rotateAmount * rotateSpeed;

            // ����ź �̵�
            rigid.velocity = transform.up * 3.0f;
        }*/
        base.FixedUpdate();
    }

    private void DetectAgent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.localPosition, 7.0f);
        float nearTarget = 999;

        Debug.Log("������ �ϴ�1?");
        foreach (Collider2D c in colliders)
        {
                Debug.Log("������ �ϴ�2?");
            if (c.gameObject.CompareTag("Agent"))
            {
                Debug.Log("������ �ϴ�3?");
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
