using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField]
    private GameObject poolingObj;
    [SerializeField]
    private int initCount;

    public Queue<Bullet> poolingObjectQueue = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;
        Initialize(initCount);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
        Debug.Log(poolingObjectQueue.Count);
    }

    private Bullet CreateNewObject()
    {
        Bullet newObj = Instantiate(poolingObj).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public Bullet GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            Bullet obj = Instance.poolingObjectQueue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Bullet newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public void ReturnObject(Bullet obj)
    {
        Debug.Log(obj);
        obj.gameObject.SetActive(false);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
