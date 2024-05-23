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
    private int imageIndex = 0;
    private int maxIndex = 99;
    [SerializeField]
    private Image[] buttons;
    [SerializeField]
    private Sprite[] sprites;

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
            if (imageIndex > 0) imageIndex--;
        }
        else
        {
            if (imageIndex < maxIndex) imageIndex++;
        }

        // 메인 캐릭터 이미지 갱신
        if (imageIndex == 0) buttons[0].sprite = sprites[1];
        else buttons[0].sprite = sprites[0];

        if (imageIndex == maxIndex) buttons[1].sprite = sprites[1];
        else buttons[1].sprite = sprites[0];

        characterImage.sprite = characterSprites[imageIndex];
    }



}
