using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public enum RoomState // 현재 방의 상태
{
    being,
    notBeen,
    been
}

public class Room : MonoBehaviour
{
    // Fog
    [SerializeField]
    private SpriteRenderer fog;

    // Agents
    [SerializeField]
    private List<AI> agents;
    public List<AI> Agents => agents;
    private bool isPlayerRoom = false;

    // Room Index
    [SerializeField]
    private int roomIndex;
    public int RoomIndex => roomIndex;

    // Door
    [SerializeField]
    private GameObject[] door;

    // Room Status
    private bool clearCheck = false; // 현재 클리어한 방인지 체크
    private int curEnemyCnt = 0; // 처치한 적 수
    private int fullEnemyCnt = 0; // 현재 적 수

    private void Awake()
    {
        InitRoom();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void InitRoom()
    {
        fullEnemyCnt = agents.Count;
        RoomStateUpdate(RoomState.notBeen);

        foreach (GameObject g in door) g.SetActive(false);
    }

    public void RoomStateUpdate(RoomState state)
    {
        switch (state)
        {
            case RoomState.notBeen:
                fog.color = new Color(0, 0, 0, 1);
                break;
            case RoomState.being:
                fog.color = new Color(0, 0, 0, 0);
                break;
            case RoomState.been:
                fog.color = new Color(0, 0, 0, 0.8f);
                break;
        }
    }

    // 룸 활성화 로직
    public void ActiveRoom()
    {
        PlayerRoom(true);
        AgentActive(isPlayerRoom);
        //ActiveDoor();
    }

    // 현재 플레이어가 방에 들어온 상태
    private void PlayerRoom(bool check)
    {
        isPlayerRoom = check;
        RoomController.Instance.ChangePlayerRoom(roomIndex); 
    }
    
    // 현재 방에 있는 몬스터들을 활성화 시키는 로직, 추 후 몬스터의 자동 움직임으로 구현을 변경
    public void AgentActive(bool check)
    {
        if (!clearCheck && check && agents.Count != 0)
        {
            foreach (AI a in agents)
                a.PlayerRoom();
        }
    }

    public void RoomAgent()
    {
        CameraController.Instance.UpdateAgent(agents);
    }

    public void ClearCheckRoom()
    {
        curEnemyCnt++;

        if(curEnemyCnt >= fullEnemyCnt)
        {
            clearCheck = true;
            DisableDoor();
        }
    }

    // 이 부분은 이제 문이 나오면 해당 문에서 수정
    public void ActiveDoor()
    {
        if (!clearCheck && door.Length != 0)
        {
            foreach (GameObject g in door) g.SetActive(true);
        }
    }

    private void DisableDoor()
    {
        if (clearCheck && door.Length != 0)
        {
            foreach (GameObject g in door) g.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ActiveRoom();
        }
    }
}
