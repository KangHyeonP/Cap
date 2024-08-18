using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DictionaryUI : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Sprite[] ilusts;
    [SerializeField]
    private Sprite[] icons;

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI descText;
    [SerializeField]
    private Image image;
    [SerializeField]
    private DictionaryContents[] contents;
    [SerializeField]
    private Scrollbar scrollbar;

    public float[] scrollvalue;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Init();
    }

    void Start()
    {
    }

    public void ShowInformation()
    {
        int i = int.Parse(EventSystem.current.currentSelectedGameObject.name);

        if (contents[i].isLock)
        {
            i = 0;
        }

        nameText.text = contents[i].GetName();
        descText.text = contents[i].GetDescription();
        image.sprite = ilusts[i];

        Debug.Log("아이템 번호: " + i);
    }

    public void Init()
    {
        for (int i = 1; i < contents.Length; i++)
        {
            contents[i].isLock = true;
        }
    }


    public void MoveBookMark(int contentType)
    {
        scrollbar.value = scrollvalue[contentType];
    }

    public void ContentUpdate()
    {
        for (int i = 1; i < contents.Length; i++)
        {
            contents[i].LockUpdate();
        }
    }

    public void Close()
    {
        animator.SetBool("Open", false);
    }
    public void DictOff()
    {
        UIManager.Instance.IsDict = false;
        gameObject.SetActive(false);
    }
}