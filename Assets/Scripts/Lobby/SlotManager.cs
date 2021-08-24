using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour, IPointerClickHandler
{
    private Image image = null;
    int currentNum;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
            if (DeckManager.Instance.isSelected)
            {
                    cardIn();
            }
    }
    private void cardIn()
    {
        DeckManager.Instance.isChosen[currentNum] = false;
        currentNum = DeckManager.Instance.card;
        image.sprite = DeckManager.Instance.sprites[DeckManager.Instance.card];
        DeckManager.Instance.Deselect();
        DeckManager.Instance.isChosen[DeckManager.Instance.card] = true;
    }

}
