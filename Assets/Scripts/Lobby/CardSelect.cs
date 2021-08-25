using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler ,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private int cardNum;//�̰� ī�带 �����ϴ� ���� ��ȣ
    private Image image = null;
    private Carditem carditem;
    private bool isSelect;
    [SerializeField]
    private ScrollRect scroll;

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


    private void CardInfo()
    {
        Debug.Log(carditem.info);
    }

    public Carditem GetCardItem()
    {
        return carditem;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        scroll.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DeckManager.Instance.Drag();
        scroll.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scroll.OnEndDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DeckManager.Instance.PointerDown(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DeckManager.Instance.PointerUp();

    }
}