using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item
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

        InGameManager.Instance.tempItem = null;
        InGameManager.Instance.isItem = false;

        UseItem();
    }

    public override void UseItem()
    {
        UseGrenade();

        PoolManager.Instance.ReturnActiveItem(this, itemValues);
    }

    public void UseGrenade()
    {
        // 로직 수정
        InGameManager.Instance.UpdateGrenade();
        //InGameManager.Instance.numKey++;
        //StartCoroutine(Explode());
        /*
        GameObject grenade = Instantiate(InGameManager.Instance.grenadeObj
            , InGameManager.Instance.player.transform.position, InGameManager.Instance.player.transform.rotation);

        InGameManager.Instance.UpdateGrenade();*/
    }


    // 콜라이더 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && InGameManager.Instance.tempItem == null)
        {
            //itemCol = collision.gameObject;
            InGameManager.Instance.tempItem = this;
            InGameManager.Instance.isItem = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && InGameManager.Instance.tempItem != null)
        {
            //itemCol = null;
            InGameManager.Instance.tempItem = null;
            InGameManager.Instance.isItem = false;
        }
    }
}
