using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("게임 시작 사운드")]
    [SerializeField]
    private AudioClip startSound;

    [Header("체스말 움직일 떄 사운드")]
    [SerializeField]
    private AudioClip moveSound;

    [Header("체스말 죽을 때 사운드")]
    [SerializeField]
    private AudioClip deadSound;

    [Header("타이핑 사운드")]
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

    //우수안 화이팅



    #region 싱글톤

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
