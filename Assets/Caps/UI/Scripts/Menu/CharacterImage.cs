using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImage : MonoBehaviour
{
    private Image characterImage;
    [SerializeField]
    private Sprite[] characterSprites;
    private int curIndex = 0;
    private int maxIndex = 99;
    [SerializeField]
    private Image[] buttons;
    [SerializeField]
    private Sprite[] sprites;

    private bool[] lockCheck;

    private void Awake()
    {
        characterImage = GetComponent<Image>();
        Init();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        maxIndex = characterSprites.Length - 1;
    }

    public void UpdateIndex(int num)
    {
        if (num < 0)
        {
            if (curIndex > 0) curIndex--;
        }
        else
        {
            if (curIndex < maxIndex) curIndex++;
        }

        if (curIndex == 0) buttons[0].sprite = sprites[1];
        else buttons[0].sprite = sprites[0];

        if (curIndex == maxIndex) buttons[1].sprite = sprites[1];
        else buttons[1].sprite = sprites[0];

        // 메인 캐릭터 이미지 갱신
        if(!DataManager.Instacne.JsonClass._PlayerData.playerLock[curIndex]) characterImage.sprite = characterSprites[4];
        else characterImage.sprite = characterSprites[curIndex];
    }

    public void SelectButton()
    {
        if (DataManager.Instacne.JsonClass._PlayerData.playerLock[curIndex])
        {
            DataManager.Instacne.DefaultData.SettingValue(curIndex);
            GameManager.Instance.playerCheck = true;
            GameManager.Instance.selectCharacter = (ECharacters)curIndex;
        }
    }

    // 임시용 현재 UI화면 lock 풀기
    public void UnLockButton()
    {
        DataManager.Instacne.UpdateLock(curIndex);
        characterImage.sprite = characterSprites[curIndex];
    }
}
