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
    private int curIndex = 0; // 현재 룸 인덱스
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

    // 미니맵 수정
    public void ChangePlayerRoom(int num)
    {
        curIndex = num;
    }

    public void ClearRoomCount() // 적을 처지할 때 마다 방이 클리어 상태인지 확인
    {
        rooms[curIndex].ClearCheckRoom();
    }

    public Room CurRoom()
    {
        return Rooms[CurIndex];
    }
 

    public void BombLogic(Vector3 pos) // getcomponent를 안 쓰고 적에게 폭발 데미지를 못줘서 여기다 구현
    {
        foreach (Agent a in rooms[curIndex].Agents)
        {
            if (Vector3.Distance(pos, a.gameObject.transform.localPosition) < 4f)
            {
                a.Damage(InGameManager.Instance.Power + DrugManager.Instance.power, WeaponValue.Knife);
                Debug.Log("폭발탄 : " + a.gameObject.name);
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
