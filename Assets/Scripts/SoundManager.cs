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
    private AudioClip tutorialBGM;
    [SerializeField]
    private AudioClip gameBGM, gameBGM2;
    [SerializeField]
    private AudioClip lobbyBGM, lobbyBGM2;


    [SerializeField]
    private AudioClip coinSound;
    [SerializeField]
    private AudioClip matchSound;
    [SerializeField]
    private AudioClip buttonSound;
    [SerializeField]
    private AudioClip winSound, loseSound;
    [SerializeField]
    private AudioClip deckSound;

    private AudioSource bgmAudio;
    private AudioSource effectAudio;

    private void Awake()
    {
        SoundManager[] smanagers = FindObjectsOfType<SoundManager>();
        if (smanagers.Length != 1)
        {
            Destroy(gameObject);
            return;
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
        if (bgmAudio == null) return;
        bgmAudio.volume = slider.value / 10;
    }

    public void EffectVolume(Slider slider)
    {
        if (effectAudio == null) return;
        effectAudio.volume = slider.value / 10;
    }

    public void SetTutorialSound()
    {
        bgmAudio.Stop();
        bgmAudio.clip = tutorialBGM;
        bgmAudio.Play();
    }

    public void SetGameBGM(int rand)
    {
        bgmAudio.Stop();

        if (rand == 0)
            bgmAudio.clip = gameBGM;
        else
            bgmAudio.clip = gameBGM2;

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

    public void Deck()
    {
        effectAudio.PlayOneShot(deckSound);
    }

    public void StartSound()
    {
        effectAudio.PlayOneShot(startSound);
    }

    public void WinOrLose(bool isWin)
    {
        bgmAudio.Stop();

        if (isWin)
            effectAudio.PlayOneShot(winSound);

        else
            effectAudio.PlayOneShot(loseSound);
    }
    public void MoveChessSound()
    {
        effectAudio.PlayOneShot(moveSound);
    }
    public void DeadChessSound()
    {
        effectAudio.PlayOneShot(deadSound);
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

    public float GetEffectVolume()
    {
        return effectAudio.volume;
    }

    public void StopBGM()
    {
        bgmAudio.Stop();
    }
}
