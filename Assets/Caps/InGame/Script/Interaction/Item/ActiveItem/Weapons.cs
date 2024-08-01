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
        if (isProduct)
        {
            if (InGameManager.Instance.money < curPrice) return;

            InGameManager.Instance.Buy(curPrice);
            isProduct = false;
            ItemUIPlay(false);
        } // ���� �Ǹ����� üũ

        InGameManager.Instance.isItem = false;

        if (eWeapons == EWeapons.Revolver) // ���� ȹ�湫�Ⱑ ����������
        {
            if (InGameManager.Instance.pistolInven != null) // ���� �������⸦ ���� ������ Ȯ��
            {
                // ���� ����
                InGameManager.Instance.PutBullet(eWeapons); // ���� �÷��̾��� ���ѿ� ź�� ����
                InGameManager.Instance.pistolInven.PutWeapon(); // ���� ���� ������ ������ ��
            }
        }
        else if(InGameManager.Instance.gunInven != null) // ���� ȹ�湫�Ⱑ �ֹ�����
        {
            // ���� ����
            InGameManager.Instance.PutBullet(eWeapons); //  ���� �÷��̾��� �ֹ��⿡ ź�� ����

            // ���� ������ Ȱ��ȭ �� �������� Ȯ��
            if (DrugManager.Instance.isManyWeapon)
            {
                if(InGameManager.Instance.blueGunInven == null) // ���� ���� �κ��� ����ִٸ�
                {
                    InGameManager.Instance.blueGunInven = InGameManager.Instance.gunInven; // ���� �� ���⸦ ���� �κ����� ����
                    InGameManager.Instance.blueGunInven.gameObject.SetActive(false); // ��ü�� �ֹ��� �����
                }
                else
                {
                    InGameManager.Instance.gunInven.PutWeapon(); // ���� ���� ���� ��ȯ
                }

            }
            else //���� ������ Ȱ��ȭ �� ���°� �ƴ϶��
            {
                InGameManager.Instance.gunInven.PutWeapon(); // ���� �ֹ��⸦ ������ ��
            }
        }
        
        UseItem();
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
            Debug.Log("���� �Ѿ� ī��Ʈ : " + bulletCount);
        }

        InGameManager.Instance.UpdateWeapon(eWeapons, this);
        gameObject.SetActive(false);
    }
   
    public void PutWeapon()
    {
        gameObject.SetActive(true);

        transform.position = InGameManager.Instance.player.transform.position;

        itemRigid.AddForce(CameraController.Instance.MouseVecValue.normalized, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InGameManager.Instance.tempItem = this;
            InGameManager.Instance.isItem = true;
            if (isProduct) ItemUIPlay(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            InGameManager.Instance.tempItem = null;
            InGameManager.Instance.isItem = false;
            if (isProduct) ItemUIPlay(false);
        }
    }
}
