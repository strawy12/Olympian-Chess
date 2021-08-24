using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour, IPointerClickHandler
{
    private Image image = null;
    [SerializeField]
    private Carditem carditem;
    private bool isUsed = false;
    int currentNum;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (DeckManager.Instance.isSelected)
        {
            CardIn();
        }
    }
    private void CardIn()
    {
        if (carditem != null && isUsed)
        {
            DeckManager.Instance.SetIsChosen(currentNum, false);
            DeckManager.Instance.RemoveMyDeck(carditem);
        }

        currentNum = DeckManager.Instance.GetCard();
        carditem = DeckManager.Instance.GetCurrentCard();
        image.sprite = carditem.sprite;

        DeckManager.Instance.AddMyDeck(carditem);
        DeckManager.Instance.SetIsSelected(false);

        DeckManager.Instance.SetIsChosen(currentNum, true);
        DeckManager.Instance.ChangeCard();

        isUsed = true;
    }
}
