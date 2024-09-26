using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpened = false;

    private GameObject qMark;
    private Animator animator;
    public BoxCollider2D boxCol;
    private GameObject sideCol;
    bool isSide = false;
    float dist;


    [SerializeField]
    private Door nextDoor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        qMark = transform.GetChild(0).gameObject;

        if (transform.localScale.y != 1)
        {
            isSide = true;
            sideCol = transform.GetChild(1).gameObject;
        }

    }

    /*private void Update()
    {
        DoorOpen();
    }

    void DoorOpen()
    {
        if (!isOpened || animator.enabled) return;

        animator.enabled = true;
    }*/

    public void nextQMOn()
    {
        if (!nextDoor.animator.enabled)
            nextDoor.qMark.SetActive(true);
    }

    public void QMOff()
    {
        qMark.SetActive(false);
    }

    public void DoorLock()
    {
        boxCol.isTrigger = false;
        DoorClose();
    }

    public void DoorUnlock()
    {
        boxCol.isTrigger = true;
    }

    IEnumerator DoorReverse()
    {
        yield return new WaitForSeconds(0.3f);
        transform.localScale += new Vector3(2, 0, 0);
    }


    public void DoorClose()
    {
        if (!isOpened) return;

        if (isSide) sideCol.SetActive(false);
        animator.SetTrigger("Close");
        isOpened = false;

        if (transform.localScale.x < 0)
            StartCoroutine(DoorReverse());
    }

    void DoorOpen()
    {
        SoundManager.Instance.PlaySFX(SFX.Door_Open);

        if (isSide)
        {
            if (dist < 0)
                transform.localScale -= new Vector3(2, 0, 0);

            animator.SetTrigger("Side");

            sideCol.SetActive(true);
        }
        else
        {
            if (dist > 0)
            {
                animator.SetTrigger("Up");
            }
            else
            {
                animator.SetTrigger("Down");
            }
        }

        isOpened = true;
    }




    public void Doorcol(bool check)
    {
        boxCol.enabled = check;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpened)
        {
            if (isSide) dist = (transform.position.x - InGameManager.Instance.player.transform.position.x);
            else dist = (transform.position.y - InGameManager.Instance.player.transform.position.y);

            DoorOpen();
        }
    }
}
