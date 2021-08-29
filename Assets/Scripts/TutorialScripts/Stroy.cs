using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        switch (num)
        {
            case 1:
                story1();
                break;

            case 2:
                story2();
                break;

            case 3:
                story3();
                break;

            case 4:
                story4();
                break;

            case 5:
                story5();
                break;

            case 6:
                story6();
                break;

            case 7:
                story7();
                break;

            case 8:
                story8();
                break;

            case 9:
                story9();
                break;

            case 10:
                Debug.Log("튜툐리얼을 완료하셧습니당");
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

    //private void Sound()
    //{
    //    if(isTyping)
    //    {
    //        Debug.Log("ytpyip");
    //        isTyping_ing = true;
    //        if (isTyping_ing) return;
    //        SoundManager.Instance.SoundPlay("typing", typingSound);

    #region 스토리진행
    private void story1()
    {
        StartCoroutine((TypingEffect(stroyText, story[0], speed)));
    }

    private void story2()
    {
        StartCoroutine((TypingEffect(stroyText, story[1], speed)));
    }

    private void story3()
    {
       if (TutorialManager.Instance.blackPawn) return;
        TutorialManager.Instance.is3Story = true;
        TutorialManager.Instance.blackPawn = true;

        StartCoroutine((TypingEffect(stroyText, story[2], speed)));
    }
    private void story4()
    {
        if (TutorialManager.Instance.card) return;
        TutorialManager.Instance.card = true;
        StartCoroutine((TypingEffect(stroyText, story[3], speed)));
    }
    private void story5()
    {
        if (!TutorialManager.Instance.clickturnBtn) return;
        TutorialManager.Instance.clickturnBtn = false;
        TutorialManager.Instance.is5Story = true;
        StartCoroutine((TypingEffect(stroyText, story[4], speed)));
    }
    private void story6()
    {
        if (!TutorialManager.Instance.turnEnd) return;
        TutorialManager.Instance.turnEnd = false;
        TutorialManager.Instance.is6Story = true;

        StartCoroutine((TypingEffect(stroyText, story[5], speed)));
    }
    private void story7()
    {
        if (TutorialManager.Instance.blackPawn2) return;
        TutorialManager.Instance.blackPawn2 = true;
        TutorialManager.Instance.is7Story = true;
        StartCoroutine((TypingEffect(stroyText, story[6], speed)));

    }
    private void story8()
    {
        StartCoroutine((TypingEffect(stroyText, story[7], speed)));
    }
    private void story9()
    {
        TutorialManager.Instance.isFadeOut = true;
        StartCoroutine((TypingEffect(stroyText, story[8], speed)));
    }
    private void OnMouseUp()
    {
        if (!isTyping)
        {
            if (index == 3)
            {
                if (!TutorialManager.Instance.blackPawn)
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

        Debug.Log(index);
    }
    #endregion
}