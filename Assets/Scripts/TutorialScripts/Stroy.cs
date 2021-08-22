using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stroy : MonoBehaviour
{
    [SerializeField]
    private Text stroyText;
    [SerializeField]
    private string[] story;
    [SerializeField]
    private float speed = 0.4f;

    private WaitForSeconds delayStory = new WaitForSeconds(6f); 
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
    }

    private IEnumerator TypingEffect(Text _typingText,string _message,float _speed)
    {
        for (int i = 0; i < _message.Length; i++)
        {
            _typingText.text = _message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }

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
        // Write conditional code

        StartCoroutine((TypingEffect(stroyText, story[2], speed)));
    }
    private void story4()
    {
        //Write conditional code

        StartCoroutine((TypingEffect(stroyText, story[3], speed)));
    }
    private void story5()
    {
        //Write conditional code

        StartCoroutine((TypingEffect(stroyText, story[4], speed)));
    }
    private void story6()
    {
        //Write conditional code

        StartCoroutine((TypingEffect(stroyText, story[5], speed)));
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
    #endregion

}
