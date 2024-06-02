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

    protected void Awake()
    {
        base.Awake();
        drugSprite = GetComponent<SpriteRenderer>();
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
        if(InGameManager.Instance.drugInven != null)
        {
            InGameManager.Instance.drugInven.gameObject.SetActive(true);
            InGameManager.Instance.drugInven.PutDrug();
            
        }

        InGameManager.Instance.drugInven = this;
        InGameManager.Instance.UpdateDrugType(drugSprite.sprite); //이거 UI연동하려고 추가한거임 (지)
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
        transform.position = InGameManager.Instance.player.transform.position;
        Vector3 nextVec = new Vector3(0, 3, transform.position.z); //GameManager.Instance.player.inputVec * 30;
        transform.position += nextVec;
        //Vector2.Distance(nextVec, sumVec));

    }
    protected virtual void DrugAbility()
    {

    }

    // 콜라이더 추가

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InGameManager.Instance.tempItem = this;
            InGameManager.Instance.tempDrug = this;
            InGameManager.Instance.isItem = true;
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //itemCol = null;
            InGameManager.Instance.tempItem = null;
            InGameManager.Instance.tempDrug = null;
            InGameManager.Instance.isItem = false;
        }
    }

}
