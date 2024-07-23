using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Item
{
    public EWeapons eWeapons;
    public int index; //각 무기의 인덱스 값
    public bool checkMagazine = false; // 무기 첫 획득 판단
    public int bulletCount = 0; // 각 무기 탄창에 필요한 총알 개수

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
        InGameManager.Instance.isItem = false;

        if (eWeapons == EWeapons.Revolver)
        {
            if (InGameManager.Instance.pistolInven != null)
            {
                InGameManager.Instance.pistolInven.gameObject.SetActive(true);
                InGameManager.Instance.pistolInven.PutWeapon();
            }
        }
        else if(InGameManager.Instance.gunInven != null)
        {
            if(!DrugManager.Instance.isManyWeapon || InGameManager.Instance.blueGunInven != null)
            {
                InGameManager.Instance.gunInven.gameObject.SetActive(true);
                InGameManager.Instance.gunInven.PutWeapon();
            }
        }
        
        UseItem();

        base.GetItem();
    }

    public override void UseItem()
    {
        GetWepaon();
    }

    public void GetWepaon()
    {
        /*
        InGameManager.Instance.drugInven = this;
        UIManager.Instance.inGameUI.DrugInven(drugSprite.sprite);
        InGameManager.Instance.tempDrug = null;
        */

        InGameManager.Instance.UpdateWeapon(eWeapons, this);
        if(!checkMagazine)
        {
            InGameManager.Instance.GetBullet(eWeapons);
            InGameManager.Instance.RequestReloadBullet(eWeapons, bulletCount);
            checkMagazine = true;
        }
        InGameManager.Instance.ChangeGun(eWeapons, bulletCount);
    }
   
    public void PutWeapon()
    {
        transform.position = InGameManager.Instance.player.transform.position;

        itemRigid.AddForce(CameraController.Instance.MouseVecValue.normalized, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InGameManager.Instance.tempItem = this;
            InGameManager.Instance.isItem = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InGameManager.Instance.tempItem = null;
            InGameManager.Instance.isItem = false;
        }
    }
}
