using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdNerf : RandomNerf
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
        DrugManager.Instance.thirdNerf1 = true;
    }

    public override void Nerf2()
    {
        DrugManager.Instance.thirdNerf2 = true;
    }

    public override void Nerf3()
    {
        DrugManager.Instance.thirdNerf3 = true;
    }
}
