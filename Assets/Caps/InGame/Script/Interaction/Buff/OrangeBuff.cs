using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeBuff : ColorBuff
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

    public override void FirstBuff()
    {
        DrugManager.Instance.orange1= true;
        DrugManager.Instance.RunOrangeBuff1();
    }

    public override void SecondBuff()
    {

    }

    public override void ThirdBuff()
    {
        
    }
}