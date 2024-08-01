using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum EActiveItems
{
    Band, Key, Bulletproof, Grenade,
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
    public int curPrice = 0;

    public TextMeshPro priceText;
    public SpriteRenderer eIconRenderer;
    public SpriteRenderer moneyRenderer;

    protected Rigidbody2D itemRigid;

    protected virtual void Awake()
    {
        itemRigid = GetComponent<Rigidbody2D>();
        curPrice = price;
    }

    protected virtual void Start()
    {
        ItemUIPlay(false);
    }

    protected virtual void Update()
    {

    }


    public virtual void GetItem()
    {
        gameObject.SetActive(false);
    }

    public abstract void UseItem();

    public void ShopItem(Vector2 pos)
    {
        isProduct = true;

        transform.position = pos;

        if (DrugManager.Instance.hostHateCheck) curPrice = (price * 6) / 5; // ���� 20�������, 1.2�� ����
        else curPrice = price;

        priceText.text = curPrice.ToString();
    }

    public void ItemUIPlay(bool check)
    {
        priceText.enabled = check;
        eIconRenderer.enabled = check;
        moneyRenderer.enabled = check;
    }
}
