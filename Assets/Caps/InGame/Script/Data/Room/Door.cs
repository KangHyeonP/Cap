using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpened = false;

    private GameObject qMark;
    private Animator animator;
    public BoxCollider2D boxCol;

    [SerializeField]
    private Door nextDoor;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        qMark = transform.GetChild(0).gameObject;
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
            nextDoor.qMark.SetActive(true);
    }

    public void QMOff()
    {
        qMark.SetActive(false);
    }

    public void DoorLock()
    {
        boxCol.isTrigger = false;
    }

    public void DoorUnlock()
    {
        boxCol.isTrigger = true;
    }

    public void Doorcol(bool check)
    {
        boxCol.enabled = check;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isOpened = true;
        }
    }
}
