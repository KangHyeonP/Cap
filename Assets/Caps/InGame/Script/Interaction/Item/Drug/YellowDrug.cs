using UnityEngine;

public class YellowDrugDrug : Drug
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
        base.DrugAbility();
        DrugManager.Instance.playerAttackRange += 0.1f;
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
