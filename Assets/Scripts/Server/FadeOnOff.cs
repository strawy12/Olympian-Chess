using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOnOff : MonoBehaviour
{
    [SerializeField] private Text textUI = null;
    [SerializeField] private Image image = null;

    Color color;
    bool isFade = false;
    private bool isBreak = false;


    void Start()
    {
        if(image == null)
        {
            color = textUI.color;
        }
        else
        {
            color = image.color;
        }
    }
        

    void Update()
    {
        if (isBreak) return;

        if (image == null)
        {
            textUI.color = color;
        }
        else
        {
            image.color = color;
        }

        if (isFade)
        {
            color.a -= Time.deltaTime * 1.5f;
        }
        else
        {
            color.a += Time.deltaTime * 2f;
        }
        if (color.a < 0)
        {
            isFade = false;
        }
        else if (color.a > 1)
        {
            isFade = true;
        }
    }

    public void StartFadeOnOff()
    {
        StartCoroutine(FadeOnOffEffect());
    }

    IEnumerator FadeOnOffEffect()
    {
        while(color.a < 0)
        { 
            color.a += Time.deltaTime * 3f;
        }
        yield return null;
    }
}
