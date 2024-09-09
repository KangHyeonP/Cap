using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isOpened = false;
    bool isLock;
    private GameObject qmMark;
    private Animator animator;
    private BoxCollider2D boxCol;

    [SerializeField]
    private Door nextDoor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        qmMark = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        DoorOpen();
    }

    void DoorOpen()
    {
        if (!isOpened || animator.enabled) return;

        animator.enabled = true;
    }

    public void nextQMOn()
    {
        if (!nextDoor.animator.enabled)
            nextDoor.qmMark.SetActive(true);
    }

    public void QMOff()
    {
        qmMark.SetActive(false);
    }

    public void DoorLock()
    {
        boxCol.isTrigger = false;
    }

    public void DoorUnlock()
    {
        boxCol.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOpened = true;
        }
    }
}
