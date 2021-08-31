using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardbufferManager : MonoBehaviour
{
    List<Carditem> myCardBuffer;
    List<Carditem> ohterCardBuffer;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public List<Carditem> GetMyCardBuffer()
    {
        return myCardBuffer;
    }
    public List<Carditem> GetOhterCardBuffer()
    {
        return ohterCardBuffer;
    }
    public void SetMyCardBuffer(List<Carditem> myCardBuffer)
    {
        this.myCardBuffer = myCardBuffer;
    }
    public void SetOhterCardBuffer(List<Carditem> ohterCardBuffer)
    {
        this.ohterCardBuffer = ohterCardBuffer;
    }
}
