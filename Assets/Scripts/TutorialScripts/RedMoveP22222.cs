using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMoveP22222 : MonoBehaviour
{
    private WhiteP2 wp;

    void Start()
    {
        wp = FindObjectOfType<WhiteP2>();
    }

    private void OnMouseUp()
    {
        Debug.Log("qweqwe");
        Ready();
    }

    private void Ready()
    {
        wp.DestroyMV_O();
        wp.DestroyMV_M();
        StartCoroutine(wp.PositionMove());
        wp.DestoryBP();
    }
}
