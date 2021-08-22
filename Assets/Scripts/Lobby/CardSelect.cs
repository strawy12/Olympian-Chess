using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour, IPointerClickHandler
{
    public int cardNum;//이게 카드를 구분하는 고유 번호
    private Image image = null;

    void Start()
    {
        image = GetComponent<Image>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!DeckManager.Instance.isChosen[cardNum])Select();
    }

    void Update()
    {
        if (DeckManager.Instance.isSelected && DeckManager.Instance.card == cardNum) 
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
        DeckManager.Instance.Deselect();
        DeckManager.Instance.selectedCard(cardNum);
        DeckManager.Instance.select();
    }
}
