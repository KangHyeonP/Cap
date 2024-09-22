using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Doctor : MonoBehaviour
{
    public Room room;
    
    bool isDoctor;
    bool touchDoctor;
    bool firstCheck;
    bool completeCure;
    bool chatPrevent = false;
    bool dieDoctor = false;

    string firstDoctorText = "�� �� �װڴ�. ������?";
    string maxGuageText = "�� �ȵ�";
    string acceptText = "�� �� �׸� �ĸԾ� ����";
    string refuseText = "�� ����? �׷� ����";
    public TextMeshPro chatText;
    public float doctorDistance = 3;
    Coroutine chatCoroutine;

    void Update()
    {
        if (dieDoctor) return;

        CheckNeedDoctor();
        InteractDoctor();

        if (Vector2.Distance(InGameManager.Instance.player.transform.position, this.transform.position) > doctorDistance)
        {

            if (!completeCure && chatCoroutine != null)
            {
                StopCoroutine(chatCoroutine);
                chatCoroutine = null;
                chatPrevent = false;
            }
            else
            {
                chatText.text = "";
                firstCheck = false;
            }
        }

        DieDoctor();
    }

    void CheckNeedDoctor()
    {
        if (completeCure) return;
        
        isDoctor = Input.GetKeyDown(KeyCode.E);
    }

    public void InteractDoctor()
    {
        if (!completeCure && isDoctor && touchDoctor && !chatPrevent)
        {
            chatCoroutine = StartCoroutine(ChatDoctor());
        }
    }

    public void RemoveGuage()
    {
        InGameManager.Instance.DecreaseDrug();
        completeCure = true;
    }

    IEnumerator ChatDoctor()
    {
        GameManager.Instance.UpdateDiaryDate((int)EDiaryValue.Unlicensed_Doctor);

        if (InGameManager.Instance.drugGauge == 100) //�ű��� ���� �� �ؽ�Ʈ
        {
            chatPrevent = true;
            chatText.text = "";
            for (int i = 0; i < maxGuageText.Length; i++)
            {
                chatText.text += maxGuageText[i];
                yield return new WaitForSeconds(0.15f);
            }
            chatPrevent = false;
        }

        else
        {
            chatPrevent = true;
            if (!firstCheck)
            {
                for (int i = 0; i < firstDoctorText.Length; i++) // ó�� ���� �� �ؽ�Ʈ
                {
                    chatText.text += firstDoctorText[i];
                    yield return new WaitForSeconds(0.15f);
                }
                firstCheck = true;
            }
            else
            {
                chatText.text = "";
                if (InGameManager.Instance.money < 10)
                {
                    for (int i = 0; i < refuseText.Length; i++) // �� ���� �� �ؽ�Ʈ
                    {
                        chatText.text += refuseText[i];
                        yield return new WaitForSeconds(0.15f);
                    }
                }
                else
                {
                    chatText.text = "";
                    RemoveGuage();
                    for (int i = 0; i < acceptText.Length; i++) // ġ�� �޾��� �� �ؽ�Ʈ
                    {
                        chatText.text += acceptText[i];
                        yield return new WaitForSeconds(0.15f);
                    }

                    yield return new WaitForSeconds(3.0f);
                    chatText.text = "";
                }

            }
            chatPrevent = false;
        }

    }

    public void DieDoctor()
    {
        if (completeCure && room.RoomState != RoomState.being)
        {
            gameObject.SetActive(false);
            dieDoctor = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchDoctor = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchDoctor = false;
        }
    }
}
