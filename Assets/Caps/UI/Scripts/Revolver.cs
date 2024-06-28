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

	

    void ShotDelay()
    {
        // ¶óÀÌÇÃÀº ¸Æ½Ã¸Ø 0.2ÃÊ´ç 1¹ß
        if (fireTime >= 0.2f &&
            fireDelay <= fireTime + InGameManager.Instance.AttackDelay + DrugManager.Instance.playerAttackDelay)
            StartCoroutine(Shot());
	}

    IEnumerator Shot()
    {
		fireEffect.transform.position = fireEffectPos.position;
		fireEffect.transform.rotation = fireEffectPos.rotation;
		fireEffect.SetActive(true);

        if (recoil < InGameManager.Instance.Aim + DrugManager.Instance.aim)
            curRecoil = 0;
        else curRecoil = recoil - (InGameManager.Instance.Aim + DrugManager.Instance.aim);

		for(int i=0;i<bulletCount; i++)
		{
            muzzleRecoil[i] = Random.Range(-90.0f - curRecoil, -90.0f + curRecoil);
            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleTransform[i] = muzzle.position;
            muzzleRotation[i] = transform.rotation;
            muzzleUp[i] = muzzle.up;
            powerSpeed[i] = fireSpeed + Random.Range(1.0f, -1.0f);

            GameObject fireBullet = Instantiate(bullet, muzzleTransform[i], muzzleRotation[i]);
			Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
			rb.AddForce(muzzleUp[i] * powerSpeed[i], ForceMode2D.Impulse);
		}

        fireTime = 0;

        if (DrugManager.Instance.lucianPassive)
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < bulletCount; i++)
            {
                muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);

                GameObject fireBullet = Instantiate(bullet, muzzleTransform[i], muzzleRotation[i]);
                Rigidbody2D rb = fireBullet.GetComponent<Rigidbody2D>();
                rb.AddForce(muzzleUp[i] * powerSpeed[i], ForceMode2D.Impulse);
            }
        }

        fireEffect.SetActive(false);
	}
}
