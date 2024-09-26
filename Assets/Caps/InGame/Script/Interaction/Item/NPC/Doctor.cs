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

    string firstDoctorText = "도박 한 번 해볼래?";
    string maxGuageText = "넌 가망이 없어...";
    string acceptText = "끊어라, 그러다 병신된다.";
    string refuseText = "돈 없어? 그럼 꺼져";

    /*
     string firstDoctorText = "도박 한 번 해볼래?"; You wanna try?
string maxGuageText = "넌 가망이 없어..."; You're hopeless.
string acceptText = "끊어라, 그러다 병신된다."; Quit it, jerk.
string refuseText = "돈 없어? 그럼 꺼져"; Flat broke? Then get the hell out. 
     * */
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

        if (InGameManager.Instance.drugGauge == 100) //신기루라서 못할 때 텍스트
        {
            chatPrevent = true;
            chatText.text = "";
            for (int i = 0; i < maxGuageText.Length; i++)
            {
                chatText.text += maxGuageText[i];
                yield return new WaitForSeconds(0.1f);
            }
            chatPrevent = false;
        }

        else
        {
            chatPrevent = true;
            if (!firstCheck)
            {
                for (int i = 0; i < firstDoctorText.Length; i++) // 처음 말걸 때 텍스트
                {
                    chatText.text += firstDoctorText[i];
                    yield return new WaitForSeconds(0.1f);
                }
                firstCheck = true;
            }
            else
            {
                chatText.text = "";
                if (InGameManager.Instance.money < 10)
                {
                    for (int i = 0; i < refuseText.Length; i++) // 돈 없을 떄 텍스트
                    {
                        chatText.text += refuseText[i];
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    chatText.text = "";
                    RemoveGuage();
                    for (int i = 0; i < acceptText.Length; i++) // 치료 받았을 때 텍스트
                    {
                        chatText.text += acceptText[i];
                        yield return new WaitForSeconds(0.1f);
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
