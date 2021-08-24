using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContents : MonoBehaviour
{
    [SerializeField]
    private CardItemSO cards;
    private List<CardSelect> cardSelects = new List<CardSelect>();

    private void Awake()
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            cardSelects.Add(transform.GetChild(i).GetComponentInChildren<CardSelect>());
            cardSelects[i].SetCardItem(cards.cardItems[i]);
        }
    }

    public void IsClicked()
    {
        for (int i = 0; i < cardSelects.Count; i++)
        {
            cardSelects[i].ChangeColor(Color.white);
            cardSelects[i].ChangeScale(1.0f);

            if(i == DeckManager.Instance.GetCard() && !DeckManager.Instance.GetIsChosen(i))
            {
                cardSelects[i].ChangeScale(1.2f);
            }

            else if(DeckManager.Instance.GetIsChosen(i))
            {
                cardSelects[i].ChangeColor(Color.gray);
            }
        }
    }
}
