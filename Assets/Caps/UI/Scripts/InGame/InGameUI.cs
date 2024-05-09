using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public SkillUI skillUI;

    private void Awake()
    {
        
    }

    void Start()
    {
        InitUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 체력, 방어력, 스킬 쿨타임, 돈 등 플레이어 기본 UI 수치 저장
    private void InitUI()
    {
        skillUI.coolTime = InGameManager.Instance.player.SkillDelay;
    }
}
