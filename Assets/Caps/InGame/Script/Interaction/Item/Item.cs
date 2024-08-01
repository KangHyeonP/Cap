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
// 순서 중요
// 붕대, 열쇠, 방탄, 수류탄, 돈 만사용함 나머진 사용x

public abstract class Item : MonoBehaviour
{
    //선언해야할 변수들!!
    //붕대, 마약(랜덤 마약과 색깔이 보이는 마약들 ), 무기들(얘들은 고민 중), 스포이드, 수류탄
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

        if (DrugManager.Instance.hostHateCheck) curPrice = (price * 6) / 5; // 가격 20프로향상, 1.2배 증가
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
