using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 추후 캐릭터 이름으로 이름 변경
public class MainPlayer : Player
{
    [SerializeField]
    private int SkillDamage = 300;

    private List<AI> agents = new List<AI>();

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

    protected override void PlayerSkill()
    {
        //Debug.Log("기본 플레이어 스킬");
        // 에이전트 초기화
        agents.Clear();

        Room playerRoom = RoomController.Instance.CurRoom();
        playerRoom.RoomAgent();

        int agentCnt = 0;
        Vector3 rangeVec = Vector3.zero;
        

        foreach(AI a in CameraController.Instance.Agents)
        {
            rangeVec = CameraController.Instance.Cam.WorldToViewportPoint(a.transform.position);
            if (rangeVec.x + (Mathf.Abs(a.transform.localScale.x) / (2 * 6.2f)) < 0 ||
                rangeVec.x - (Mathf.Abs(a.transform.localScale.x) / (2 * 6.2f)) > 1 || 
                rangeVec.y - (a.transform.localScale.y / (2 * 5.2f)) > 1 || 
                rangeVec.y + (a.transform.localScale.y / (2 * 5.2f)) < 0) continue;
            // 2는 반지름, 6은 camera x축 반지름 길이, 5는 y축 반지름 길이
            agents.Add(a);
            agentCnt++;

            //Debug.Log("x :  " + a.transform.localScale.x);
            //Debug.Log("y /  : " + a.transform.localScale.y / (2 * 5));
        }

        foreach(AI a in agents)
        {
            a.Damage(SkillDamage / agentCnt, WeaponValue.Knife);
        }
    }
}
