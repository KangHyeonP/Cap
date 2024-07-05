using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Revolver : MonoBehaviour
{
    public GameObject fireEffect;
	public Transform fireEffectPos;
    public Transform muzzle;
	public float fireSpeed = 10.0f;
    public float fireDelay = 3.0f;
    public float recoil = 0;
	public int bulletCount = 1;
    private float curRecoil = 0;

    private float fireTime = 0;

    private float[] muzzleRecoil;
    private Vector3[] muzzleTransform;
    private Quaternion[] muzzleRotation;
    private Vector3[] muzzleUp;
    private float[] powerSpeed;

	// Start is called before the first frame update
	void Start()
    {
        fireTime = fireDelay;
        muzzleRecoil = new float[bulletCount];
        muzzleTransform = new Vector3[bulletCount];
        muzzleRotation = new Quaternion[bulletCount];
        muzzleUp = new Vector3[bulletCount];
        powerSpeed = new float[bulletCount];
    }

	// Update is called once per frame
	void Update()
    {
		fireTime += Time.deltaTime;

		if (Input.GetMouseButton(0))
        {
			ShotDelay();
		}
    }

<<<<<<< HEAD
    void ShotDelay()
    {
		if (fireTime >= fireDelay)
		{
			fireTime = 0;
			StartCoroutine(Shot());
		}
=======
	

    // �ϴ� �������̶� �����ϰ� �۾� ��
    void ShotDelay()
    {
        // �������� �ƽø� 0.2�ʴ� 1��
        if (fireTime >= 0.2f &&
            fireDelay <= fireTime + InGameManager.Instance.AttackDelay / 10.0f + DrugManager.Instance.playerAttackDelay / 16.0f)
        {
            StartCoroutine(Shot());
        }
>>>>>>> feature/TES-16_마약_모든_기능_구현
	}

    IEnumerator Shot()
    {
		fireEffect.transform.position = fireEffectPos.position;
		fireEffect.transform.rotation = fireEffectPos.rotation;
		fireEffect.SetActive(true);

<<<<<<< HEAD
=======
        if (recoil < InGameManager.Instance.Aim + DrugManager.Instance.aim)
            curRecoil = 0;
        else curRecoil = recoil - (InGameManager.Instance.Aim + DrugManager.Instance.aim);

>>>>>>> feature/TES-16_마약_모든_기능_구현
		for(int i=0;i<bulletCount; i++)
		{
            muzzleRecoil[i] = Random.Range(-90.0f - curRecoil, -90.0f + curRecoil);
            
            Bullet bullet = PoolManager.Instance.GetObject();
            bullet.transform.position = muzzle.position;
            muzzleTransform[i] = bullet.transform.position;
            muzzleUp[i] = muzzle.up;

<<<<<<< HEAD
			Bullet bullet = ObjectPool.Instance.GetObject();
			bullet.transform.position = muzzle.position;
			bullet.MoveBullet(muzzle.up * (fireSpeed + Random.Range(1, -1)));
			//bullet.MoveBullet(muzzle.up * (fireSpeed + Random.Range(1, -1)));
			//GameObject fireBullet = Instantiate(bullet, muzzle.position, transform.rotation);
			//Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
			//rb.AddForce(muzzle.up * (fireSpeed + Random.Range(1, -1)), ForceMode2D.Impulse);
=======
            // �Ѿ� ���� �ӵ��� �޾ƾ��� Random.Range(1, -1)) (��þȿ���)
            bullet.MoveBullet(muzzle.up * (fireSpeed + Random.Range(1, -1)));

            /*
            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleTransform[i] = muzzle.position;
            muzzleRotation[i] = transform.rotation;
            muzzleUp[i] = muzzle.up;
            powerSpeed[i] = fireSpeed + Random.Range(1.0f, -1.0f);
            */

            /*GameObject fireBullet = Instantiate(bullet, muzzleTransform[i], muzzleRotation[i]);
			Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
			rb.AddForce(muzzleUp[i] * powerSpeed[i], ForceMode2D.Impulse);*/
>>>>>>> feature/TES-16_마약_모든_기능_구현
		}

		Debug.Log(ObjectPool.Instance.poolingBulletQueue.Count);


		if (DrugManager.Instance.lucianPassive)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < bulletCount; i++)
            {
<<<<<<< HEAD
				//muzzle.localRotation = Quaternion.Euler(0, 0, Random.Range(-90 - recoil, -90 + recoil));
				//muzzle.position, transform.rotation �̰� ������ ���� �ʿ���(��þ� �нú� ����)
				var bullet = ObjectPool.Instance.GetObject();
				bullet.transform.position = muzzle.position;
				//bullet.MoveBullet(muzzle.up * (fireSpeed + Random.Range(1, -1)));
				//GameObject fireBullet = Instantiate(bullet, muzzle.position, transform.rotation);
				//Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
				//rb.AddForce(muzzle.up * (fireSpeed + Random.Range(1, -1)), ForceMode2D.Impulse);
			}
=======

                Bullet bullet = PoolManager.Instance.GetObject();
                bullet.transform.position = muzzleTransform[i];

                bullet.MoveBullet(muzzleUp[i] * (fireSpeed + Random.Range(1, -1)));

                /*
                muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);

                GameObject fireBullet = Instantiate(bullet, muzzleTransform[i], muzzleRotation[i]);
                Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
                rb.AddForce(muzzleUp[i] * powerSpeed[i], ForceMode2D.Impulse);
                */
            }
>>>>>>> feature/TES-16_마약_모든_기능_구현
        }

		yield return new WaitForSeconds(0.1f);

		fireEffect.SetActive(false);
	}
}
