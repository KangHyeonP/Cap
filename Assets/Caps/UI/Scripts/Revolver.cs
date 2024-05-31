using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    public GameObject fireEffect;
	public Transform fireEffectPos;
    public Transform muzzle; 
    public GameObject bullet;
	public float fireSpeed = 10.0f;
    public float fireDelay = 3.0f;
    public int recoil = 0;
	public int bulletCount = 1;
	public int greenBulletCount = 0;

    private float fireTime = 0;
	

	// Start is called before the first frame update
	void Start()
    {
        fireTime = fireDelay;
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

	

    void ShotDelay()
    {
		if (fireTime >= fireDelay)
		{
            StartCoroutine(Shot());
		}
	}

    IEnumerator Shot()
    {
		fireEffect.transform.position = fireEffectPos.position;
		fireEffect.transform.rotation = fireEffectPos.rotation;
		fireEffect.SetActive(true);


		for(int i=0;i<bulletCount; i++)
		{
			muzzle.localRotation = Quaternion.Euler(0, 0, Random.Range(-90 - recoil, -90 + recoil));

			GameObject fireBullet = Instantiate(bullet, muzzle.position, transform.rotation);
			Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
			rb.AddForce(muzzle.up * (fireSpeed+Random.Range(1,-1)), ForceMode2D.Impulse);
		}

        fireTime = 0;

        yield return new WaitForSeconds(0.1f);

		if(DrugManager.Instance.lucianPassive)
		{
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < bulletCount; i++)
            {
                muzzle.localRotation = Quaternion.Euler(0, 0, Random.Range(-90 - recoil, -90 + recoil));
                //muzzle.position, transform.rotation 이걸 저장할 값이 필요함(루시안 패시브 떄문)
                GameObject fireBullet = Instantiate(bullet, muzzle.position, transform.rotation);
                Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
                rb.AddForce(muzzle.up * (fireSpeed + Random.Range(1, -1)), ForceMode2D.Impulse);
            }
        }

        fireEffect.SetActive(false);
	}
}
