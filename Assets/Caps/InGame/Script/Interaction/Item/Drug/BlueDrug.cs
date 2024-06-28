using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueDrug : Drug
{
    protected void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        base.Start();

    }

    protected void Update()
    {
        base.Update();

    }

    protected override void DrugAbility()
    {
        DrugManager.Instance.playerAttackDelay += 1;
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
