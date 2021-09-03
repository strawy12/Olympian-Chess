using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region 싱글톤

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

    [Header("게임 시작 사운드")]
    [SerializeField]
    private AudioClip startSound;

    [Header("체스말 움직일 떄 사운드")]
    [SerializeField]
    private AudioClip moveSound;

    [Header("체스말 죽을 때 사운드")]
    [SerializeField]
    private AudioClip deadSound;

    [Header("버튼 눌렀을 때 사운드")]
    [SerializeField]
    private AudioClip buttonClick;

    [SerializeField]
    private AudioClip gameBGM;
    [SerializeField]
    private AudioClip lobbyBGM;
    [SerializeField]
    private AudioClip lobbyBGM2;


    [SerializeField]
    private AudioClip coinSound;
    [SerializeField]
    private AudioClip matchSound;
    [SerializeField]
    private AudioClip buttonSound;

    private AudioSource bgmAudio;
    private AudioSource effectAudio;

    private void Awake()
    {
        SoundManager[] sms = FindObjectsOfType<SoundManager>();

        if (sms.Length != 1)
        {
            for (int i = 0; i < sms.Length; i++)
            {
                if (sms[i] != this)
                {
                    Destroy(sms[i].gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);

        bgmAudio = GetComponent<AudioSource>();
        effectAudio = transform.GetChild(0).GetComponent<AudioSource>();
        VolumeSetting();
    }

    public void VolumeSetting()
    {
        User user = NetworkManager.Inst.LoadDataFromJson<User>();
        bgmAudio.volume = user.bgmVolume;
        effectAudio.volume = user.effectVolume;
    }

    public void BGMVolume(Slider slider)
    {
        bgmAudio.volume = slider.value / 10;
    }

    public void EffectVolume(Slider slider)
    {
        effectAudio.volume = slider.value / 10;
    }

    public void SetGameBGM()
    {
        bgmAudio.Stop();
        bgmAudio.clip = gameBGM;
        bgmAudio.Play();
    }

    public void SetLobbyBGM(int rand)
    {
        bgmAudio.Stop();
        if (rand == 0)
            bgmAudio.clip = lobbyBGM;
        else
            bgmAudio.clip = lobbyBGM2;
        bgmAudio.Play();
    }

    public void Buy()
    {
        effectAudio.PlayOneShot(coinSound);
    }

    public void Match()
    {
        effectAudio.PlayOneShot(matchSound);
    }

    public void Button()
    {
        effectAudio.PlayOneShot(buttonSound);
    }

    public void StartSound()
    {
        effectAudio.PlayOneShot(startSound);
    }

    public void MoveChessSound()
    {
        effectAudio.PlayOneShot(moveSound);
    }
    public void DeadChess()
    {
        effectAudio.PlayOneShot(deadSound);
    }

    public void SoundPlay(string name, AudioClip clip)
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
        if (TutorialManager.Instance.isTypingSound)
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
