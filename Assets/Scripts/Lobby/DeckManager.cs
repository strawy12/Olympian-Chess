using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
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

    private int card = -1;
    public bool isSelected = false;
    private bool isSave = false;
    private bool isInfo = false;
    [SerializeField]
    private bool[] isChosen = new bool[23];
    private Carditem currentCard;
    public Carditem[] myDeck = new Carditem[10];
    private User user;
    [SerializeField]
    private CardItemSO cards;
    private CardSelect cardSelect;

    [SerializeField]
    private CardContents cardContents;
    [SerializeField]
    private Image message;
    [SerializeField]
    private Image cardInfo;
    private Text messageText;
    private Text cardInfoText;

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
        messageText = message.gameObject.GetComponentInChildren<Text>();
        cardInfoText = cardInfo.gameObject.GetComponentInChildren<Text>();
    }

    private void LoadFromJson()
    {
        string json;

        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            user = JsonUtility.FromJson<User>(json);
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

        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }


    private void SettingIsChosen()
    {
        for (int i = 0; i < cards.cardItems.Length; i++)
        {
            for (int j = 0; j < user.myDecks.Length; j++)
            {
                if (cards.cardItems[i].name == user.myDecks[j])
                {
                    SetIsChosen(i, true);
                }
            }
        }
    }

    private void SettingMyDeck()
    {
        for (int i = 0; i < user.myDecks.Length; i++)
        {
            for (int j = 0; j < cards.cardItems.Length; j++)
            {
                if (user.myDecks[i] == cards.cardItems[j].name)
                {
                    myDeck[i] = cards.cardItems[j];
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

    public void InsertMyDeck(int index, Carditem carditem)
    {
        myDeck[index] = carditem;
        user.myDecks[index] = carditem.name;
    }

    public void RemoveMyDeck(int index)
    {
        myDeck[index] = null;
        user.myDecks[index] = null;
        Debug.Log("삭제");
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
        Debug.Log(myDeck.Length);
        int i;

        for (i = 0; i < myDeck.Length; i++)
        {
            if (myDeck[i].name == "") break;
        }

        if (i == myDeck.Length)
        {
            StartCoroutine(Message("덱이 저장되었습니다!"));
            SaveToJson();
            isSave = true;
        }
        else
        {
            StartCoroutine(Message("선택된 카드가 10장 이하입니다"));
        }
    }

    public string GetDeck(int index)
    {
        return user.myDecks[index];
    }

    public string[] GetDeck()
    {
        return user.myDecks;
    }

    public int GetGold()
    {
        return user.gold;
    }

    public void SetGold(int gold)
    {
        user.gold = gold;
    }

    public CardItemSO GetCardItemSO()
    {
        return cards;
    }

    public Carditem GetMyDeck(int index)
    {
        return myDeck[index];
    }

    public void SetBackground(string bg)
    {
        user.backGround = bg;
    }

    public string GetBackground()
    {
        return user.backGround;
    }

    public User GetUser()
    {
        return user;
    }

    public void SetIsSave(bool isTrue)
    {
        isSave = isTrue;
    }

    IEnumerator Message(string messageText)
    {
        message.transform.DOScale(1, 0.3f).SetEase(Ease.InBounce);
        this.messageText.text = string.Format(messageText);
        yield return new WaitForSeconds(0.7f);
        message.transform.DOScale(0, 0.1f);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    public void SetCardSelect(CardSelect card)
    {
        cardSelect = card;
    }
    public void PointerUp()
    {
        isInfo = false;
        cardInfo.gameObject.SetActive(false);
    }

    public void PointerDown(CardSelect cardSelect)
    {
        isInfo = true;
        StartCoroutine(Info(cardSelect.GetCardItem()));
    }

    IEnumerator Info(Carditem carditem)
    {
        while(true)
        {
            yield return new WaitForSeconds(0.7f);

            if(isInfo)
            {
                cardInfo.gameObject.SetActive(true);
                cardInfoText.text = carditem.info;
                yield break;
            }

            else if(!isInfo)
            {
                yield break;
            }
        }
    }
}

[System.Serializable]
public class User
{
    public int gold = 1000;
    public string backGround;
    public string[] myDecks = new string[10];
    public bool[] myBackground = new bool[4];
}