using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    // Start is called before the first frame update

    bool openKey;
    bool touchBox;
    public bool isLock;
 
    Animator animator;

    void Start()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!touchBox || animator.enabled) return;

        OpenInputKey();
        OpenBox();
    }

    void OpenInputKey()
    {
        openKey = Input.GetKeyDown(KeyCode.E);
    }

    void OpenBox()
    {
        if (!openKey) return;

        Debug.Log(InGameManager.Instance.key);    
        if (isLock)
        {
            if (InGameManager.Instance.key > 0)
            {
                InGameManager.Instance.UpdateKey(-1);
            }                
            else
                return;
        }

        animator.enabled = true;

        Debug.Log("상자열기");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchBox = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchBox = false;
        }
    }
}


