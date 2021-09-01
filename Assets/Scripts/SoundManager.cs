using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê

    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("SoundManager").AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    public void SoundPlay(string name,AudioClip clip)
    {
        //Debug.Log(name + "Sound");
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void TypingSound(string name, AudioClip clip)
    {
        //Debug.Log(name + "Sound");
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        Debug.Log(TutorialManager.Instance.isTypingSound);
        if(TutorialManager.Instance.isTypingSound)
        {
            TutorialManager.Instance.isTypingSound_ing = true;
            if (TutorialManager.Instance.isTypingSound_ing) return;
            audioSource.Play();
            TutorialManager.Instance.isTypingSound_ing = false;
        }

        else
        {
            Destroy(go);
            TutorialManager.Instance.isTypingSound_ing = false;
        }
    }
}
