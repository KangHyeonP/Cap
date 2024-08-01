using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item
{
    public int value = -1; // 0은 주무기 탄창, 1은 보조무기 탄창

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

        if (isProduct)
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
        UseMagazine();
        PoolManager.Instance.ReturnMagzine(this, value);
    }

    public void UseMagazine()
    {
        InGameManager.Instance.GetMagazine(value);
    }
    // 콜라이더 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //itemCol = collision.gameObject;
            InGameManager.Instance.tempItem = this;
            InGameManager.Instance.isItem = true;
            if (isProduct) ItemUIPlay(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //itemCol = null;
            InGameManager.Instance.tempItem = null;
            InGameManager.Instance.isItem = false;
            if (isProduct) ItemUIPlay(false);
        }
    }
}
