using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stroy : MonoBehaviour
{
    [Header("Ÿ�� ����")]
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
    private bool deckClicked = true; //deckClicked�� false�� �ٲ��ּ���

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
        }
    }

    private IEnumerator TypingEffect(Text _typingText, string _message, float _speed)
    {
        isTyping_ing = true;
        TutorialManager.Instance.isTyiingSound = true;
        for (int i = 0; i < _message.Length; i++)
        {
            _typingText.text = _message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
        TutorialManager.Instance.isTyiingSound = false;
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

    #region ���丮����
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
        TutorialManager.Instance.blackPawn = false;
        StartCoroutine((TypingEffect(stroyText, story[2], speed)));
    }
    private void story4()
    {
        StartCoroutine((TypingEffect(stroyText, story[3], speed)));
    }
    private void story5()
    {
        if (TutorialManager.Instance.card) return;
        TutorialManager.Instance.card = false;

        StartCoroutine((TypingEffect(stroyText, story[4], speed)));
    }
    private void story6()
    {

        StartCoroutine((TypingEffect(stroyText, story[5], speed)));
    }
    private void story7()
    {
        //Write conditional code
        if (TutorialManager.Instance.blackPawn2) return;
        TutorialManager.Instance.blackPawn2 = false;
        StartCoroutine((TypingEffect(stroyText, story[6], speed)));

    }
    private void story8()
    {
        StartCoroutine((TypingEffect(stroyText, story[7], speed)));
    }
    private void story9()
    {
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
                    isTyping = true;
                }
            }
            else if (index == 5)
            {
                //õ���� ����ϸ� deckClicked�� true�� �ٲ��ּ��� ȭ����!!

                index++;
                isTyping = true;
                TutorialManager.Instance.card = false;

            }
            else if (index == 7)
            {
                if (!TutorialManager.Instance.blackPawn2)
                {
                    Debug.Log("7" + TutorialManager.Instance.blackPawn2);
                    index++;
                    isTyping = true;
                }
            }
            else
            {
                isTyping = true;
                index++;
            }

            StartStory(index);

        }
        else
        {
            speed = 0.04f;
        }



        Debug.Log(index);
    }
    #endregion
}