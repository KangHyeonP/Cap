using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : BasicWeapon
{
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

    protected override void AttackDelay()
    {
        base.AttackDelay();
    }

    protected override IEnumerator Attack()
    {
        SoundManager.Instance.PlaySFX(SFX.Knife_Shot);
        return base.Attack();
    }

    public override void CancleAttack()
    {
        base.CancleAttack();
    }
}
