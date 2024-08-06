using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void GetItem()
    {
        if (DrugManager.Instance.itemBanCheck) return;

        UseItem();
    }

    public override void UseItem()
    {
        GetMoney();

        PoolManager.Instance.ReturnMoney(this);
    }

    public void GetMoney()
    {
        // 로직 수정
        InGameManager.Instance.UpdateMoney(1);
    }
    // 콜라이더 추가
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
