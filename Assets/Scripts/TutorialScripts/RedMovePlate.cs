using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovePlate : MonoBehaviour
{
    public static bool isClicke = false;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log("������");
        DestroyMovePlates();
    }

    private void DestroyMovePlates()
    {
        isClicke = true;
    }
}
