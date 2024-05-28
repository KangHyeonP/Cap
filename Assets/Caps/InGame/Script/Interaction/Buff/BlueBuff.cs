using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBuff : ColorBuff
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
        DrugManager.Instance.blue1 = true;
    }

    public override void SecondBuff()
    {
        
    }
    
    public override void ThirdBuff()
    {
        DrugManager.Instance.blue3 = true;
    }
}
