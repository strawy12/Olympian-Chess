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
        Debug.Log(currentNum);
        DeckManager.Instance.isChosen[currentNum] = false;
        if (carditem != null) DeckManager.Instance.RemoveMyDeck(carditem);

        currentNum = DeckManager.Instance.GetCard();
        carditem = DeckManager.Instance.GetCurrentCard();
        image.sprite = carditem.sprite;

        DeckManager.Instance.AddMyDeck(carditem);
        DeckManager.Instance.SetIsSelected(false);

        DeckManager.Instance.isChosen[currentNum] = true;
    }
}
