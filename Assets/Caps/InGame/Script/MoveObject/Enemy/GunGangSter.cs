using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGangSter : Agent
{
    private GameObject bullet;

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
        // 로직 수정하기
        GameObject bulletcopy = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector2 bulletDir = (target.position - transform.position).normalized;
        bulletcopy.GetComponent<Rigidbody2D>().velocity = bulletDir * attackSpeed;
    }
}
