using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColorBuff : MonoBehaviour
{
    protected void Awake()
    {
        
    }
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    public void ExcuteBuff(int i)
    {
        if (i == 0) FirstBuff();
        else if (i == 1) SecondBuff();
        else if (i == 2) ThirdBuff();

        Debug.Log(i+1 + " ���� ����");
    }

    public abstract void FirstBuff();

    public abstract void SecondBuff();

    public abstract void ThirdBuff();

}
