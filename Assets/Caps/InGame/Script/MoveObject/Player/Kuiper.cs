using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kuiper : Player
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override IEnumerator ESkill()
    {
        yield return null;
        isSkill = false;
    }

    protected override void PlayerSkill()
    {
        //ebug.Log("기본 플레이어 스킬");
       
    }
}
