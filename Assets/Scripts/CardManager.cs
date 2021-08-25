using DG.Tweening;
using Photon.Pun;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviourPunCallbacks
{
    #region SingleTon
    private static CardManager inst;
    public static CardManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<CardManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<CardManager>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }
    #endregion

    #region SerializeField Var
    [SerializeField] CardItemSO cardItemSO;

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject skillPrefab;
    [SerializeField] GameObject targetPicker;
    [SerializeField] GameObject cardArea;
    [SerializeField] GameObject cardInfo;

    [SerializeField] Transform cardSpawnPoint;

    // Later these var name retouch
    [SerializeField] Transform whiteCardLeft; // White Player Card Area Left 
    [SerializeField] Transform whiteCardRight; // White Player Card Area Right
    [SerializeField] Transform blackCardRight;  // Black Player Card Area Right
    [SerializeField] Transform blackCardLeft; // Black Player Card Area Left

    [SerializeField] List<Card> whiteCards;
    [SerializeField] List<Card> blackCards;

    [SerializeField] ECardState eCardState; // now Game system state

    [SerializeField] Text infoText;
    [SerializeField] Text nameText;
    [SerializeField] Text godText;

    [SerializeField] Image godImage;

    #endregion

    #region Var List
    // Later these var name retouch
    private List<Carditem> whiteCardBuffer; //white Card Buffer
    private List<Carditem> blackCardBuffer; //Balck Card Buffer


    public List<Carditem> usedCards = new List<Carditem>();

    private Vector3 localPosition = Vector3.zero;
    private ChessBase chessPiece;
    private Card selectCard;
    [SerializeField]
    private User deck;
    [SerializeField] private GameObject cards;
    private CardbufferManager cardbufferManager;
    private int IDs = 0;

    public bool isMyCardDrag { get; private set; }
    public bool onMyCardArea { get; private set; }
    private bool isTargeting = false;
    [SerializeField] private bool isMine = false;
    [SerializeField] private bool isBreak = false;
    [SerializeField] private bool isStop = false;
    [SerializeField] private bool isUse = false;

    int myPutCount = 0;
    [SerializeField] private bool isUsed;

    enum ECardState { Nothing, CanMouseDrag }
    #endregion

    #region System
    void Awake()
    {
        cardPrefab.GetComponent<Card>().enabled = true;
    }

    private void Start()
    {
        LoadFromJson();
        SetUpCardBuffer();
    }

    private void Update()
    {
        if (TurnManager.Instance.isLoading) return;
        if (isMyCardDrag)
            CardDrag(); // Dragging
        if (!TurnManager.Instance.isLoading)
            DetectCardArea(); // When Dragging Check CardArea in out

        SetECardState(); //enum event check and set
    }
    #endregion

    #region System Manage

    public void TargetingChessPiece()
    {
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {

            if (hit.collider.CompareTag("ChessPiece"))
            {
                chessPiece = hit.collider.gameObject.GetComponent<ChessBase>();
                isMine = chessPiece.CheckIsMine();
                isTargeting = true;
                localPosition = hit.collider.transform.position;

                if (CheckCardname("여행자") || (CheckCardname("전쟁광") && !TurnManager.Instance.CheckPlayer(hit.collider.name))) // Only Pawn Targeting
                {
                    if (CheckPawn(hit.collider.name))
                    {
                        SpawnTargetPicker(isTargeting, isMine);
                        break;
                    }
                    else
                    {
                        isStop = true;
                        return;
                    }
                }

                else if (CheckCardname("제물")) // Pawn not Targeting
                {
                    if (CheckPawn(hit.collider.name))
                    {
                        isStop = true;
                        return;
                    }
                    else
                    {
                        SpawnTargetPicker(isTargeting, isMine);
                        break;
                    }
                }
                SpawnTargetPicker(isTargeting, isMine); // Default Targeting
                break;
            }
            else
            {
                isTargeting = false;
                SpawnTargetPicker(isTargeting, isMine);
            }
        }
    }

    public void SpawnTargetPicker(bool isShow, bool isMine)
    {

        targetPicker.transform.position = localPosition;
        if (isMine)
        {
            if (CheckCardname("음악,천벌,수면,서풍")) // Can not be used for my ChessPiece
            {
                isStop = true;
                return;
            }
            else
            {
                isStop = false;
            }
            targetPicker.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(29, 219, 22, 255));
        }
        else
        {
            if (CheckCardname("여행자,에로스의 사랑,질서,달빛,제물,아테나의 방패,대쉬,동귀어진,후진,부활")) //Can not be used for other ChessPiece

            {
                isStop = true;
                return;
            }
            else
            {
                isStop = false;
            }
            targetPicker.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
        }
        targetPicker.SetActive(isShow);
    }

    private void ShowInfo() // When Card MouseDown, Show CardInfo
    {
        if (selectCard == null) return;

        infoText.text = string.Format("{0}", selectCard.carditem.info);
        godText.text = string.Format("{0}", selectCard.carditem.god);
        nameText.text = string.Format("{0}", selectCard.carditem.name);
        nameText.color = selectCard.carditem.color;

        godImage.sprite = selectCard.carditem.sprite;
    }

    private void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    void SetECardState() // enum event Setting
    {
        if (TurnManager.Instance.isLoading)
            eCardState = ECardState.Nothing;

        else if (TurnManager.Instance.GetCurrentPlayerTF())
            eCardState = ECardState.CanMouseDrag;
    }

    #endregion

    #region System Check

    public void ChangeCardArea(bool isWihte)
    {
        if (isWihte)
        {
            cardArea.transform.position = new Vector2(0f, -3.9f);
        }
        else
        {
            cardArea.transform.position = new Vector2(0f, 3.9f);
        }
    }


    void DetectCardArea() // CardArea in out Check
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("CardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    public bool CheckCard(string name, bool isWhite) // same card in CardBuffer Check
    {
        List<Card> targetCards;

        targetCards = isWhite ? whiteCards : blackCards;

        for (int i = 0; i < targetCards.Count; i++)
        {
            if (targetCards[i].carditem.name == name)
                return true;
        }
        return false;
    }
    private bool CheckCardname(string name) // Check if a specific card and a select card are the same
    {
        string[] names = name.Split(',');
        if (selectCard == null) return false;
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i] == selectCard.carditem.name)
            {
                return true;
            }
        }
        return false;
    }



    private bool CheckPawn(string name) // argument value == pawn Check
    {
        if (name == "white_pawn" || name == "black_pawn")
        {
            return true;
        }
        return false;
    }

    #endregion

    #region Script Access 

    public ChessBase GetChessPiece()
    {
        return chessPiece;
    }

    public void SetisBreak(bool isBreak)
    {
        this.isBreak = isBreak;
    }

    public List<Card> GetWhiteCards()
    {
        return whiteCards;
    }
    public List<Carditem> GetUsedCards()
    {
        return usedCards;
    }

    public List<Card> GetBlackCards()
    {
        return blackCards;
    }

    public Card GetSelectCard()
    {
        return selectCard;
    }
    public void SetSelectCard(Card card)
    {
        selectCard = card;
    }

    public void ChangeIsUse(bool _isUse)// retouch SetIsUse 
    {
        isUse = _isUse;
    }

    #endregion

    #region CardSetting
    public Carditem PopCard(bool isMine) // First CardBuffer's Card draw
    {

        if (isMine)
        {
            Carditem carditem = null;
            carditem = whiteCardBuffer[0];
            whiteCardBuffer.RemoveAt(0);
            return carditem;
        }
        else
        {

            Carditem carditem = null;
            carditem = blackCardBuffer[0];
            blackCardBuffer.RemoveAt(0);
            return carditem;
        }

    }

    void SetUpCardBuffer() // Card Buffer value Get, Set
    {
        var targetCardBuffer = NetworkManager.Inst.GetPlayer() == "white" ? whiteCardBuffer : blackCardBuffer;
        for (int i = 0; i < deck.myDecks.Length; i++)
        {
            for (int j = 0; j < cardItemSO.cardItems.Length; j++)
            {
                if (deck.myDecks[i] == cardItemSO.cardItems[j].name)
                {
                    targetCardBuffer.Add(cardItemSO.cardItems[j]);
                }
            }
        }
    }



public void SetIds(Card card)
{
    if (card.GetCarditem().player == "white")
    {
        card.SetID(IDs + 100);
    }
    else if (card.GetCarditem().player == "black")
    {
        card.SetID(IDs + 200);
    }

    IDs++;
}
public void CardShare()
{
    StartCoroutine(CardShareCo());
}

private IEnumerator CardShareCo()
{
    Card[] cds;
    List<Card> targetCards;
    for (int i = 0; i < 10; i++)
    {
        yield return new WaitForSeconds(0.05f);
        AddCard(NetworkManager.Inst.GetPlayer() == "white");
    }

    yield return new WaitForSeconds(2f);
    cds = FindObjectsOfType<Card>();
    targetCards = NetworkManager.Inst.GetPlayer() == "white" ? blackCards : whiteCards;

    for (int i = 0; i < cds.Length; i++)
    {
        if (cds[i].GetCarditem().player == NetworkManager.Inst.GetPlayer())
        {
            continue;
        }
        targetCards.Add(cds[i]);
    }
}
    private void LoadFromJson()
    {
        string json;
        string SAVE_PATH = Application.dataPath + "/Save";
        string SAVE_FILENAME = "/SaveFile.txt";

        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            deck = JsonUtility.FromJson<User>(json);
        }
    }

public void AddCard(bool isMine) // CardBuffer value => GameObjectCard conversion
{
    if (whiteCardBuffer.Count == 0 && blackCardBuffer.Count == 0) return;

    if (isMine)
    {
        var cardObject = NetworkManager.Inst.SpawnObject(cardPrefab);
        cardObject.transform.SetParent(cards.transform);
        var card = cardObject.GetComponent<Card>();
        card.SetUp(PopCard(isMine), isMine);
        card.SetPlayer("white");
        SetIds(card);
        card.isFront = isMine;
        whiteCards.Add(card);
        SetOriginOrder(isMine);
        CardAlignment(isMine);
    }
    else
    {
        var cardObject = NetworkManager.Inst.SpawnObject(cardPrefab);
        cardObject.transform.SetParent(cards.transform);
        var card = cardObject.GetComponent<Card>();
        card.SetUp(PopCard(false), true);
        card.SetPlayer("black");
        SetIds(card);
        card.isFront = true;
        blackCards.Add(card);
        SetOriginOrder(isMine);
        CardAlignment(isMine);
    }
}


public void NotAmolang()
{
    if (selectCard != null)
    {
        DestroyCard(selectCard);
        CardAlignment(true);
        isUse = true;
    }
}
public Card GetCard(Carditem carditem)
{

    if (carditem == null) return null;
    var targetCards = carditem.ID < 200 ? whiteCards : blackCards;

    for (int i = 0; i < targetCards.Count; i++)
    {

        if (carditem.ID == targetCards[i].GetID())
        {

            return targetCards[i];
        }
    }
    return null;
}
public void DestroyCard(Card card) // Using Card Destroy
{
    if (card == null) return;

    string jsonData = NetworkManager.Inst.SaveDataToJson(card.GetCarditem(), false);
    photonView.RPC("DestroyCard", RpcTarget.AllBuffered, jsonData);
}

[PunRPC]
public void DestroyCard(string jsonData)
{
    bool isSame = false;
    Carditem carditem = NetworkManager.Inst.LoadDataFromJson<Carditem>(jsonData);
    var targetCards = carditem.ID < 200 ? whiteCards : blackCards;
    Card card = GetCard(carditem);
    if (card == null) return;
    targetCards.Remove(card);
    card.transform.DOKill();
    card.transform.SetParent(GameManager.Inst.pool.transform);
    card.gameObject.SetActive(false);
    if (!TurnManager.Instance.GetCurrentPlayerTF())
    {
        CardAlignment(false);
    }
    for (int i = 0; i < usedCards.Count; i++)
    {
        if (card.carditem.name == usedCards[i].name)
            isSame = true;
    }

    if (!isSame)
        usedCards.Add(card.carditem);
}
public void DestroyCards() // All Cards Destroy
{
    for (int i = 0; i < whiteCards.Count; i++)
    {
        Destroy(whiteCards[i].gameObject);
    }
    for (int i = 0; i < blackCards.Count; i++)
    {
        Destroy(blackCards[i].gameObject);
    }
}



#endregion

#region CardObject Setting
void SetOriginOrder(bool isMine) // OrderInLayer Setting
{
    if (isMine) // my Cards
    {
        int cnt = whiteCards.Count;
        if (TurnManager.Instance.CheckPlayer("white"))
        {
            for (int i = 0; i < cnt; i++)
            {
                var targetCard = whiteCards[i];
                targetCard?.GetComponent<Order>().SetOriginOrder(i);
            }
        }
        else
        {
            int j = 0;
            for (int i = cnt - 1; i >= 0; i--)
            {
                var targetCard = whiteCards[i];
                targetCard?.GetComponent<Order>().SetOriginOrder(j);
                j++;
            }
        }
    }
    else // other Cards
    {
        int cnt = blackCards.Count;
        if (TurnManager.Instance.CheckPlayer("black"))
        {
            for (int i = 0; i < cnt; i++)
            {
                var targetCard = blackCards[i];
                targetCard?.GetComponent<Order>().SetOriginOrder(i);
            }
        }
        else
        {
            int j = 0;
            for (int i = cnt - 1; i >= 0; i--)
            {
                var targetCard = blackCards[i];
                targetCard?.GetComponent<Order>().SetOriginOrder(j);
                j++;
            }
        }
    }
}

public void CardAlignment(bool isMine)
{
    if (onMyCardArea) return;
    List<PRS> originCardPRSs = new List<PRS>();
    float zPosition = 0f;


    if (TurnManager.Instance.CheckPlayer("white"))
    {
        if (isMine)
        {
            originCardPRSs = RoundAlignment(whiteCardLeft, whiteCardRight, whiteCards.Count, 0.5f, Vector3.one * 1.9f);
        }
        else
        {
            originCardPRSs = RoundAlignment(blackCardLeft, blackCardRight, blackCards.Count, -0.5f, Vector3.one * 1.9f);
        }
    }


    else
    {
        if (isMine)
        {
            originCardPRSs = RoundAlignment(blackCardLeft, blackCardRight, blackCards.Count, -0.5f, Vector3.one * 1.9f);

        }

        else
        {
            originCardPRSs = RoundAlignment(whiteCardLeft, whiteCardRight, whiteCards.Count, 0.5f, Vector3.one * 1.9f);

        }
    }

    SetZPosition(isMine, originCardPRSs, zPosition);

    SetOriginOrder(isMine);
}

private List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCnt, float height, Vector3 scale) // practical card sorting (Align the cards using the equation of a circle)
{
    float[] objLerps = new float[objCnt];
    List<PRS> results = new List<PRS>(objCnt);

    switch (objCnt)
    {
        case 1: objLerps = new float[] { 0.5f }; break;
        case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
        case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
        default:
            float interval = 1f / (objCnt - 1);
            for (int i = 0; i < objCnt; i++)
                objLerps[i] = interval * i;
            break;
    }

    for (int i = 0; i < objCnt; i++)
    {
        var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
        var targetRot = Utils.QI;
        if (objCnt >= 4)
        {
            float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
            curve = height >= 0 ? curve : -curve;
            targetPos.y += curve;
            targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
        }
        results.Add(new PRS(targetPos, targetRot, scale));
    }
    return results;

}
// Code you don't necessarily need to understand

private void SetZPosition(bool isMine, List<PRS> originCardPRSs, float zPosition)
{
    int cnt;
    if (TurnManager.Instance.CheckPlayer("white"))
    {
        if (isMine)
        {
            cnt = whiteCards.Count;

            for (int i = 0; i < cnt; i++)
            {
                var targetCard = whiteCards[i];
                targetCard.originPRS = originCardPRSs[i];
                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

                zPosition++;
            }
            return;
        }

        else
        {
            cnt = blackCards.Count;

            zPosition = 10;
            for (int i = 0; i < cnt; i++)
            {
                var targetCard = blackCards[i];
                targetCard.originPRS = originCardPRSs[i];
                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

                zPosition--;
            }
            return;
        }

    }
    else
    {
        if (isMine)
        {
            cnt = blackCards.Count;

            zPosition = 10;
            for (int i = 0; i < cnt; i++)
            {
                var targetCard = blackCards[i];
                targetCard.originPRS = originCardPRSs[i];
                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

                zPosition--;
            }
            return;
        }


        else
        {
            cnt = whiteCards.Count;

            for (int i = 0; i < cnt; i++)
            {
                var targetCard = whiteCards[i];
                targetCard.originPRS = originCardPRSs[i];
                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

                zPosition++;
            }
            return;
        }
    }
}

private bool TryPutCard(bool isMine, bool isUsed) // Executed when using a card
{
    if (isStop) return false;
    if (isUse) return false;
    if (isMine && myPutCount >= 1)
        return false;

    if (!isMine && blackCards.Count <= 0)
        return false;

    Card card = selectCard;
    if (isUsed)
    {

        isTargeting = false;

        SkillManager.Inst.SpawnSkillPrefab(card, chessPiece);
        if (isBreak)
        {
            isBreak = false;
            return false;
        }
        if (CheckCardname("에로스의 사랑,수면,죽음의 땅,파도")) return true;

        DestroyCard(card);

        isUse = true;

        if (isMine)
        {
            selectCard = null;
            targetPicker.SetActive(false);
        }

        CardAlignment(TurnManager.Instance.GetCurrentPlayerTF());
        return true;
    }
    CardAlignment(TurnManager.Instance.GetCurrentPlayerTF());
    return false;
}

private void EnlargeCard(bool isEnlarge, Card card) // Card enlarged when clicked
{
    if (isEnlarge)
    {
        if (TurnManager.Instance.CheckPlayer("white"))
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -3.5f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.5f), false, false);
        }
        else
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, 3.5f, -10f);
            Quaternion rot = Quaternion.Euler(180f, 180f, 0f);
            card.MoveTransform(new PRS(enlargePos, rot, Vector3.one * 2.5f), false, false);
        }
    }
    else
        card.MoveTransform(card.originPRS, false, false);

    card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
}

public void AddUsedCard(int randomNum) // sacrificial function, Among the used cards, randomly add them back to the CardBuffer
{
    if (usedCards.Count == 0) return;

    var cardObject = NetworkManager.Inst.SpawnObject(cardPrefab);
    cardObject.transform.SetParent(cards.transform);
    var card = cardObject.GetComponent<Card>();
    card.SetUp(usedCards[randomNum], true);
    card.SetPlayer(NetworkManager.Inst.GetPlayer());
    SetIds(card);
    card.isFront = isMine;
    var targetCards = NetworkManager.Inst.GetPlayer() == "white" ? whiteCards : blackCards;
    targetCards.Add(card);

    SetOriginOrder(true);
    CardAlignment(true);
}

public void RemoveCard(int rand, List<Card> targetCards)
{
    DestroyCard(targetCards[rand]);
}
#endregion

#region Card Control

public void CardMouseOver(Card card)
{
    if (eCardState == ECardState.Nothing)
        return;
    if (!card.isSelected) return;
    if (selectCard == null) return;
    if (card.carditem.name != selectCard.carditem.name) return;
    if (isMyCardDrag && !onMyCardArea)
        card.cardPrame.sprite = null;
}

public void CardMouseDown(Card card)
{
    if (eCardState != ECardState.CanMouseDrag || eCardState == ECardState.Nothing)
        return;
    if (isUse) return;

    DestroyMovePlates();
    SkillManager.Inst.SetUsingCard(false);
    SkillManager.Inst.CheckSkillCancel("에로스의 사랑,수면,죽음의 땅,파도");
    isMyCardDrag = true;
    selectCard = card;
    EnlargeCard(true, card);
    cardInfo.SetActive(true);
    ShowInfo();
}

public void CardMouseUp(Card card)
{
    if (isUse) return;
    isMyCardDrag = false;
    isUsed = isTargeting;
    targetPicker.SetActive(false);
    cardInfo.SetActive(false);
    if (eCardState != ECardState.CanMouseDrag || eCardState == ECardState.Nothing)
        return;
    EnlargeCard(false, card);
    if (CheckCardname("죽음의 땅,시간왜곡,바카스,정의구현") && !onMyCardArea)
    {
        isUsed = true;
    }
    if (!TryPutCard(true, isUsed))
    {
        selectCard = null;
    }
}

private void CardDrag()
{
    TargetingChessPiece();
    if (!onMyCardArea)
    {
        if (TurnManager.Instance.CheckPlayer("white"))
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false, false);
        }
        else
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Quaternion.Euler(0f, 0f, 180f), selectCard.originPRS.scale), false, false);
        }
        cardInfo.SetActive(false);
    }
}

    #endregion
}