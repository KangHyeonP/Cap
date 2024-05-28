using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBuff : ColorBuff
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
        DrugManager.Instance.green1 = true;
    }

    public override void SecondBuff()
    {
        DrugManager.Instance.green2 = true;
    }

    public override void ThirdBuff()
    {
        
    }
}
