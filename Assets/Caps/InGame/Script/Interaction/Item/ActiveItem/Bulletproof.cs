using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletproof : Item
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
        if (UIManager.Instance.isBulletProof) return;

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
        if (UIManager.Instance.isBulletProof) return;

        UseBulletproof();
        PoolManager.Instance.ReturnActiveItem(this, itemValues);
    }

    public void UseBulletproof()
    {
        int index = (InGameManager.Instance.Hp - 1) / 2;
        UIManager.Instance.bulletProof.rectTransform.anchoredPosition
            = new Vector2(index * 57, -5);


            //= UIManager.Instance.heartImages[index].rectTransform.position;

        UIManager.Instance.BulletproofUpdate(true);
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