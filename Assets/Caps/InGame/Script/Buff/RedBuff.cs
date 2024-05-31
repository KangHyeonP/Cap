using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBuff : ColorBuff
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
        DrugManager.Instance.red1 = true;
        DrugManager.Instance.RunRedBuff1();
    }

    public override void SecondBuff()
    {
        DrugManager.Instance.red2 = true;
        DrugManager.Instance.RunRedBuff2();
    }

    public override void ThirdBuff()
    {

    }

}
