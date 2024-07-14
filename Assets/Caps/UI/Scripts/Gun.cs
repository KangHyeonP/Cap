using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum EWeapons
{
    Rifle, Shotgun, Sniper, Revolver
}

public abstract class Gun : MonoBehaviour
{
    [SerializeField]
    protected EWeapons wepons;
    [SerializeField]
    protected EUsers users;
    
    public Transform fireEffectPos;
    public Transform muzzle; 
	public float fireSpeed = 10.0f;
    public float fireDelay = 3.0f;
    public float fireMaximumDelay = 0.2f;
    public float recoil = 0;
	public int bulletCount = 1;
    [SerializeField]
    protected float curRecoil = 0;

    protected float fireTime = 0;

    protected float[] muzzleRecoil;
    protected Vector3[] muzzleTransform;
    protected Quaternion[] muzzleRotation;
    protected Vector3[] muzzleUp;
    protected float[] powerSpeed;
    protected float[] bulletSpeed;

    // Start is called before the first frame update
    protected void Start()
    {
        fireTime = fireDelay;
        muzzleRecoil = new float[bulletCount];
        muzzleTransform = new Vector3[bulletCount];
        muzzleRotation = new Quaternion[bulletCount];
        muzzleUp = new Vector3[bulletCount];
        powerSpeed = new float[bulletCount];
        bulletSpeed = new float[bulletCount];
    }

    // Update is called once per frame
    protected virtual void Update()
    {
		fireTime += Time.deltaTime;

		if (Input.GetMouseButton(0))
        {
			ShotDelay();
		}
    }



    // 일단 라이플이라 생각하고 작업 중
    protected abstract void ShotDelay();

    protected virtual IEnumerator Shot()
    {
		InGameManager.Instance.player.fireEffect.transform.position = fireEffectPos.position;
        InGameManager.Instance.player.fireEffect.transform.rotation = fireEffectPos.rotation;
        InGameManager.Instance.player.fireEffect.SetActive(true);

        if (recoil < InGameManager.Instance.Aim + DrugManager.Instance.aim)
            curRecoil = 0;
        else curRecoil = recoil - (InGameManager.Instance.Aim + DrugManager.Instance.aim);

		for(int i=0;i<bulletCount; i++)
		{
            muzzleRecoil[i] = Random.Range(-90.0f - curRecoil, -90.0f + curRecoil);

            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleRotation[i] = transform.rotation;

            Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)wepons, muzzleRotation[i]);
            bullet.transform.position = muzzle.position;
            muzzleTransform[i] = bullet.transform.position;
            muzzleUp[i] = muzzle.up;
            bulletSpeed[i] = Random.Range(1, -1);

            // 총알 각기 속도도 받아야함 Random.Range(1, -1)) (루시안에서)
            bullet.MoveBullet(muzzle.up * (fireSpeed + bulletSpeed[i]));

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
		}

        fireTime = 0;

        if (DrugManager.Instance.lucianPassive)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                muzzle.localRotation = muzzleRotation[i];

                Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)wepons, muzzleRotation[i]);
                bullet.transform.position = muzzleTransform[i];

                bullet.MoveBullet(muzzleUp[i] * (fireSpeed + bulletSpeed[i]));

                /*
                muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);

                GameObject fireBullet = Instantiate(bullet, muzzleTransform[i], muzzleRotation[i]);
                Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
                rb.AddForce(muzzleUp[i] * powerSpeed[i], ForceMode2D.Impulse);
                */
            }
        }

        yield return new WaitForSeconds(0.1f);
        InGameManager.Instance.player.fireEffect.SetActive(false);
	}
}
