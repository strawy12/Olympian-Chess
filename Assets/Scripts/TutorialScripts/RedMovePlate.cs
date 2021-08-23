using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovePlate : MonoBehaviour
{
    private WhitePPawn WP;
   
    void Start()
    {
        WP = FindObjectOfType<WhitePPawn>();
    }

    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log("qweqwe");
        Ready();
        TutorialManager.Instance.is3Story = true;
    }

    private void Ready()
    {
        WP.DestroyMV_M();
        WP.DestroyMV_O();
        StartCoroutine(WP.PositionMove());
        WP.DestoryBP();
    }
}
