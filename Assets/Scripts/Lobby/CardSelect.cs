using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler ,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private int cardNum;//이게 카드를 구분하는 고유 번호
    private Image image = null;
    private Carditem carditem;
    private bool isSelect;
    [SerializeField]
    private ScrollRect scroll;
    private bool isDrag;

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
        if (isDrag) return;

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

    public Carditem GetCardItem()
    {
        return carditem;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        scroll.OnBeginDrag(eventData);
        isDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DeckManager.Instance.Drag();
        scroll.OnDrag(eventData);
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scroll.OnEndDrag(eventData);
        isDrag = false;
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
