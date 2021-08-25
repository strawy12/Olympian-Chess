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

        for (int i = 0; i < DeckManager.Instance.GetDeck().Length; i++)
        {
            if(i == Child())
            {
                ChangeSprite(DeckManager.Instance.GetDeck(i));
                carditem = DeckManager.Instance.GetMyDeck(i);
            }
        }
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
        if (carditem != null)
        {
            Debug.Log(carditem.name);
            CurrentNum(carditem);
            DeckManager.Instance.SetIsChosen(currentNum, false);
        }

        currentNum = DeckManager.Instance.GetCard();
        carditem = DeckManager.Instance.GetCurrentCard();
        image.sprite = carditem.sprite;

        DeckManager.Instance.SetIsSave(false);
        DeckManager.Instance.RemoveMyDeck(Child());
        DeckManager.Instance.InsertMyDeck(Child(),carditem);
        DeckManager.Instance.SetIsSelected(false);

        DeckManager.Instance.SetIsChosen(currentNum, true);
        DeckManager.Instance.ChangeCard();
    }

    private int Child()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform == transform.parent.GetChild(i))
            {
                return i;
            }

        }
        return 0;
    }

    private void ChangeSprite(string card)
    {
        CardItemSO cards = DeckManager.Instance.GetCardItemSO();

        for (int i = 0; i< cards.cardItems.Length; i++)
        {
            if(cards.cardItems[i].name == card)
            {
                image.sprite = cards.cardItems[i].sprite;
            }
        }
    }

    private void CurrentNum(Carditem carditem)
    {
        CardItemSO cards = DeckManager.Instance.GetCardItemSO();

        for (int i = 0; i < cards.cardItems.Length; i++)
        {
            if (cards.cardItems[i].name == carditem.name)
            {
                currentNum = i;
                Debug.Log(i + cards.cardItems[i].name);
            }
        }
    }
}
