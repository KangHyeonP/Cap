using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBuff : ColorBuff
{
    
    // Start is called before the first frame update
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
        DrugManager.Instance.yellow1 = true;

    }

    public override void SecondBuff()
    {

    }

    public override void ThirdBuff()
    {

    }
}
