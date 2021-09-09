using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        if (gameObject.name.Contains("CardButton"))
            SoundManager.Instance.Deck();

        else
            SoundManager.Instance.Button();
    }
}
