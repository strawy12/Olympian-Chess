using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private static DeckManager instance = null;
    public static DeckManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DeckManager>();
                if (instance == null)
                {
                    instance = new GameObject("DeckManager").AddComponent<DeckManager>();
                }
            }
            return instance;
        }
    }
    private int card;
    public bool isSelected = false;
    public bool[] isChosen = new bool[23];
    private Carditem currentCard;
    public List<Carditem> myDeck = new List<Carditem>();

    [SerializeField]
    CardContents cardContents;

    //여기서 isChosen[카드 번호]가 true면 그 카드를 선택된거임
    public int GetCard()
    {
        return card;
    }

    public void SelectedCard(int Card)
    {
        card = Card;
    }

    public void SetIsSelected(bool isTrue)
    {
        isSelected = isTrue;
        cardContents.IsClicked();
    }

    public void AddMyDeck(Carditem carditem)
    {
        myDeck.Add(carditem);
    }

    public void RemoveMyDeck(Carditem carditem)
    {
        myDeck.Remove(carditem);
    }

    public void SetCurrentCard(Carditem carditem)
    {
        currentCard = carditem;
    }

    public Carditem GetCurrentCard()
    {
        return currentCard;
    }

    public void SetIsChosen(int index, bool isTrue)
    {
        isChosen[index] = isTrue;
    }

    public bool GetIsChosen(int index)
    {
        return isChosen[index];
    }

    public void ChangeCard()
    {
        cardContents.IsClicked();
    }
}