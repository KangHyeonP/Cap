using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Item
{
    public EWeapons eWeapons;
    public int index; //�� ������ �ε��� ��
    public bool checkMagazine = false; // ���� ù ȹ�� �Ǵ�
    public int bulletCount = 0; // �� ������ źâ�� ź�� ����

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
                // �߰��� �κ�
                InGameManager.Instance.PutBullet(eWeapons);
                InGameManager.Instance.pistolInven.PutWeapon();
            }
        }
        else if(InGameManager.Instance.gunInven != null)
        {
            if(!DrugManager.Instance.isManyWeapon || InGameManager.Instance.blueGunInven != null)
            {
                InGameManager.Instance.gunInven.gameObject.SetActive(true);
                InGameManager.Instance.PutBullet(eWeapons);
                //�߰��Ѻκ�
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
        if (!checkMagazine)
        {
            checkMagazine = true;
            
            if(DrugManager.Instance.blue3) bulletCount *= 2;
        }

        InGameManager.Instance.UpdateWeapon(eWeapons, this);
        InGameManager.Instance.GetBullet(eWeapons, bulletCount);
       
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
