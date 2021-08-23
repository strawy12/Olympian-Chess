using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("���� ���� ����")]
    [SerializeField]
    private AudioClip startSound;

    [HideInInspector]
    public bool is2StoryEnd = false;

    [HideInInspector]
    public bool is3Story = false;

    [HideInInspector]
    public bool is5Story = false;

    [HideInInspector]
    public bool is5StoryEnd = false;

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

    void Update()
    {
        
    }

    private void StartSoundPlay()
    {
        SoundManager.Instance.SoundPlay("StartSound", startSound);
    }
}
