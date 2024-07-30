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

    protected override void Awake()
    {
        base.Awake();
        drugSprite = GetComponent<SpriteRenderer>();
        curDrugSprite = drugSprite.sprite;
    }

    protected virtual void OnEnable()
    {
        if (DrugManager.Instance == null) return;
        else if (DrugManager.Instance.colorBlindCheck) drugSprite.sprite = blackDrugSprite;
        else drugSprite.sprite = curDrugSprite;
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
        InGameManager.Instance.tempItem = null;
        InGameManager.Instance.isItem = false; // 이부분 추가

        if (InGameManager.Instance.drugInven != null)
        {
            InGameManager.Instance.drugInven.gameObject.SetActive(true);
            InGameManager.Instance.drugInven.PutDrug();     
        }

        InGameManager.Instance.drugInven = this;
        UIManager.Instance.inGameUI.DrugInven(drugSprite.sprite);
        InGameManager.Instance.tempDrug = null;

        base.GetItem();
    }

    public override void UseItem()
    {
        GetDrug();
        InGameManager.Instance.UpdateDrug(drugGuage);
        DrugAbility();
        Destroy(this.gameObject);
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
        drugSprite.sprite = curDrugSprite;
        transform.position = InGameManager.Instance.player.transform.position;

        itemRigid.AddForce(CameraController.Instance.MouseVecValue.normalized, ForceMode2D.Impulse);
    }
    protected virtual void DrugAbility()
    {

    }

    // 콜라이더 추가

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && InGameManager.Instance.tempItem == null)
        {
            InGameManager.Instance.tempItem = this;
            InGameManager.Instance.tempDrug = this;
            InGameManager.Instance.isItem = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && InGameManager.Instance.tempItem != null)
        {
            InGameManager.Instance.tempItem = null;
            InGameManager.Instance.tempDrug = null;
            InGameManager.Instance.isItem = false;
        }
    }

}
