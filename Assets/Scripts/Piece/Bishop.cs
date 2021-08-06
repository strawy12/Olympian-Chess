using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : MonoBehaviour
{
    Vector3 originPos;
    void Start()
    {
        SetPositon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetPositon()
    {
        originPos = new Vector3(-2.4f, -1.8f, 0f);
    }
}
