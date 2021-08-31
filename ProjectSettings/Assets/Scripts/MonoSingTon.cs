using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonoSingTon<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    private static bool shuttingDown = false;
    private static object locker = new object();
    private static T inst = null;
    public static T Inst
    {
        get
        {
            if (shuttingDown)
            {
                Debug.LogError("[SingleTon] Instance" + typeof(T) + "already destroyed. Returning null.");
            }

            lock (locker)
            {
                if (inst == null)
                {
                    inst = FindObjectOfType<T>();
                    if (inst == null)
                    {
                        inst = new GameObject(typeof(T).ToString()).AddComponent<T>();
                    }
                }
                DontDestroyOnLoad(inst);
            }

            return inst;
        }
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

    private void OnDestroy()
    {
        shuttingDown = true;
    }
}
