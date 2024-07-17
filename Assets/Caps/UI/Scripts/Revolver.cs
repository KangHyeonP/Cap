using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{
    protected override void ShotDelay()
    {
        if (fireTime >= fireMaximumDelay &&
            fireDelay <= fireTime + InGameManager.Instance.AttackDelay / 10.0f + DrugManager.Instance.playerAttackDelay / 8.0f)
        {
            fireTime = 0;
            base.StartCoroutine(Shot());
        }
    }
}
