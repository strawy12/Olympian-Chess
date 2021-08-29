using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("���� ���� ����")]
    [SerializeField]
    private AudioClip startSound;

    [Header("ü���� ������ �� ����")]
    [SerializeField]
    private AudioClip moveSound;

    [Header("ü���� ���� �� ����")]
    [SerializeField]
    private AudioClip deadSound;

    public bool isTypingSound_ing = false;

    public bool isTypingSound = false;
    
    [HideInInspector]
    public bool is3Story = false;

    [HideInInspector]
    public bool is5Story = false;

    [HideInInspector]
    public bool is4Story = false;

    [HideInInspector]
    public bool is6Story = false;

    [HideInInspector]
    public bool is7Story = false;

    [HideInInspector]
    public bool isClicked = false;
    [HideInInspector]
    public bool isUsed = false;
    [HideInInspector]
    public bool turnEnd = true;
    public bool blackPawn = false;
    public bool blackPawn2 = false;
    public bool card = false;

    //����� ȭ����



    #region �̱���

    private static TutorialManager _instance;
    public static TutorialManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TutorialManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("TutorialManager").AddComponent<TutorialManager>();
                }
            }
            return _instance;
        }
    }

    #endregion
    void Start()
    {
        StartSoundPlay();

    }
    private void StartSoundPlay()
    {
        SoundManager.Instance.SoundPlay("StartSound", startSound);
    }

    public void MoveChessSound()
    {
        SoundManager.Instance.SoundPlay("MoveChess", moveSound);
    }

    public void DeadChessSound()
    {
        SoundManager.Instance.SoundPlay("DeadSound", deadSound);
    }

    //public void TypingSound(string name, AudioClip clip)
    //{
    //    //Debug.Log(name + "Sound");
    //    GameObject go = new GameObject(name + "Sound");
    //    AudioSource audioSource = go.AddComponent<AudioSource>();
    //    audioSource.clip = clip;
    //    if (isTypingSound)
    //    {
    //        isTypingSound_ing = true;
    //        if (isTypingSound_ing) return;
    //        audioSource.Play();
    //        isTypingSound_ing = false;


    //    }
    //    else
    //    {
    //        Destroy(go);
    //        isTypingSound_ing = false;
    //    }
    //}
}
