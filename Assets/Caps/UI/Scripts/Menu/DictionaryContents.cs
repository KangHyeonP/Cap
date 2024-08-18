using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryContents : MonoBehaviour
{
    private string[] descs = new string[]
    {
      "���� ���� ����..",
        //���� 6��
        "���� ����", "��Ȳ ����", "��� ����", "�ʷ� ����", "�Ķ� ����", "���� ����",
        //������ 7��
        "��ó�� ġ������ ��� ��� ����", "����ź", "�ູ�� ����", "�Ⱦ��� �� �Ⱦ���", "�ִ� �� �� ���ο�", "�ֹ��� źâ", "�������� źâ",
        //����
        "��ù�", "���� ������", "�ָ�", "�Ƴ��ܴ�", "����Ʈ �̱�", "AK-47", "M870", "AWP",
        //ĳ����
        "�ؼ�", "����", "ī����",
        //��
        "1�� �ൿ��", "2�� �ൿ��", "3�� �ൿ��", "��ī��Ű", "���̸�", "����",
        //NPC
        "����", "������ �ǻ�",
        //����
        "�߰� ü��", "������", "�ݺ�",
      "Ȯ��ź", "����", "ó��",
      "���ݼ�", "����", "����ź",
      "����ݱ�", "ȸ��", "�ð� ��â",
      "����", "�߰� ����", "����������",
        //�����
        "���� ������", "������ �ش�", "�ҹ� ����ź",
      "���� �ߵ�", "���� �̽�", "���� ����",
      "������ ����", "�����ұ�", "���� ����",
        "�ű��"
   };

    private string[] names = new string[]
    {
        "���ر�",
        //���� 6��
        "���� ����", "��Ȳ ����", "��� ����", "�ʷ� ����", "�Ķ� ����", "���� ����",
        //������ 7��
        "�ش�", "����ź", "��", "��ź��", "����", "�ֹ��� źâ", "�������� źâ",
        //����
        "��ù�", "���� ������", "�ָ�", "�Ƴ��ܴ�", "����Ʈ �̱�", "AK-47", "M870", "AWP",
        //ĳ����
        "�ؼ�", "����", "ī����",
        //��
        "1�� �ൿ��", "2�� �ൿ��", "3�� �ൿ��", "��ī��Ű", "���̸�", "����",
        //NPC
        "����", "������ �ǻ�",
        //����
        "�߰� ü��", "������", "�ݺ�",
      "Ȯ��ź", "����", "ó��",
      "���ݼ�", "����", "����ź",
        "����ݱ�", "ȸ��", "�ð� ��â",
        "����", "�߰� ����", "����������",
        //�����
        "���� ������", "������ �ش�", "�ҹ� ����ź",
        "���� �ߵ�", "���� �̽�", "���� ����",
        "������ ����", "�����ұ�", "���� ����",
        "�ű��"
   };

    private Image iconImage;
    public bool isLock;
    public int itemNum;

    // Start is called before the first frame update
    void Awake()
    {
        itemNum = int.Parse(gameObject.name);
        if (itemNum == 0) return;
        iconImage = transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LockUpdate()
    {
        if (isLock)
            iconImage.color = Color.black;
        else
            iconImage.color = Color.white;
    }

    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    public string GetName()
    {
        return names[itemNum];
    }
    public string GetDescription()
    {
        return descs[itemNum];
    }
}
