using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum EActiveItems
{
    Band, Key, Bulletproof, Grenade, Money,
    None
}
// ���� �߿�
// �ش�, ����, ��ź, ����ź, �� ������� ������ ���x

public abstract class Item : MonoBehaviour
{
    //�����ؾ��� ������!!
    //�ش�, ����(���� ����� ������ ���̴� ����� ), �����(����� ��� ��), �����̵�, ����ź
    [SerializeField]
    protected EActiveItems itemValues;
    public int price = 0;
    public bool isProduct = false;

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
