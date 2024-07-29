using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyGun : Gun
{
    // Enemy는 Update를 쓰지 않음
    // 추 후 Gun의 하위 PlayerGun으로 분리해야 할 수 도 있음
    protected override void OnEnable()
    {
       
    }

    protected override void Update()
    {
        
    }

    public void ShotReady()
    {
        ShotDelay();
    }

    public void ShotReady(Vector2 pos, float angle)
    {
        StartCoroutine(Shot(pos, angle));
    }

    public void ShotReady(Vector3 dir, Vector2 pos,int angle) // 범위 각도
    {
        Shot(dir, angle);
    }

    public void Shot(Vector3 dir, Vector2 pos, int angle)
    {
        Bullet bullet = PoolManager.Instance.GetBullet(EUsers.Enemy, EBullets.Revolver, Quaternion.Euler(0, 0, angle % 360));
        bullet.transform.position = pos;
        bullet.MoveBullet(dir * fireSpeed);
    }

    protected override void ShotDelay()
    {
        StartCoroutine(Shot());
    }

    protected override IEnumerator Shot()
    {

        for (int i = 0; i < bulletCount; i++)
        {
            muzzleRecoil[i] = Random.Range(-90.0f - curRecoil, -90.0f + curRecoil);

            muzzle.localRotation = Quaternion.Euler(0, 0, muzzleRecoil[i]);
            muzzleRotation[i] = transform.rotation;

            Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)weapons, muzzleRotation[i]);
            bullet.transform.position = muzzle.position;
            muzzleTransform[i] = bullet.transform.position;
            muzzleUp[i] = muzzle.up;
            bulletSpeed[i] = Random.Range(1, -1);

            bullet.MoveBullet(muzzle.up * (fireSpeed + bulletSpeed[i]));
        }

        fireTime = 0;

        yield return new WaitForSeconds(0.1f);
        //InGameManager.Instance.player.fireEffect.SetActive(false);
    }

    protected virtual IEnumerator Shot(Vector2 pos, float angle)
    {
        yield return null;

        muzzle.localRotation = Quaternion.Euler(0, 0, angle);

        Bullet bullet = PoolManager.Instance.GetBullet(users, (EBullets)weapons, muzzle.localRotation);
        bullet.transform.position = pos;
        bullet.MoveBullet(muzzle.up * fireSpeed);
    }
}
