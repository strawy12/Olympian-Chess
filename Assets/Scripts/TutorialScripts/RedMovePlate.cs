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

    private void OnMouseUp()
    {
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
