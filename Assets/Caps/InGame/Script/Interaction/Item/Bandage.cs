using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandage : Item
{
    

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
        if (InGameManager.Instance.MaxHp == InGameManager.Instance.Hp)
            return;
        UseItem();
    }

    public override void UseItem()
    {
        UseBandage();
        
        Destroy(this.gameObject);
    }

    public void UseBandage()
    {
        InGameManager.Instance.HealHp(1);


        if(DrugManager.Instance.red2)
        {
            DrugManager.Instance.RunRedBuff2();
        }
        // 로직 수정
        if (DrugManager.Instance.bandageNerf)
        {
            int infect = Random.Range(1, 11);
            if (infect == 1)
                Debug.Log("붕대 사용 못함");
            return;
        }


        //if (InGameManager.Instance.health < InGameManager.Instance.maxHealth)
        //   InGameManager.Instance.health++; 
    }
    // 콜라이더 추가
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
