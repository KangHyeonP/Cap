using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    bool triggerCheck = false;

    protected override IEnumerator Erase()
    {
        yield return new WaitForSeconds(eraseSpeed);
        TrrigerLogic();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Player" && !InGameManager.Instance.player.AvoidCheck 
            && !InGameManager.Instance.player.IsHit)
        {
            TrrigerLogic();
        }
    }
}
