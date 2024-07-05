using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
	public static ObjectPool Instance;

	[SerializeField]
	private GameObject poolingObj;
	[SerializeField]
	private int initCount;

	public Queue<Bullet> poolingBulletQueue = new Queue<Bullet>();
	public Queue<Drug> poolingDrugQueue = new Queue<Drug>();

	private void Awake()
	{
		Instance = this;
		Initialize(initCount);
	}

	private void Initialize(int initCount)
	{
		for (int i = 0; i < initCount; i++)
		{
			poolingBulletQueue.Enqueue(CreateNewObject());
		}
		Debug.Log(poolingBulletQueue.Count);
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
		if (Instance.poolingBulletQueue.Count > 0)
		{
			Bullet obj = Instance.poolingBulletQueue.Dequeue();
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
		Instance.poolingBulletQueue.Enqueue(obj);
	}
}
