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
    private float speed = 0.4f;

    private bool isTyping = true;
    private bool isTyping_ing = true;
    private bool deckClicked = true; //deckClicked를 false로 바꿔주세용

    private int index = 1;

    private WaitForSeconds delayStory = new WaitForSeconds(6f);
    void Start()
    {
        StartCoroutine(StoryStart());
        isTyping = true;
    }

    private IEnumerator StoryStart()
    {
        if (isTyping && index == 1)
        {
            story1();
            yield return delayStory;
        }
        if (isTyping && index == 2)
        {
            story2();
            yield return delayStory;
        }

        if (isTyping && index == 3)
        {
            story3();
            yield return delayStory;
        }

        if (isTyping && index == 4)
        {
            story4();
            yield return delayStory;
        }
        if (isTyping && index == 5)
        {
            story5();
            yield return delayStory;
        }
        if (isTyping && index == 6)
        {
            story6();
            yield return delayStory;
        }
        if (isTyping && index == 7)
        {
            story7();
            yield return delayStory;
        }
        if (isTyping && index == 8)
        {
            story8();
            yield return delayStory;
        }
        if (isTyping && index == 9)
        {
            story9();
            yield return delayStory;
        }
        isTyping = false;
    }

    private IEnumerator TypingEffect(Text _typingText, string _message, float _speed)
    {
        TutorialManager.Instance.isTyiingSound = true;
        for (int i = 0; i < _message.Length; i++)
        {
            _typingText.text = _message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
        TutorialManager.Instance.isTyiingSound = false;

        isTyping_ing = false;
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
        isTyping = false;
    }

    private void story2()
    {
        isTyping = false;
        StartCoroutine((TypingEffect(stroyText, story[1], speed)));
    }

    private void story3()
    {
        isTyping = false;
        TutorialManager.Instance.is3Story = true;
        StartCoroutine((TypingEffect(stroyText, story[2], speed)));
    }
    private void story4()
    {
        isTyping = false;
        StartCoroutine((TypingEffect(stroyText, story[3], speed)));
    }
    private void story5()
    {
        isTyping = false;
        TutorialManager.Instance.is5Story = true;
        StartCoroutine((TypingEffect(stroyText, story[4], speed)));
    }
    private void story6()
    {
        isTyping = false;
        TutorialManager.Instance.is6Story = true;
        StartCoroutine((TypingEffect(stroyText, story[5], speed)));
    }
    private void story7()
    {
        //Write conditional code

        isTyping = false;
        TutorialManager.Instance.is7Story = true;
        StartCoroutine((TypingEffect(stroyText, story[6], speed)));
        isTyping = false;

    }
    private void story8()
    {
        isTyping = false;
        StartCoroutine((TypingEffect(stroyText, story[7], speed)));
    }
    private void story9()
    {
        isTyping = false;
        StartCoroutine((TypingEffect(stroyText, story[8], speed)));
    }
    private void OnMouseUp()
    {

        if (index == 3)
        {
            if (!isTyping_ing && !TutorialManager.Instance.blackPawn)
            {
                index++;
                isTyping = true;
                isTyping_ing = true;
                StartCoroutine(StoryStart());
            }
        }
        else if (index == 5)
        {
            if (!isTyping_ing) //천벌을 사용하면 deckClicked를 true로 바꿔주세용 화이팅!!
            {
                index++;
                isTyping = true;
                isTyping_ing = true;
            }
        }
        else if (index == 7)
        {
            if (!isTyping_ing && !TutorialManager.Instance.blackPawn2)
            {
                Debug.Log("7" + TutorialManager.Instance.blackPawn2);
                index++;
                isTyping = true;
                isTyping_ing = true;
            }
        }
        else
        {
            if (!isTyping_ing)
            {
                index++;
                isTyping = true;
                isTyping_ing = true;
            }
        }
        Debug.Log(isTyping_ing);
        Debug.Log(isTyping);
        Debug.Log(index);
    }
    #endregion
}