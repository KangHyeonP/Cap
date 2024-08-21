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
    }

    private void Start()
    {
        GameManager.Instance.dictionaryUI = this;
        
        for(int i=1; i<contents.Length;i++)
        {
            contents[i].isUnLock = GameManager.Instance.DiaryDataCheck[i-1];
            contents[i].LockUpdate();
        }

        int j = 1;
        if (!contents[j].isUnLock) j = 0;

        nameText.text = contents[j].GetName();
        descText.text = contents[j].GetDescription();
        image.sprite = ilusts[j];
    }

    public void ShowInformation()
    {
        int i = int.Parse(EventSystem.current.currentSelectedGameObject.name);

        if (!contents[i].isUnLock)
        {
            i = 0;
        }

        nameText.text = contents[i].GetName();
        descText.text = contents[i].GetDescription();
        image.sprite = ilusts[i];

        Debug.Log("아이템 번호: " + i);
    }

    public void UpdateContent(int i) // 단일 업데이트
    {
        Debug.Log("단일 업데이트 실행");

        contents[i].UnLockContent();

    }


    public void MoveBookMark(int contentType)
    {
        scrollbar.value = scrollvalue[contentType];
    }

    public void Close()
    {
        animator.SetBool("Open", false);
    }
    public void DictOff()
    {
        if(UIManager.Instance != null) UIManager.Instance.IsDict = false;
        gameObject.SetActive(false);
    }
}