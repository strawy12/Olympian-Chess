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

    private int index = 1;

    private WaitForSeconds delayStory = new WaitForSeconds(6f);
    void Start()
    {
        StartCoroutine(StoryStart());
        isTyping = true;
    }

    private IEnumerator StoryStart()
    {
        story1();
        yield return delayStory;

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
        for (int i = 0; i < _message.Length; i++)
        {
            _typingText.text = _message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
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
    //    }
    //}

    #region 스토리진행
    private void story1()
    {
        StartCoroutine((TypingEffect(stroyText, story[0], speed)));
        isTyping = false;
    }

    private void story2()
    {
        StartCoroutine((TypingEffect(stroyText, story[1], speed)));
    }

    private void story3()
    {
        StartCoroutine((TypingEffect(stroyText, story[2], speed)));
        TutorialManager.Instance.is2StoryEnd = true;
    }
    private void story4()
    {
        //Write conditional code
        if (TutorialManager.Instance.is3Story)
        {
            StartCoroutine((TypingEffect(stroyText, story[3], speed)));
        }
    }
    private void story5()
    {
        //Write conditional code
        // turn
        TutorialManager.Instance.is5Story = true;
        StartCoroutine((TypingEffect(stroyText, story[4], speed)));
    }
    private void story6()
    {
        //Write conditional code
        if (TutorialManager.Instance.is5StoryEnd)
        {
            StartCoroutine((TypingEffect(stroyText, story[5], speed)));
        }
    }
    private void story7()
    {
        //Write conditional code

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
    private void OnMouseDown()
    {
        Debug.Log(isTyping_ing);
        Debug.Log(isTyping);
        Debug.Log(index);

        if (index != 5)
        {
            if (!isTyping_ing)
            {
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
    }
    #endregion

}
