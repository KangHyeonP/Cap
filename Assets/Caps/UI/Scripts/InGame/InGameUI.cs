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

    // ü��, ����, ��ų ��Ÿ��, �� �� �÷��̾� �⺻ UI ��ġ ����
    private void InitUI()
    {
        skillUI.coolTime = InGameManager.Instance.player.SkillDelay;
    }
}
