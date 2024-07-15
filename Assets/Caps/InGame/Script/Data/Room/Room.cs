using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public enum RoomStatus // ���� ���� ����
{
    
}

public class Room : MonoBehaviour
{
    // Agents
    [SerializeField]
    private List<Agent> agents;
    public List<Agent> Agents => agents;
    private bool isPlayerRoom = false;

    // Room Index
    [SerializeField]
    private int roomIndex;
    public int RoomIndex => roomIndex;

    // Door
    [SerializeField]
    private GameObject[] door;

    // Room Status
    private bool clearCheck = false; // ���� Ŭ������ ������ üũ
    private int curEnemyCnt = 0; // óġ�� �� ��
    private int fullEnemyCnt = 0; // ���� �� ��

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

        foreach (GameObject g in door) g.SetActive(false);
    }

    // �� Ȱ��ȭ ����
    public void ActiveRoom()
    {
        PlayerRoom(true);
        AgentActive(isPlayerRoom);
        ActiveDoor();
    }

    // ���� �÷��̾ �濡 ���� ����
    private void PlayerRoom(bool check)
    {
        isPlayerRoom = check;
        RoomController.Instance.ChangePlayerRoom(roomIndex); 
    }
    
    // ���� �濡 �ִ� ���͵��� Ȱ��ȭ ��Ű�� ����, �� �� ������ �ڵ� ���������� ������ ����
    public void AgentActive(bool check)
    {
        if (!clearCheck && check && agents.Count != 0)
        {
            foreach (Agent a in agents)
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
