using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;


    [SerializeField]
    private GameObject[] playerBullet;

    [SerializeField]
    private Transform[] bulletPos;


    [SerializeField]
    private int initCount;

    // 얘도 늘려야함
    public Queue<Bullet>[] poolingPlayerBullet = { new Queue<Bullet>(), new Queue<Bullet>(), new Queue<Bullet>(), new Queue<Bullet>() };
    public Queue<Bullet> poolingEnemyBullet = new Queue<Bullet>();

    private void Awake()
    {
        Instance = this;


        Initialize(initCount);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            CreateNewBullet(EUsers.Player, EBullets.Rifle);
            CreateNewBullet(EUsers.Player, EBullets.Shotgun);
            CreateNewBullet(EUsers.Player, EBullets.Sniper);
            CreateNewBullet(EUsers.Player, EBullets.Revolver);
            CreateNewBullet(EUsers.Enemy, EBullets.Revolver);
        }
    }

    // PlayerBullet, EnemyBullet = 둘다 Bullet 상속, 단 Bullet은 사용안함(생성에서 문제가 생겨서)
    /*private Bullet CreateNewBullet(EUsers eUser, EBullets eBullet)
    {
        Bullet newObj = Instantiate(poolingObj).GetComponent<Bullet>();
        Debug.Log("디버그 생성 총알 : "+newObj.name);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }*/

    private void CreateNewBullet(EUsers eUser, EBullets eBullet)
    {
        Bullet newObj = Instantiate(playerBullet[(int)eBullet]).GetComponent<Bullet>();
        Debug.Log("디버그 생성 총알 : " + newObj.name);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(bulletPos[(int)eBullet]);
        newObj.name = eBullet.ToString();

        if(eUser == EUsers.Enemy)
        {
            poolingEnemyBullet.Enqueue(newObj);
        }
        else
        {
            poolingPlayerBullet[(int)eBullet].Enqueue(newObj);
        }
    }

    public Bullet GetBullet(EUsers eUser, EBullets eBullet)
    {
        if(eUser == EUsers.Player)
        {
            if(poolingPlayerBullet[(int)eUser].Count < 0)
            {
                CreateNewBullet(eUser, eBullet);
            }
            Bullet obj = poolingPlayerBullet[(int)eBullet].Dequeue();

            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            if (poolingEnemyBullet.Count < 0)
            {
                CreateNewBullet(eUser, eBullet);
            }
        
            Bullet obj = poolingEnemyBullet.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        /*if (Instance.poolingPlayerBullet.Count > 0)
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
        }*/
    }

    public void ReturnBullet(Bullet obj, EUsers eUser, EBullets eBullet)
    {
        Debug.Log(obj);
        obj.gameObject.SetActive(false);

        if (eUser == EUsers.Enemy)
        {
            poolingEnemyBullet.Enqueue(obj);
        }
        else
        {
            poolingPlayerBullet[(int)eBullet].Enqueue(obj);
        }

        /*
        Debug.Log(obj);
        obj.gameObject.SetActive(false);
        Instance.poolingPlayerBullet.Enqueue(obj);*/
    }
}
