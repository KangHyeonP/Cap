using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drug : Item
{
    public int drugGuage;
    private SpriteRenderer drugSprite;
    [SerializeField]
    protected EDrugColor value;
    public Sprite blackDrugSprite;
    public Sprite curDrugSprite;

    public bool mapDrug = false; // 이미 생선된 마약인지 체크, 맵에 있는 마약은
    // 이미 생성된 상태이므로 색맹마약과 별개로 사용할려고

    protected override void Awake()
    {
        base.Awake();
        drugSprite = GetComponent<SpriteRenderer>();
        curDrugSprite = drugSprite.sprite;
    }

    protected virtual void OnEnable()
    {
        if (DrugManager.Instance == null) return;
        else if (DrugManager.Instance.colorBlindCheck && !mapDrug) drugSprite.sprite = blackDrugSprite;
        else drugSprite.sprite = curDrugSprite;

        mapDrug = true; // 생성 이후 맵 마약으로 등록
        // 색맹은 나중에 몬스터 아이템 드랍 이후로 확인해봐야함
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
        if (isProduct)
        {
            if (DrugManager.Instance.hostHateCheck) curPrice = (price * 6) / 5; // 가격 20프로향상, 1.2배 증가
            else curPrice = price;
            if (InGameManager.Instance.money < curPrice) return;

            InGameManager.Instance.Buy(curPrice);

            isProduct = false;
            ItemUIPlay(false);
        }

        if (InGameManager.Instance.drugInven != null)
        {
            //InGameManager.Instance.drugInven.gameObject.SetActive(true);
            InGameManager.Instance.drugInven.PutDrug();     
        }

        InGameManager.Instance.drugInven = this;
        UIManager.Instance.inGameUI.DrugInven(drugSprite.sprite);
        //InGameManager.Instance.tempDrug = null;

        base.GetItem();
    }

    public override void UseItem()
    {
        GetDrug();
        InGameManager.Instance.UpdateDrug(drugGuage);
        DrugAbility();
        mapDrug = false;

        PoolManager.Instance.ReturnDrug(this, value);
    }

    public void GetDrug()
    {
        DrugManager.Instance.tempStackDrug[(int)value]++;

        if(DrugManager.Instance.gaugeUp)
        {
            drugGuage = Random.Range(9, 13);
            return;
        }
        drugGuage = Random.Range(6, 10);
    }

    public void PutDrug()
    {
        mapDrug = true;
        gameObject.SetActive(true);
        drugSprite.sprite = curDrugSprite;
        transform.position = InGameManager.Instance.player.transform.position;

        itemRigid.AddForce(CameraController.Instance.MouseVecValue.normalized, ForceMode2D.Impulse);
    }
    protected virtual void DrugAbility()
    {

    }

    // 콜라이더 추가

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isProduct) ItemUIPlay(true);

            InGameManager.Instance.tempItems.Add(this);
            InGameManager.Instance.tempDrug.Add(this);
            playerCheck = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isProduct) ItemUIPlay(false);

            InGameManager.Instance.tempItems.Remove(this);
            InGameManager.Instance.tempDrug.Remove(this);
            distance = 999f;
            playerCheck = false;
        }
    }

}
