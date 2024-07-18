using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField]
    private List<Room> rooms;
    public List<Room> Rooms => rooms;


    private static RoomController instance;
    public static RoomController Instance => instance;

    [SerializeField]
    private int curIndex = 0; // ���� �� �ε���
    public int CurIndex => curIndex;


    private void Awake()
    {
        Init();
    }

    void Start()
    {
        rooms[0].ActiveRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Init()
    {
        instance = this;
    }

    // �̴ϸ� ����
    public void ChangePlayerRoom(int num)
    {
        curIndex = num;
    }

    public void ClearRoomCount() // ���� ó���� �� ���� ���� Ŭ���� �������� Ȯ��
    {
        rooms[curIndex].ClearCheckRoom();
    }

    public Room CurRoom()
    {
        return Rooms[CurIndex];
    }
 

    public void BombLogic(Vector3 pos) // getcomponent�� �� ���� ������ ���� �������� ���༭ ����� ����
    {
        foreach (Agent a in rooms[curIndex].Agents)
        {
            if (Vector3.Distance(pos, a.gameObject.transform.localPosition) < 4f)
            {
                a.Damage(InGameManager.Instance.Power + DrugManager.Instance.power, WeaponValue.Knife);
                Debug.Log("����ź : " + a.gameObject.name);
            }
        }
    }
    /*
    private void CheckEnemy(int num)
    {
        rooms[num]?.AgentActive(false);

     }
    */


}
