using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";

    private int card;
    public bool isSelected = false;
    public bool[] isChosen = new bool[23];
    private Carditem currentCard;
    public List<Carditem> myDeck = new List<Carditem>();
    [SerializeField]
    private Deck deck;
    [SerializeField]
    private CardItemSO cards;

    [SerializeField]
    CardContents cardContents;

    //여기서 isChosen[카드 번호]가 true면 그 카드를 선택된거임
    private void Awake()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        //Application.persistentDataPath (나중에 안드로이드)
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();
        SettingIsChosen();
    }

    private void Start()
    {
        ChangeCard();
        SettingMyDeck();
    }

    private void LoadFromJson()
    {
        string json;

        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            deck = JsonUtility.FromJson<Deck>(json);
        }

        else
        {
            SaveToJson();
            LoadFromJson();
        }
    }

    private void SaveToJson()
    {
        SAVE_PATH = Application.dataPath + "/Save";

        string json = JsonUtility.ToJson(deck, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }

    private void SettingIsChosen()
    {
        for (int i = 0; i < cards.cardItems.Length; i++)
        {
            for (int j = 0; j < deck.myDecks.Count; j++)
            {
                if (cards.cardItems[i].name == deck.myDecks[j])
                {
                    SetIsChosen(i, true);
                }
            }
        }
    }

    private void SettingMyDeck()
    {
        for(int i = 0; i < deck.myDecks.Count; i++)
        {
            for(int j = 0; j < cards.cardItems.Length; j++)
            {
                if(deck.myDecks[i] == cards.cardItems[j].name)
                {
                    myDeck.Add(cards.cardItems[j]);
                }
            }
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
        cardContents.IsClicked();
    }

    public void AddMyDeck(Carditem carditem)
    {
        myDeck.Add(carditem);
        deck.myDecks.Add(carditem.name);
    }

    public void RemoveMyDeck(Carditem carditem)
    {
        myDeck.Remove(carditem);
        deck.myDecks.Remove(carditem.name);
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

    public void SaveButton()
    {
        Debug.Log(myDeck.Count);

        if (myDeck.Count != 10) return;

        Debug.Log("세이브");
        SaveToJson();
    }

    public string GetDeck(int index)
    {
        return deck.myDecks[index];
    }

    public List<string> GetDeck()
    {
        return deck.myDecks;
    }

    public CardItemSO GetCardItemSO()
    {
        return cards;
    }
}

[System.Serializable]
public class Deck
{
    public List<string> myDecks = new List<string>();
}