using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstNerf : RandomNerf
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

    public override void Nerf1()
    {
        DrugManager.Instance.firstNerf1 = true;
    }

    public override void Nerf2()
    {
        DrugManager.Instance.firstNerf2 = true;
    }

    public override void Nerf3()
    {      
        DrugManager.Instance.firstNerf3 = true;
    }
}
