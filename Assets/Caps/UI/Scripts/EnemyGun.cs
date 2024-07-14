using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    // Enemy�� Update�� ���� ����
    // �� �� Gun�� ���� PlayerGun���� �и��ؾ� �� �� �� ����
    protected override void Update()
    {
        
    }

    public void ShotReady()
    {
        ShotDelay();
    }

    protected override void ShotDelay()
    {
        StartCoroutine(Shot());
    }

    protected override IEnumerator Shot()
    {
        /* �� �κ��� ������ �ذ��ؾ��ҵ���
        InGameManager.Instance.player.fireEffect.transform.position = fireEffectPos.position;
        InGameManager.Instance.player.fireEffect.transform.rotation = fireEffectPos.rotation;
        InGameManager.Instance.player.fireEffect.SetActive(true);
        */

        for (int i = 0; i < bulletCount; i++)
        {
            muzzleRecoil[i] = Random.Range(-90.0f - curRecoil, -90.0f + curRecoil);

            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleRotation[i] = transform.rotation;

            Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)wepons, muzzleRotation[i]);
            bullet.transform.position = muzzle.position;
            muzzleTransform[i] = bullet.transform.position;
            muzzleUp[i] = muzzle.up;
            bulletSpeed[i] = Random.Range(1, -1);

            // �Ѿ� ���� �ӵ��� �޾ƾ��� Random.Range(1, -1)) (��þȿ���)
            bullet.MoveBullet(muzzle.up * (fireSpeed + bulletSpeed[i]));
        }

        fireTime = 0;

        yield return new WaitForSeconds(0.1f);
        //InGameManager.Instance.player.fireEffect.SetActive(false);
    }
}
