using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public abstract class Item : MonoBehaviour
{
    //선언해야할 변수들!!
    //붕대, 마약(랜덤 마약과 색깔이 보이는 마약들 ), 무기들(얘들은 고민 중), 스포이드, 수류탄

    protected Rigidbody2D itemRigid;

    protected virtual void Awake()
    {
        itemRigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }


    public virtual void GetItem()
    {
        gameObject.SetActive(false);
    }

    public abstract void UseItem();

}
