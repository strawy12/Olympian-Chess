using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour, IPointerClickHandler
{
    public int cardNum;//이게 카드를 구분하는 고유 번호
    private Image image = null;
    private Carditem carditem;
    private bool isSelect;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = carditem.sprite;
        gameObject.name = carditem.name;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!DeckManager.Instance.isChosen[cardNum])
        {
            Select();
        }
    }

    void Update()
    {
        if (DeckManager.Instance.isSelected && DeckManager.Instance.GetCard() == cardNum) 
        {
            transform.localScale = new Vector3(1.2f, 1.2f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (DeckManager.Instance.isChosen[cardNum])
        {
            image.color = Color.gray;
        }
        else
        {
            image.color = Color.white;
        }
    }

    private void Select()
    {
        DeckManager.Instance.SetIsSelected(false);
        DeckManager.Instance.SelectedCard(cardNum);
        DeckManager.Instance.SetCurrentCard(carditem);
        DeckManager.Instance.SetIsSelected(true);
    }

    public void SetCardItem(Carditem carditem)
    {
        this.carditem = carditem;
    }
}
