using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        TutorialManager.Instance.isClicked = true;
    }
}
