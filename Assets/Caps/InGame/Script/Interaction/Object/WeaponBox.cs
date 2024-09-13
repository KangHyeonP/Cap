using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    // Start is called before the first frame update

    bool openKey;
    bool touchBox;
    public bool isLock;

    public Item[] guns;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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

        StartCoroutine(DropWeapon());
        Debug.Log("상자열기");
    }
    IEnumerator DropWeapon()
    {
        yield return new WaitForSeconds(3f);
        int value = Random.Range(1, 51);
        Debug.Log(value);
        int index = -1;
        if (value <= 10)
        {
            index = 0;
        }
        else if (value <= 20)
        {
            index = 1;
        }
        else if (value <= 30)
        {
            index = 2;
        }
        else if (value <= 40)
        {
            index = 3;
        }
        else if (value <= 50)
        {
            index = 4;
        }

        Instantiate(guns[index], transform.position, transform.rotation);
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


