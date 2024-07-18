using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    protected override void ShotDelay()
    {
        if (fireTime >= fireMaximumDelay &&
            fireDelay <= fireTime + InGameManager.Instance.AttackDelay / 5.0f + DrugManager.Instance.playerAttackDelay)
        {
            StartCoroutine(Shot());
        }
    }
}
