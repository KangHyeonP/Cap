using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Band : Item
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

        if(isProduct)
        {
            if (InGameManager.Instance.money < curPrice) return;

            InGameManager.Instance.Buy(curPrice);
            ItemUIPlay(false);
            isProduct = false;
        }

        InGameManager.Instance.tempItem = null;
        InGameManager.Instance.isItem = false;
        UseItem();
    }

    public override void UseItem()
    {
        UseBand();
        PoolManager.Instance.ReturnActiveItem(this, itemValues);
    }

    public void UseBand()
    {
        // 로직 수정
        if (DrugManager.Instance.bandNerf)
        {
            int infect = Random.Range(1, 11);
            Debug.Log("값 : " + infect);
            if (infect == 1)
                Debug.Log("붕대 사용 못함");
            return;
        }

        InGameManager.Instance.HealHp(1);
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
