using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum SoundType
    {
        ChessDie1,
        ChessDie2,
        ChessDie3,
        Deck,
        EndGame,
        StartGame,
        ChessDrop,
    }
    //public int AddSound(SoundType st)
    //{
    //    switch (st)
    //    {
    //        case SoundType.Deck:
    //            if (false == soundDic.ContainsKey(st))
    //            {

    //            }
    //        default:
    //            Debug.Log("아직 연결하지 않은 사운드를 불렀어!!");
    //            break;
    //    }

    //    GameObject go = Instantiate<GameObject>(soundDic[st]);
    //    return 0;
    //}

    public void SoundPlay(string name,AudioClip clip)
    {
        GameObject go = new GameObject(name + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }
}
