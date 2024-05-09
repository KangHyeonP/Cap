using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance;
    public static InGameManager Instance => instance;

    // �ӽ� �÷��̾� ���
    public Player player;

    private bool isPause = false; //�ϴ� ����� �ΰ��� �����̹Ƿ� �Ͻ����� ����
    public bool IsPause => isPause;

    [Header("�÷��̾�")]
    private int hp;
    private int maxHp;

    private void Awake()
    {
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
        if (Instance == null)
        {
            instance = this;
        }
    }

    public void Pause(bool check)
    {
        isPause = check;

        if (isPause) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
}
