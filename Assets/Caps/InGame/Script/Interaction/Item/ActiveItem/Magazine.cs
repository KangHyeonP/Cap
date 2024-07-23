using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : Item
{
    public int value = -1; // 0~2�� �ֹ��� źâ, 3�� �������� źâ

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
        UseItem();
    }

    public override void UseItem()
    {
        UseMagazine();

        Destroy(this.gameObject);
    }

    public void UseMagazine()
    {
        InGameManager.Instance.GetMagazine((EWeapons)value);
    }
    // �ݶ��̴� �߰�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //itemCol = collision.gameObject;
            InGameManager.Instance.tempItem = this;
            InGameManager.Instance.isItem = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //itemCol = null;
            InGameManager.Instance.tempItem = null;
            InGameManager.Instance.isItem = false;
        }
    }
}
