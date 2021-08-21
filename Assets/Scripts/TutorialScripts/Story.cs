using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Story : MonoBehaviour
{
    [Header("타이핑 사운드")]
    [SerializeField]
    private AudioClip typingSoound;
    [SerializeField]
    private Text stroy;
    [SerializeField]
    private string[] message = new string[5];
    [SerializeField]
    private float speed = 0.1f;
    public int arr = 0;

    private bool isTyping = true;

    void Start()
    {
        Stroy();
    }

    void Update()
    {
        
    }

    private void Stroy()
    {
        StartCoroutine(StroyAni(stroy, message[arr], speed));
        SoundTyping();
    }

    private IEnumerator StroyAni(Text _typingText, string _message, float _speed)
    {
        if(arr<5)
        {
            isTyping = true;
            for (int i = 0; i < _message.Length; i++)
            {
                _typingText.text = _message.Substring(0, i + 1);
                yield return new WaitForSeconds(_speed);
            }
            arr++;
        }
            
    }

    private void SoundTyping()
    {
        if(isTyping)
        {
            SoundManager.Instance.SoundPlay("typingsound", typingSoound);
        }
    }

}
