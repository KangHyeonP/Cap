using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public SkillUI skillUI;
    public Image DrugUI;
    public Image WeaponUI;

    public Sprite[] weaponSprites;

    private void Awake()
    {
        
    }

    void Start()
    {
        InitUI();
        WeaponInven(5); // ���� ĳ���͸��� Į�� �ٸ��ٸ� player���� �ʱ�ȭ�� ��Ű���� ����
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

    public void DrugInven(Sprite s)
    {
        DrugUI.sprite = s;
    }

    public void WeaponInven(int index)
    {
        WeaponUI.sprite = weaponSprites[index];
    }
}
