using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovePlate : MonoBehaviour
{
    private WhitePPawn wp;
    private WhiteP2 wwp;


    void Start()
    {
        wp = FindObjectOfType<WhitePPawn>();
        wwp = FindObjectOfType<WhiteP2>();
    }

    private void OnMouseUp()
    {
        Debug.Log("눌려써");

        if (TutorialManager.Instance.isFirst)
        {
            ReadyFirst();
            Debug.Log("눌려써");
        }
        else if (TutorialManager.Instance.isSecond)
        {
            ReadySecond();
            Debug.Log("눌려써");
        }
    }

    private void ReadyFirst()
    {
        StartCoroutine(wp.PositionMove());
        wp.DestroyMV_M();
        wp.DestroyMV_O();
        wp.DestoryBP();
        TutorialManager.Instance.isFirst = false;
    }

    private void ReadySecond()
    {
        wwp.DestroyMV_O();
        wwp.DestroyMV_M();
        StartCoroutine(wwp.PositionMove());
        wwp.DestoryBP();
        TutorialManager.Instance.isSecond = false;
    }
}
