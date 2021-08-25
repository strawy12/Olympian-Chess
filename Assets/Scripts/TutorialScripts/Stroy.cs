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

    private bool isTyping = false;
    private bool isTyping_ing = false;

    private WaitForSeconds delayStory = new WaitForSeconds(4f); 
    void Start()
    {
        SettingStory();
    }

    private void SettingStory()
    {
        StartCoroutine(StoryStart());
    }

    private IEnumerator StoryStart()
    {
        isTyping = true;
        story1();
        yield return delayStory;

        story2();
        yield return delayStory;

        story3();
        yield return delayStory;

        story4();
        yield return delayStory;

        story5();
        yield return delayStory;

        story6();
        yield return delayStory;

        story7();
        yield return delayStory;

        story8();
        yield return delayStory;

        story9();
        yield return delayStory;
        isTyping = false;
    }

    private IEnumerator TypingEffect(Text _typingText,string _message,float _speed)
    {
        for (int i = 0; i < _message.Length; i++)
        {
            _typingText.text = _message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
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
        if(TutorialManager.Instance.is3Story)
        {
            StartCoroutine((TypingEffect(stroyText, story[3], speed)));
        }
    }
    private void story5()
    {
        TutorialManager.Instance.is5Story = true;   
        StartCoroutine((TypingEffect(stroyText, story[4], speed)));
    }
    private void story6()
    {

        //Write conditional code    
      
        if (TutorialManager.Instance.is6Story)
        {
            StartCoroutine((TypingEffect(stroyText, story[5], speed)));
        }
        
    }
    private void story7()
    {
        TutorialManager.Instance.is7Story = true;

        //Write conditional code
        if (TutorialManager.Instance.is7Story)
        {
            StartCoroutine((TypingEffect(stroyText, story[6], speed)));
        }
    }
    private void story8()
    {
        StartCoroutine((TypingEffect(stroyText, story[7], speed)));
    }
    private void story9()
    {
        StartCoroutine((TypingEffect(stroyText, story[8], speed)));
    }
    #endregion

}
