using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Grenade : Item
{
    [SerializeField]
    private float delay = 3.0f;
    private float countDown;
    protected void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
    }

    public override void GetItem()
    {
        InGameManager.Instance.tempItem = null;
        InGameManager.Instance.isItem = false;
        InGameManager.Instance.grenadeCount++;
        InGameManager.Instance.grenades.Push(this);

        base.GetItem();
    }

    public override void UseItem()
    {
        UseGrenade();

        Destroy(this.gameObject);
    }

    public void UseGrenade()
    {
        // 로직 수정
        //InGameManager.Instance.numKey++;
        //StartCoroutine(Explode());

        GameObject grenade = Instantiate(InGameManager.Instance.grenadeObj
            , InGameManager.Instance.player.transform.position, InGameManager.Instance.player.transform.rotation);

        InGameManager.Instance.UpdateGrenade();
    }

    private IEnumerator Explode()
    {
        yield return null;
        GameObject grenade = Instantiate(InGameManager.Instance.grenadeObj
            , InGameManager.Instance.player.transform.position, InGameManager.Instance.player.transform.rotation);
        //yield return new WaitForSeconds(1.5f);
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
