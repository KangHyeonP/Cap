using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGangSter : Agent
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override IEnumerator IAttack()
    {
        yield return null;

        Debug.Log("���� ���� ����");
        yield return new WaitForSeconds(2f);
        Debug.Log("���� ���� ����");

        yield return StartCoroutine(base.IAttack());
    }
}
