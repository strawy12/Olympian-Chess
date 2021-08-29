using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingSound : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (TutorialManager.Instance.isSpeedTypingSound)
        {
            _audioSource.pitch = 1.65f;
        }
        else
        {
            _audioSource.pitch = 1f;
        }
        if (TutorialManager.Instance.isTypingSound == false)
        {
            if (_audioSource.mute) return;
            _audioSource.mute = true;
            Debug.Log("음소거");
        }
        else
        {

            if (_audioSource.mute == false) return;
            _audioSource.mute = false;
            Debug.Log("재생");
        }
        
    }

    
}
