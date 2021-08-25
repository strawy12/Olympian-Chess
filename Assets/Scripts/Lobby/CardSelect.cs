using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int cardNum;//이게 카드를 구분하는 고유 번호
    private Image image = null;
    private Carditem carditem;
    private bool isSelect;

    void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        image.sprite = carditem.sprite;
        gameObject.name = carditem.name;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!DeckManager.Instance.GetIsChosen(cardNum))
        {
            Select();
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

    public void ChangeColor(Color color)
    {
        image.color = color;
    }

    public void ChangeScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
