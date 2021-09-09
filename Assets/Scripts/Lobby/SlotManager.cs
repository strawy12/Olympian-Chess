using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler
{
    private Image image = null;
    [SerializeField]
    private Carditem carditem;
    private int currentNum = -1;
    private 

    void Start()
    {
        image = GetComponent<Image>();
        if (DeckManager.Instance.GetDeck() == null) return;
        for (int i = 0; i < DeckManager.Instance.GetDeck().Length; i++)
        {
            if (i == Child())
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
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0))
            DeckManager.Instance.PointerDown(carditem);
        SoundManager.Instance.Button();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
            DeckManager.Instance.PointerUp();

        if (DeckManager.Instance.IsInfoActive())
        {
            DeckManager.Instance.InActiveInfo();
        }
    }

    private void CardIn()
    {
        if (carditem != null)
        {
            CurrentNum(carditem);

            if (currentNum != -1)
            {
                DeckManager.Instance.SetIsChosen(currentNum, false);
            }
        }

        currentNum = DeckManager.Instance.GetCard();
        carditem = DeckManager.Instance.GetCurrentCard();
        image.sprite = carditem.sprite;

        DeckManager.Instance.RemoveMyDeck(Child());
        DeckManager.Instance.InsertMyDeck(Child(), carditem);
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

        for (int i = 0; i < cards.cardItems.Length; i++)
        {
            if (cards.cardItems[i].name == card)
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

            if (carditem.name == "")
            {
                currentNum = -1;
                break;
            }

            else if (cards.cardItems[i].name == carditem.name)
            {
                currentNum = i;
                Debug.Log(i + cards.cardItems[i].name);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DeckManager.Instance.Drag();
        if (DeckManager.Instance.IsInfoActive())
        {
            DeckManager.Instance.InActiveInfo();
        }
    }
}
