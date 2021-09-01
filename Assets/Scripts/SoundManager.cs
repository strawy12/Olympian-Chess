using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region �̱���

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

    [Header("���� ���� ����")]
    [SerializeField]
    private AudioClip startSound;

    [Header("ü���� ������ �� ����")]
    [SerializeField]
    private AudioClip moveSound;

    [Header("ü���� ���� �� ����")]
    [SerializeField]
    private AudioClip deadSound;

    [Header("��ư ������ �� ����")]
    [SerializeField]
    private AudioClip buttonClick;

    public void StartSoundPlay()
    {
        SoundPlay("StartSound", startSound);
    }

    public void MoveChessSound()
    {
        SoundPlay("MoveChess", moveSound);
    }

    public void DeadChessSound()
    {
       SoundPlay("DeadSound", deadSound);
    }

    public void ButtonClickSound()
    {
        SoundPlay("buttonClick", buttonClick);
    }
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
