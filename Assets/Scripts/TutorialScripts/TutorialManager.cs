using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("��ư ������ �� ����")]
    [SerializeField]
    private AudioClip buttonClick;

    [HideInInspector]
    public bool isFadeOut = false;

    [HideInInspector]
    public bool isTypingSound_ing = false;

    [HideInInspector]
    public bool isTypingSound = false;

    [HideInInspector]
    public bool isSpeedTypingSound = false;

    #region ���� �����÷���Ʈ ����

    public bool isFirst = false;
    public bool isSecond = false;

    #endregion

    [HideInInspector]
    public bool is3Story = false;

    [HideInInspector]
    public bool is5Story = false;

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
    
    [HideInInspector]
    public bool clickturnBtn = false;
    
    [HideInInspector]
    public bool blackPawn = false;
    
    [HideInInspector]
    public bool blackPawn2 = false;
    
    [HideInInspector]
    public bool card = false;

    //[HideInInspector]
    public bool iscardPush = false;
    //����� ȭ����!!!!!!!!!11

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
        SoundManager.Instance.StartSoundPlay();
    }

    public void OnClickBtn()
    {
        SceneManager.LoadScene("Lobby");
    }
}
