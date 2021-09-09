using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stroy : MonoBehaviour
{
    [Header("타자 사운드")]
    [SerializeField]
    private AudioClip typingSound;
    [SerializeField]
    private Text stroyText;
    [SerializeField]
    private string[] story;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Image endPanel;

    private bool isTyping = true;
    private bool isTyping_ing = true;
    private bool deckClicked = true; //deckClicked를 false로 바꿔주세용

    private int index = 1;

    private WaitForSeconds delayStory = new WaitForSeconds(6f);
    void Start()
    {
        StartStory(index);
    }


    private void StartStory(int num)
    {
        Debug.Log(num);
        switch (num)
        {
            case 1:
                Story1();
                break;

            case 2:
                Story2();
                break;

            case 3:
                Story3();
                break;

            case 4:
                Story4();
                break;

            case 5:
                Story5();
                break;

            case 6:
                Story6();
                break;

            case 7:
                Story7();
                break;

            case 8:
                Story8();
                break;

            case 9:
                Story9();
                break;

            case 10:
                Story10();
                break;

            case 11:
                Story11();
                break;

            case 12:
                Story12();
                break;

            case 13:
                Story13();
                break;

            case 14:
                Story14();
                break;

            case 15:
                Story15();
                break;

            case 16:
                Story16();
                break;

            case 17:
                Story17();
                break;

            case 18:
                TuToEnd();
                break;
        }
    }

    private IEnumerator TypingEffect(Text _typingText, string _message, float _speed)
    {
        isTyping_ing = true;
        isTyping = true;
        TutorialManager.Instance.isTypingSound = true;
        for (int i = 0; i < _message.Length; i++)
        {
            _typingText.text = _message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
        TutorialManager.Instance.isTypingSound = false;
        TutorialManager.Instance.isSpeedTypingSound = false;
        speed = 0.1f;
        isTyping_ing = false;
        isTyping = false;
    }

    #region 스토리진행
    private void Story1()
    {
        StartCoroutine((TypingEffect(stroyText, story[0], speed)));
    }

    private void Story2()
    {
        StartCoroutine((TypingEffect(stroyText, story[1], speed)));
    }

    private void Story3()
    {
       if (TutorialManager.Instance.blackPawnOut1) return;
        TutorialManager.Instance.is3Story = true;
        TutorialManager.Instance.blackPawnOut1 = true;

        StartCoroutine((TypingEffect(stroyText, story[2], speed)));
    }
    private void Story4()
    {
        if (TutorialManager.Instance.card) return;
        TutorialManager.Instance.card = true;
        TutorialManager.Instance.iscardPush = true;

        StartCoroutine((TypingEffect(stroyText, story[3], speed)));
    }
    private void Story5()
    {
        
        StartCoroutine((TypingEffect(stroyText, story[4], speed)));
    }
    private void Story6()
    {
      

        StartCoroutine((TypingEffect(stroyText, story[5], speed)));
    }
    private void Story7()
    {
       

        StartCoroutine((TypingEffect(stroyText, story[6], speed)));

    }
    private void Story8()
    {
        if (!TutorialManager.Instance.clickturnBtn) return;
        TutorialManager.Instance.clickturnBtn = false;
        TutorialManager.Instance.is5Story = true;

        StartCoroutine((TypingEffect(stroyText, story[7], speed)));
    }
    private void Story9()
    {
        if (!TutorialManager.Instance.turnEnd) return;
        TutorialManager.Instance.turnEnd = false;
        TutorialManager.Instance.is6Story = true;
        StartCoroutine((TypingEffect(stroyText, story[8], speed)));
    }

    private void Story10()
    {
        if (TutorialManager.Instance.blackPawn2) return;
        TutorialManager.Instance.is7Story = true;
        TutorialManager.Instance.blackPawn2 = true;

        StartCoroutine((TypingEffect(stroyText, story[9], speed)));

    }

    private void Story11()
    {

        StartCoroutine((TypingEffect(stroyText, story[10], speed)));

    }

    private void Story12()
    {
        StartCoroutine((TypingEffect(stroyText, story[11], speed)));

    }

    private void Story13()
    {
        StartCoroutine((TypingEffect(stroyText, story[12], speed)));

    }
    private void Story14()
    {
        StartCoroutine((TypingEffect(stroyText, story[13], speed)));

    }
    private void Story15()
    {
        StartCoroutine((TypingEffect(stroyText, story[14], speed)));

    }
    private void Story16()
    {
        StartCoroutine((TypingEffect(stroyText, story[15], speed)));

    }
    private void Story17()
    {
        if (!TutorialManager.Instance.clickturnBtn) return;
        TutorialManager.Instance.clickturnBtn = false;
        TutorialManager.Instance.is5Story = true;

        StartCoroutine((TypingEffect(stroyText, story[16], speed)));

    }

    private void TuToEnd()
    {
        Debug.Log("튜툐리얼을 완료하셧습니당");
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.WinOrLose(true);
        endPanel.transform.DOScale(1, 0.4f);
        User user = DataManager.Inst.LoadDataFromJson<User>();
        if (user == null)
        {
            Story1();
        }
        user.isTuto = true;

        DataManager.Inst.SaveDataToJson(user, true);
    }
    private void OnMouseUp()
    {
        if (!isTyping)
        {
            if (index == 3)
            {
                if (!TutorialManager.Instance.blackPawnOut1)
                {
                    index++;
                }

            }
            else if(index == 4)
            {
                if(!TutorialManager.Instance.card)
                {
                    index++;
                }
            }
            else if (index == 5)
            {
                if(TutorialManager.Instance.clickturnBtn)
                {
                    index++;

                }
            }
            else if (index == 6)
            {

                if (TutorialManager.Instance.turnEnd)
                {
                    index++;
                }
            }

            else if (index == 7)
            {
                if (!TutorialManager.Instance.blackPawn2)
                {
                    index++;
                }
            }
            else
            {
                index++;
            }
            StartStory(index);

        }
        else
        {
            TutorialManager.Instance.isSpeedTypingSound = true;
            speed = 0.04f;
        }

        //Debug.Log(index);
    }
    #endregion
}