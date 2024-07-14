using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : Gun
{
    // Enemy는 Update를 쓰지 않음
    // 추 후 Gun의 하위 PlayerGun으로 분리해야 할 수 도 있음
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
        /* 이 부분은 적에서 해결해야할듯함
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

            // 총알 각기 속도도 받아야함 Random.Range(1, -1)) (루시안에서)
            bullet.MoveBullet(muzzle.up * (fireSpeed + bulletSpeed[i]));
        }

        fireTime = 0;

        yield return new WaitForSeconds(0.1f);
        //InGameManager.Instance.player.fireEffect.SetActive(false);
    }
}
