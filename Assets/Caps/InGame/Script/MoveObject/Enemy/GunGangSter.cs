using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGangSter : Agent
{
    [SerializeField]
    private GameObject bullet;
    int hp = 5;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void AttackLogic()
    {

        Vector2 bulletDir = (target.position - muzzle.position).normalized;
        muzzle.up = bulletDir;
        muzzle.rotation = Quaternion.Euler(0, 0, muzzle.rotation.eulerAngles.z + Random.Range(-attackRecoil, attackRecoil + 1));

        GameObject bulletcopy = Instantiate(bullet, muzzle.position, muzzle.rotation);
        bulletcopy.GetComponent<Rigidbody2D>().velocity = muzzle.up * attackSpeed;
    }

    // 나중에 agent로 올리기
    private void OnTriggerEnter2D(Collider2D collision) //수정
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);

            hp--;
            Debug.Log("몬스터 남은 체력: " + hp);

            if (hp == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
