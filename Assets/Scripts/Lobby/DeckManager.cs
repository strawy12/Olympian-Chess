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
    private Sprite[] sprites = new Sprite[23];
    private int card;
    public bool isSelected = false;
    public bool[] isChosen;
    [SerializeField]
    private CardItemSO cardItemSO;
    private Carditem currentCard;
    public List<Carditem> myDeck = new List<Carditem>();

    //여기서 isChosen[카드 번호]가 true면 그 카드를 선택된거임

    private void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = cardItemSO.cardItems[i].sprite;
        }
    }
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
    }

    public Sprite GetSprite(int index)
    {
        return sprites[index];
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
}
