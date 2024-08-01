using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    /*
    [SerializeField]
    GameObject[] items; //6��
    */
    [SerializeField]
    GameObject[] guns; //3��
    [SerializeField]
    GameObject[] glocks; //2��
    [SerializeField]
    /*
    GameObject[] drugs; //5��
    [SerializeField]
    */
    Transform[] itemPos; //5��
    [SerializeField]
    Transform[] drugPos; //5��

    int curIndex = 0;

    void Start()
    {
        ItemSpawn();

        /*for (int i = 0; i < drugs.Length; i++)
        {
            Instantiate(drugs[i], drugPos[i].position, Quaternion.identity, transform);
        }*/

        for (int i = 0; i < drugPos.Length; i++)
        {
            PoolManager.Instance.GetDrug((EDrugColor)i).ShopItem(drugPos[i].position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ItemSpawn()
    {
        int isGunSpawn = Random.Range(0, 6);
        Debug.Log("�����õ�: " + isGunSpawn);

        /*if(isGunSpawn == 3)
        {
            Instantiate(glocks[Random.Range(0, glocks.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
        }
        else if (isGunSpawn == 4)
        {
            Instantiate(guns[Random.Range(0, guns.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
        }
        else if (isGunSpawn == 5)
        {
            Instantiate(glocks[Random.Range(0, glocks.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
            Instantiate(guns[Random.Range(0, guns.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
        }*/
        /*for (int i = curIndex; i < itemPos.Length; i++)
        {
            int itemRand = Random.Range(0, items.Length);
            Instantiate(items[itemRand], itemPos[curIndex++].position, Quaternion.identity, transform);
        }*/

        if (isGunSpawn == 3)
        {
            Instantiate(glocks[Random.Range(0, glocks.Length)]).GetComponent<Item>().ShopItem(itemPos[curIndex++].position);
        }
        else if (isGunSpawn == 4)
        {
            Instantiate(guns[Random.Range(0, glocks.Length)]).GetComponent<Item>().ShopItem(itemPos[curIndex++].position);
        }
        else if (isGunSpawn == 5)
        {
            Instantiate(glocks[Random.Range(0, glocks.Length)]).GetComponent<Item>().ShopItem(itemPos[curIndex++].position);
            Instantiate(guns[Random.Range(0, glocks.Length)]).GetComponent<Item>().ShopItem(itemPos[curIndex++].position);
        }

        for (int i=curIndex; i<itemPos.Length;i++)
        {
            int itemRand = Random.Range(0, 6);

            Debug.Log("�� : "+curIndex);
            if (itemRand < 2)
            {
                PoolManager.Instance.GetMagzine(itemRand).ShopItem(itemPos[curIndex++].position);
            }
            else
            {
                PoolManager.Instance.GetActiveItem((EActiveItems)(itemRand - 2)).ShopItem(itemPos[curIndex++].position);
            }
        }
    }

}
