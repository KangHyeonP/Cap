using System.Collections;
using System.Collections.Generic;
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
        UseItem();
    }

    public override void UseItem()
    {
        UseKey();

        Destroy(this.gameObject);
    }

    public void UseKey()
    {
        // ���� ����
        //InGameManager.Instance.numKey++;
        Debug.Log("���� ����");
        InGameManager.Instance.UpdateKey();
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
