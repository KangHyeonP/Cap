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
    private GameObject agentBullet;

    [SerializeField]
    private Transform[] playerBulletPos;
    [SerializeField]
    private Transform agentBulletPos;


    [SerializeField]
    private int initCount;

    // ¾êµµ ´Ã·Á¾ßÇÔ
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

    private void CreateNewBullet(EUsers eUser, EBullets eBullet)
    {
        if(eUser == EUsers.Enemy)
        {
            Bullet newObj = Instantiate(agentBullet).GetComponent<Bullet>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(agentBulletPos);
            newObj.name = eBullet.ToString();
            poolingEnemyBullet.Enqueue(newObj);
        }
        else
        {
            Bullet newObj = Instantiate(playerBullet[(int)eBullet]).GetComponent<Bullet>();
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(playerBulletPos[(int)eBullet]);
            newObj.name = eBullet.ToString();
            poolingPlayerBullet[(int)eBullet].Enqueue(newObj);
        }
    }

    public Bullet GetBullet(EUsers eUser, EBullets eBullet, Quaternion q)
    {
        if(eUser == EUsers.Player)
        {
            //Debug.Log("poolingPlayerBullet : " + )

            if(poolingPlayerBullet[(int)eBullet].Count <= 0)
            {
                CreateNewBullet(eUser, eBullet);
            }
            Bullet obj = poolingPlayerBullet[(int)eBullet].Dequeue();
            obj.gameObject.transform.rotation = q;

            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            if (poolingEnemyBullet.Count <= 0)
            {
                CreateNewBullet(eUser, eBullet);
            }
        
            Bullet obj = poolingEnemyBullet.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

    }

    public void ReturnBullet(Bullet obj, EUsers eUser, EBullets eBullet)
    {
        obj.gameObject.SetActive(false);

        if (eUser == EUsers.Enemy)
        {
            poolingEnemyBullet.Enqueue(obj);
        }
        else
        {
            poolingPlayerBullet[(int)eBullet].Enqueue(obj);
        }
    }
}
