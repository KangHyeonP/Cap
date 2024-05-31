using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SecondNerf : RandomNerf
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
        DrugManager.Instance.secondNerf1 = true;
        DrugManager.Instance.RunSecondNerf1();
    }

    public override void Nerf2()
    {
        DrugManager.Instance.secondNerf2 = true;
    }

    public override void Nerf3()
    {
        DrugManager.Instance.secondNerf3 = true;
    }

}
