using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private AudioSource _audioSource;
    public double fadeInSeconds = 0.6;
    bool isFadeIn = true;
    double fadeDeltaTime1 = 0;

    double fadeOutSeconds = 1f;
    double fadeDeltaTime2 = 0;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();

    }

    void Update()
    {
        BGMFadeIn();
        BGMFadeOut();
    }

    private void BGMFadeIn()
    {
        if(isFadeIn)
        {
            fadeDeltaTime1 += Time.deltaTime/9f;
            if(fadeDeltaTime1>=fadeInSeconds)
            {
                fadeDeltaTime1 = fadeInSeconds;
                isFadeIn = false;
            }
            if (_audioSource.volume > 0.7f) return;
            else
                _audioSource.volume = (float)(fadeDeltaTime1 / fadeInSeconds);

        }

    }

    private void BGMFadeOut()
    {
        if(TutorialManager.Instance.isFadeOut)
        {
            fadeOutSeconds -= Time.deltaTime/9f;
            if(fadeOutSeconds>=fadeInSeconds)
            {
                fadeDeltaTime2 = fadeOutSeconds;
                TutorialManager.Instance.isFadeOut = false;
            }
            _audioSource.volume = (float)(1f - (fadeDeltaTime2 / fadeOutSeconds));
        }
    }
}
