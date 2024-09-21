using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoor : MonoBehaviour
{
    public Door door;
    public Door backDoor;
    public LockBackDoor lockBackDoor;
    public Room room;

    public bool isTouch;
    public bool doorCheck = false; // 다음방으로 넘어간 상태인지 체크
    private BoxCollider2D boxcol;
    public AI[] agent;

    private void Awake()
    {
        boxcol = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        door.Doorcol(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouch) Interact();
        if(!doorCheck)
        {
            StopAgent();
        }
    }

    public void Interact()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (InGameManager.Instance.key >= 1)
            {
                InGameManager.Instance.UpdateKey(-1);
                SoundManager.Instance.PlaySFX(SFX.UseKey);
                door.Doorcol(true);
                backDoor.Doorcol(true);
                lockBackDoor.DisCol();
                boxcol.enabled = false;
                door.isOpened = true;
            }
        }
    }

    public void StopAgent()
    {
        if (door.boxCol.isTrigger) return;

        doorCheck = true;

        Debug.Log("차이 값 기존 도어 락: " + (room.gameObject.transform.position.x - InGameManager.Instance.player.transform.position.x));
        if (room.gameObject.transform.position.x > InGameManager.Instance.player.transform.position.x) return;

        foreach(Agent a in agent)
        {
            a.DisPlayerRoom();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))   isTouch = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) isTouch = false;
    }
}
