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

    [Header("Ÿ���� ����")]
    [SerializeField]
    private AudioClip typingSound;

    public bool isTyiingSound;
    
    //[HideInInspector]
    //public bool is2StoryEnd = false;

    [HideInInspector]
    public bool is3Story = false;

    [HideInInspector]
    public bool is5Story = false;

    //[HideInInspector]
    //public bool is5StoryEnd = false;

    [HideInInspector]
    public bool is6Story = false;

    //[HideInInspector]
    //public bool is6StoryEnd = false;

    [HideInInspector]
    public bool is7Story = false;

    //[HideInInspector]
    //public bool is8Stroy = false;

    [HideInInspector]
    public bool isClicked = false;
    [HideInInspector]
    public bool isUsed = false;
    [HideInInspector]
    public bool turnEnd = false;
    [HideInInspector]
    public bool blackPawn = true;
    [HideInInspector]
    public bool blackPawn2 = true;

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
        TypingSound();
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

    public void TypingSound()
    {
        SoundManager.Instance.TypingSound("TypingSound", typingSound);
    }
}
