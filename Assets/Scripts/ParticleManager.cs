using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region SingleTon
    private static ParticleManager _instance = null;
    public static ParticleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ParticleManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("ParticleManager").AddComponent<ParticleManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public enum ParticleType
    {
        button,
    }

    public int AddParticle(ParticleType pt, Vector3 pos)
    {
        switch(pt)
        {
            case ParticleType.button:
                if(false==particleDic.ContainsKey(pt))
                {
                    particleDic[pt] = Resources.Load<GameObject>("Particle/P_button");
                }
                break;
        }
        if(particleDic[pt]==null)
        {
            Debug.LogWarning($"로딩을 못했어!!!ㅠㅅㅠ{pt}");
            return 0;
        }

        GameObject go = Instantiate<GameObject>(particleDic[pt], pos, Quaternion.identity);

        return 0;
    }

    private static Dictionary<ParticleType, GameObject> particleDic = new Dictionary<ParticleType, GameObject>();
}
