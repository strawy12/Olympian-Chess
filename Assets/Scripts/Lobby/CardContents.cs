using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContents : MonoBehaviour
{
    [SerializeField]
    private CardItemSO cards;

    void Start()
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            transform.GetChild(i).GetComponentInChildren<CardSelect>().SetCardItem(cards.cardItems[i]);
        }
    }
}
