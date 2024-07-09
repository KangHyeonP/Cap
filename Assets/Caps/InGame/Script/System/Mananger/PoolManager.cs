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

    // 얘도 늘려야함
    public Queue<Bullet> poolingPlayerBullet = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;
        Initialize(initCount);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingPlayerBullet.Enqueue(CreateNewBullet());
        }
        Debug.Log(poolingPlayerBullet.Count);
    }

    // PlayerBullet, EnemyBullet = 둘다 Bullet 상속, 단 Bullet은 사용안함(생성에서 문제가 생겨서)
    private Bullet CreateNewBullet()
    {
        Bullet newObj = Instantiate(poolingObj).GetComponent<Bullet>();
        Debug.Log("디버그 생성 총알 : "+newObj.name);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public Bullet GetBullet(EUsers eUser, EBullets eBullet)
    {
        if (Instance.poolingPlayerBullet.Count > 0)
        {
            Bullet obj = Instance.poolingPlayerBullet.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            Bullet newObj = Instance.CreateNewBullet();
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public void ReturnBullet(Bullet obj, EUsers eUser, EBullets eBullet)
    {
        Debug.Log(obj);
        obj.gameObject.SetActive(false);
        Instance.poolingPlayerBullet.Enqueue(obj);
    }
}
