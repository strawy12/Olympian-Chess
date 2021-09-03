using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using Photon.Pun;

public class DeckManager : MonoBehaviourPunCallbacks
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

    private int card = -1;
    public bool isSelected = false;
    private bool isInfo = false;
    [SerializeField]
    private bool[] isChosen = new bool[23];
    private Carditem currentCard;
    public Carditem[] myDeck = new Carditem[10];
    private User user;
    [SerializeField]
    private CardItemSO cards;
    [SerializeField]
    private SuperSkillSO supers;
    [SerializeField]
    private Coroutine coroutineInfo;

    [SerializeField] private Image cardInfoImage;
    private Text cardNameText;
    private Text cardInfoText;
    private Image cardImage;
    private Text godNameText;
    private Text cardTypeText;
    private Text targetText;
    private Text turnText;

    [SerializeField]
    private CardContents cardContents;
    [SerializeField]
    private Image message;
    private Text messageText;
    private bool isDrag = false;

    //여기서 isChosen[카드 번호]가 true면 그 카드를 선택된거임
    private void Awake()
    {
        user = NetworkManager.Inst.LoadDataFromJson<User>();
        if (user == null)
        {
            user = new User(10000, 0, new string[10], new bool[6], 0.5f, 0.5f, supers.superSkills);
        }

        SettingIsChosen();
    }

    private void Start()
    {
        ChangeCard();
        SettingMyDeck();
        GetCardInfoComponent();
    }

    private void GetCardInfoComponent()
    {
        messageText = message.gameObject.transform.GetChild(0).GetComponent<Text>();
        cardNameText = cardInfoImage.transform.GetChild(0).GetComponent<Text>();
        cardInfoText = cardInfoImage.transform.GetChild(1).GetComponent<Text>();
        cardImage = cardInfoImage.transform.GetChild(2).GetComponent<Image>();
        godNameText = cardInfoImage.transform.GetChild(3).GetComponent<Text>();
        cardTypeText = cardInfoImage.transform.GetChild(4).GetComponent<Text>();
        targetText = cardInfoImage.transform.GetChild(5).GetComponent<Text>();
        turnText = cardInfoImage.transform.GetChild(6).GetComponent<Text>();
    }

    private void SettingIsChosen()
    {
        if (user == null || user.myDecks == null) return;
        for (int i = 0; i < cards.cardItems.Length; i++)
        {
            for (int j = 0; j < user.myDecks.Length; j++)
            {
                if (user.myDecks[j] == null)
                    continue;

                if (cards.cardItems[i].name == user.myDecks[j])
                {
                    SetIsChosen(i, true);
                }
            }
        }
    }

    private void SettingMyDeck()
    {
        if (user == null || user.myDecks == null) return;
        for (int i = 0; i < user.myDecks.Length; i++)
        {
            if (user.myDecks[i] == "")
                continue;
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
        int i;

        for (i = 0; i < myDeck.Length; i++)
        {
            if (myDeck[i].name == "") break;
        }

        if (i == myDeck.Length)
        {
            StartCoroutine(Message("덱이 저장되었습니다!"));
            NetworkManager.Inst.SaveDataToJson(user, true);
        }
        else
        {
            StartCoroutine(Message("선택된 카드가 10장 이하입니다"));
        }
    }

    public void ClickMessage(string str)
    {
        StartCoroutine(Message(str));
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

    public void SetBackground(int bg)
    {
        user.backGround = bg;
    }

    public void SetSoundVolume(float bgm, float eff)
    {
        user.bgmVolume = bgm;
        user.effectVolume = eff;
    }

    public int GetBackground()
    {
        return user.backGround;
    }

    public User GetUser()
    {
        return user;
    }

    public IEnumerator Message(string messageText)
    {
        Debug.Log(messageText);
        message.transform.DOScale(1, 0.3f).SetEase(Ease.InBounce);
        this.messageText.text = string.Format(messageText);
        yield return new WaitForSeconds(1.25f);
        message.transform.DOScale(0, 0.1f);
    }

    private void OnApplicationQuit()
    {
        user.player = "";
        NetworkManager.Inst.SaveDataToJson(user, true);
    }

    public void PointerUp()
    {
        isInfo = false;
        isDrag = false;
        StopCoroutine(coroutineInfo);
        coroutineInfo = null;
    }

    public void Drag()
    {
        isDrag = true;
        cardInfoImage.gameObject.SetActive(false);
    }

    public void PointerDown(Carditem carditem)
    {
        isInfo = true;
        coroutineInfo = StartCoroutine(Info(carditem));
    }

    IEnumerator Info(Carditem carditem)
    {
        yield return new WaitForSeconds(0.7f);
        Debug.Log("d2d");
        SoundManager.Instance.Button();

        if (isInfo && !isDrag)
        {
            cardInfoImage.gameObject.SetActive(true);
            cardInfoText.text = carditem.detail;
            cardNameText.text = carditem.name;
            cardImage.sprite = carditem.sprite;
            godNameText.text = carditem.god;
            cardTypeText.text = carditem.cardType;
            targetText.text = carditem.target;
            turnText.text = carditem.turn;
        }

        if (!isInfo)
        {
            cardInfoImage.gameObject.SetActive(false);
        }
    }
    public void SetPlayer(int i)
    {
        Debug.Log("실러");

        if (i == 0)
        {
            user.player = "white";
            photonView.RPC("SetPlayer", RpcTarget.OthersBuffered, "black");
        }
        else if (i == 1)
        {
            user.player = "black";
            photonView.RPC("SetPlayer", RpcTarget.OthersBuffered, "white");
        }
        NetworkManager.Inst.SaveDataToJson(user, true);
    }

    [PunRPC]
    private void SetPlayer(string player)
    {
        Debug.Log("실러");
        user.player = player;
        NetworkManager.Inst.SaveDataToJson(user, true);
    }

    public bool IsInfoActive()
    {
        if (cardInfoImage.gameObject.activeSelf)
            return true;
        else
            return false;
    }

    public void InActiveInfo()
    {
        cardInfoImage.gameObject.SetActive(false);
    }
}


[System.Serializable]
public class User
{
    public int gold;
    public int backGround;
    public string player;
    public string[] myDecks;
    public bool[] myBackground;
    public float bgmVolume;
    public float effectVolume;
    public SuperSkillData[] superSkills;

    public User(int gold, int backGround, string[] myDecks, bool[] myBackground, float bgmVolume, float effectVolume, SuperSkillData[] superSkills)
    {
        this.gold = gold;
        this.backGround = backGround;
        this.myDecks = myDecks;
        this.myBackground = myBackground;
        this.bgmVolume = bgmVolume;
        this.effectVolume = effectVolume;
        this.superSkills = superSkills;
    }
}