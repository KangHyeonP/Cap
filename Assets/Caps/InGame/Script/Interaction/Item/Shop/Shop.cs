using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    GameObject[] items; //6��
    [SerializeField]
    GameObject[] guns; //3��
    [SerializeField]
    GameObject[] glocks; //2��
    [SerializeField]
    GameObject[] drugs; //5��
    [SerializeField]
    Transform[] itemPos; //5��
    [SerializeField]
    Transform[] drugPos; //5��

    int curIndex = 0;

    void Start()
    {
        ItemSpawn();

        for (int i = 0; i < drugs.Length; i++)
        {
            Instantiate(drugs[i], drugPos[i].position, Quaternion.identity, transform);
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
        if (isGunSpawn == 3)
        {
            Instantiate(glocks[Random.Range(0, glocks.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
        }
        if (isGunSpawn == 4)
        {
            Instantiate(guns[Random.Range(0, guns.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
        }
        if (isGunSpawn == 5)
        {
            Instantiate(glocks[Random.Range(0, glocks.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
            Instantiate(guns[Random.Range(0, guns.Length)], itemPos[curIndex++].position, Quaternion.identity, transform);
        }
        for (int i = curIndex; i < itemPos.Length; i++)
        {
            int itemRand = Random.Range(0, items.Length);
            Instantiate(items[itemRand], itemPos[curIndex++].position, Quaternion.identity, transform);
        }
    }

}