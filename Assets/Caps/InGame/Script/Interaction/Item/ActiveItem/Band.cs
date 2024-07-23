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
        UseItem();
    }

    public override void UseItem()
    {
        UseBand();
        
        Destroy(this.gameObject);
    }

    public void UseBand()
    {
        // ���� ����
        if (DrugManager.Instance.bandageNerf)
        {
            int infect = Random.Range(1, 11);
            if (infect == 1)
                Debug.Log("�ش� ��� ����");
            return;
        }

        InGameManager.Instance.HealHp(1);
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
