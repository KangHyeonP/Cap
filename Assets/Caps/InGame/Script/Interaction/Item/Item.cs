using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public abstract class Item : MonoBehaviour
{
    //�����ؾ��� ������!!
    //�ش�, ����(���� ����� ������ ���̴� ����� ), �����(����� ���� ��), �����̵�, ����ź

    protected Rigidbody2D itemRigid;

    protected void Awake()
    {
        itemRigid = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {

    }

    protected void Update()
    {

    }


    public virtual void GetItem()
    {
        gameObject.SetActive(false);
    }

    public abstract void UseItem();

}