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
    

    [SerializeField] Transform cardSpawnPoint;

    // Later these var name retouch
    [SerializeField] Transform whiteCardLeft; // White Player Card Area Left 
    [SerializeField] Transform whiteCardRight; // White Player Card Area Right
    [SerializeField] Transform blackCardRight;  // Black Player Card Area Right
    [SerializeField] Transform blackCardLeft; // Black Player Card Area Left

    [SerializeField] List<Card> whiteCards;
    [SerializeField] List<Card> blackCards;

    [SerializeField] ECardState eCardState; // now Game system state

    [SerializeField] private Image cardInfoImage;
    private Text cardNameText;
    private Text cardInfoText;
    private Image cardImage;
    private Text godNameText;
    private Text cardTypeText;
    private Text targetText;
    private Text turnText;

    #endregion

    #region Var List
    // Later these var name retouch
    private List<Carditem> whiteCardBuffer = new List<Carditem>(); //white Card Buffer
    private List<Carditem> blackCardBuffer = new List<Carditem>(); //Balck Card Buffer

    public List<Carditem> usedCards = new List<Carditem>();

    private Vector3 localPosition = Vector3.zero;
    private ChessBase chessPiece;
    private Card selectCard;
    [SerializeField] private GameObject cards;
    private CardbufferManager cardbufferManager;
    private int IDs = 0;

    public bool isMyCardDrag { get; private set; }
    public bool onMyCardArea { get; private set; }
    private bool isShow = true;
    private bool isTargeting = false;
    private bool isClick = false;

    private float pointDownTime = 0f;
    [SerializeField] private bool isMine = false;
    [SerializeField] private bool isBreak = false;
    [SerializeField] private bool isStop = false;
    [SerializeField] private bool isUse = false;

    int myPutCount = 0;
    [SerializeField] private bool isUsed;

    enum ECardState { Nothing, CanMouseDrag }
    #endregion

    #region System
    void Start()
    {
        GetCardInfoComponent();
    }

    private void Update()
    {
        if (TurnManager.Instance.isLoading) return;

        if (isClick)
        {
            if (pointDownTime > 0.75f)
            {
                isMyCardDrag = true;
            }

            pointDownTime += Time.deltaTime;

        }
        if (isMyCardDrag)
        {
            CardDrag(); // Dragging
        }
        if (!TurnManager.Instance.isLoading)
            DetectCardArea(); // When Dragging Check CardArea in out

        SetECardState(); //enum event check and set
    }
    #endregion

    #region System Manage
    private void GetCardInfoComponent()
    {
        cardNameText = cardInfoImage.transform.GetChild(0).GetComponent<Text>();
        cardInfoText = cardInfoImage.transform.GetChild(1).GetComponent<Text>();
        cardImage = cardInfoImage.transform.GetChild(2).GetComponent<Image>();
        godNameText = cardInfoImage.transform.GetChild(3).GetComponent<Text>();
        cardTypeText = cardInfoImage.transform.GetChild(4).GetComponent<Text>();
        targetText = cardInfoImage.transform.GetChild(5).GetComponent<Text>();
        turnText = cardInfoImage.transform.GetChild(6).GetComponent<Text>();
    }
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

                if (CheckCardname("여행자")) // Only Pawn Targeting
                {
                    if (CheckPawn(hit.collider.name))
                    {
                        SpawnTargetPicker(isTargeting, isMine);
                        selectCard.CardEnable(isTargeting);
                        break;
                    }

                    else
                    {
                        isStop = true;
                        selectCard.CardEnable(false);
                        return;
                    }
                }

                else if (CheckCardname("제물")) // Pawn not Targeting
                {
                    if (CheckPawn(hit.collider.name))
                    {
                        isStop = true;
                        selectCard.CardEnable(false);
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
                selectCard.CardEnable(false);
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
            if (CheckCardname("여행자,에로스의 사랑,질서,달빛,제물,아테나의 방패,대쉬,동귀어진,후진,부활,전쟁광")) //Can not be used for other ChessPiece
            {
                selectCard.CardEnable(false);
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
        selectCard.CardEnable(isShow);
    }

    public void ShowInfo() // When Card MouseDown, Show CardInfo
    {
        if (selectCard == null) return;

        cardInfoImage.gameObject.SetActive(true);
        cardInfoText.text = selectCard.carditem.info;
        cardNameText.text = selectCard.carditem.name;
        cardImage.sprite = selectCard.carditem.sprite;
        godNameText.text = selectCard.carditem.god;
        cardTypeText.text = selectCard.carditem.cardType;
        targetText.text = selectCard.carditem.target;
        turnText.text = selectCard.carditem.turn;
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
            cardArea.transform.position = new Vector2(1.82f, -3.85f);
        }
        else
        {
            cardArea.transform.position = new Vector2(-1.82f, 3.85f);
        }
    }


    void DetectCardArea() // CardArea in out Check
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("CardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    private bool CheckCardname(string name) // Check if a specific card and a select card are the same
    {
        string[] names = name.Split(',');
        if (selectCard == null) return false;

        return Array.Exists(names, x => x == selectCard.carditem.name);
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
    public List<Card> GetTargetCards()
    {
        return ComparisonPlayer("white") ? whiteCards : blackCards;
    }

    public bool ComparisonPlayer(string player)
    {
        if (GameManager.Inst.GetPlayer() == player)
        {
            return true;
        }
        return false;
    }

    public Card GetSelectCard()
    {
        return selectCard;
    }
    public void SetSelectCard(Card card)
    {
        selectCard = card;
    }
    public void SetSelectCardNull()
    {
        selectCard = null;
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
        var targetCardBuffer = ComparisonPlayer("white") ? whiteCardBuffer : blackCardBuffer;
        var deck = NetworkManager.Inst.LoadDataFromJson<User>();
        if (deck == null)
        {
            for (int i = 0; i < 10; i++)
            {
                whiteCardBuffer.Add(cardItemSO.cardItems[i]);
            }
        }

        else
        {
            for (int i = 0; i < 10; i++)
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
        SetUpCardBuffer();

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.05f);
            AddCard(ComparisonPlayer("white"));
        }

        yield return new WaitForSeconds(2f);
        cds = FindObjectsOfType<Card>();
        targetCards = ComparisonPlayer("white") ? blackCards : whiteCards;

        for (int i = 0; i < cds.Length; i++)
        {
            if (cds[i].GetCarditem().player == GameManager.Inst.GetPlayer())
            {
                continue;
            }
            targetCards.Add(cds[i]);
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
            StartCoroutine(DontShowCards(whiteCards, true));
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
            StartCoroutine(DontShowCards(blackCards, true));
        }
    }

    public void NotAmolang()
    {
        if (selectCard != null)
        {
            DestroyCard(selectCard);
            isUse = true;
        }
    }

    public Card GetCard(Carditem carditem)
    {
        if (carditem == null) return null;
        var targetCards = carditem.ID < 200 ? whiteCards : blackCards;

        return targetCards.Find(x => x.GetID() == carditem.ID);
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
        isSame = Array.Exists(usedCards.ToArray(), x => x.name == card.carditem.name);
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

    public void ShowCardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();
        List<Card> targetCards = GetTargetCards();
        if (isShow)
        {
            isShow = false;
            originCardPRSs = ShowCards(targetCards.Count, Vector3.one * 1.9f);
            StartCoroutine(SetPosition(originCardPRSs, targetCards));
            SetOriginOrder(true);
        }
        else
        {
            isShow = true;

            StartCoroutine(DontShowCards(targetCards));
        }

    }
    //public void CardAlignment(bool isMine)
    //{
    //    if (onMyCardArea) return;
    //    List<PRS> originCardPRSs = new List<PRS>();
    //    float zPosition = 0f;


    //    if (TurnManager.Instance.CheckPlayer("white"))
    //    {
    //        if (isMine)
    //        {
    //            originCardPRSs = RoundAlignment(whiteCardLeft, whiteCardRight, whiteCards.Count, 0.5f, Vector3.one * 1.9f);
    //        }
    //        else
    //        {
    //            originCardPRSs = RoundAlignment(blackCardRight, blackCardLeft, blackCards.Count, -0.5f, Vector3.one * 1.9f);
    //        }
    //    }


    //    else
    //    {
    //        if (isMine)
    //        {
    //            originCardPRSs = RoundAlignment(blackCardRight, blackCardLeft, blackCards.Count, -0.5f, Vector3.one * 1.9f);

    //        }

    //        else
    //        {
    //            originCardPRSs = RoundAlignment(whiteCardLeft, whiteCardRight, whiteCards.Count, 0.5f, Vector3.one * 1.9f);

    //        }
    //    }

    //    SetZPosition(isMine, originCardPRSs, zPosition);

    //    SetOriginOrder(isMine);
    //}

    private IEnumerator SetPosition(List<PRS> originCardPRSs, List<Card> targetCards)
    {
        Debug.Log(targetCards.Count);
        Debug.Log(originCardPRSs.Count);
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];
            targetCard.gameObject.SetActive(true);
            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, false, 0.3f);
            yield return new WaitForSeconds(0.05f);
        }
    }
    #region
    //private List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCnt, float height, Vector3 scale) // practical card sorting (Align the cards using the equation of a circle)
    //{
    //    float[] objLerps = new float[objCnt];
    //    List<PRS> results = new List<PRS>(objCnt);

    //    switch (objCnt)
    //    {
    //        case 1: objLerps = new float[] { 0.5f }; break;
    //        case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
    //        case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
    //        default:
    //            float interval = 1f / (objCnt - 1);
    //            for (int i = 0; i < objCnt; i++)
    //                objLerps[i] = interval * i;
    //            break;
    //    }

    //    for (int i = 0; i < objCnt; i++)
    //    {
    //        var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
    //        var targetRot = Utils.QI;
    //        if (objCnt >= 4)
    //        {
    //            float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
    //            curve = height >= 0 ? curve : -curve;
    //            targetPos.y += curve;
    //            targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
    //        }
    //        results.Add(new PRS(targetPos, targetRot, scale));
    //    }
    //    return results;

    //}
    //// Code you don't necessarily need to understand

    //private void SetZPosition(bool isMine, List<PRS> originCardPRSs, float zPosition)
    //{
    //    int cnt;
    //    if (TurnManager.Instance.CheckPlayer("white"))
    //    {
    //        if (isMine)
    //        {
    //            cnt = whiteCards.Count;

    //            for (int i = 0; i < cnt; i++)
    //            {
    //                var targetCard = whiteCards[i];
    //                targetCard.originPRS = originCardPRSs[i];
    //                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
    //                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

    //                zPosition++;
    //            }
    //            return;
    //        }

    //        else
    //        {
    //            cnt = blackCards.Count;

    //            zPosition = 10;
    //            for (int i = 0; i < cnt; i++)
    //            {
    //                var targetCard = blackCards[i];
    //                targetCard.originPRS = originCardPRSs[i];
    //                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
    //                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

    //                zPosition--;
    //            }
    //            return;
    //        }

    //    }
    //    else
    //    {
    //        if (isMine)
    //        {
    //            cnt = blackCards.Count;

    //            zPosition = 10;
    //            for (int i = 0; i < cnt; i++)
    //            {
    //                var targetCard = blackCards[i];
    //                targetCard.originPRS = originCardPRSs[i];
    //                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
    //                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

    //                zPosition--;
    //            }
    //            return;
    //        }


    //        else
    //        {
    //            cnt = whiteCards.Count;

    //            for (int i = 0; i < cnt; i++)
    //            {
    //                var targetCard = whiteCards[i];
    //                targetCard.originPRS = originCardPRSs[i];
    //                targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
    //                targetCard.MoveTransform(targetCard.originPRS, true, true, 0.7f);

    //                zPosition++;
    //            }
    //            return;
    //        }
    //    }
    //}
    #endregion
    private bool TryPutCard(bool isMine, bool isUsed) // Executed when using a card
    {
        if (isStop) return false;
        if (isUse) return false;
        if (isMine && myPutCount >= 1)
            return false;

        if (!isMine && blackCards.Count <= 0)
            return false;

        Card card = selectCard;
        var targetCards = GetTargetCards();
        if (isUsed)
        {
            isTargeting = false;

            SkillManager.Inst.SpawnSkillPrefab(card, chessPiece);
            if (isBreak)
            {
                isBreak = false;
                return false;
            }
            if (CheckCardname("에로스의 사랑,수면,죽음의 땅,파도"))
            {
                card.SetActive(false);
                return true;
            }
            DestroyCard(card);

            isUse = true;

            if (isMine)
            {
                selectCard = null;
                targetPicker.SetActive(false);
            }
            StartCoroutine(DontShowCards(targetCards));
            return true;
        }


        return false;
    }

    private void EnlargeCard(bool isEnlarge, Card card) // Card enlarged when clicked
    {
        if (isEnlarge)
        {
            if (TurnManager.Instance.CheckPlayer("white"))
            {
                Vector3 enlargePos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y, -10f);
                card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.5f), false, false);
            }
            else
            {
                Vector3 enlargePos = new Vector3(card.originPRS.pos.x, card.originPRS.pos.y, -10f);
                Quaternion rot = Quaternion.Euler(0f, 0f, 180f);
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
        card.SetPlayer(GameManager.Inst.GetPlayer());
        SetIds(card);
        card.isFront = isMine;
        var targetCards = GetTargetCards();
        targetCards.Add(card);

        SetOriginOrder(true);
        StartCoroutine(DontShowCards(targetCards));
    }

    public void RemoveCard(int rand, List<Card> targetCards)
    {
        DestroyCard(targetCards[rand]);
    }
    #endregion

    #region Card Control

    public void CardMouseDown(Card card)
    {
        if (eCardState != ECardState.CanMouseDrag || eCardState == ECardState.Nothing)
            return;
        if (isUse) return;

        isClick = true;
        cardInfoImage.gameObject.SetActive(false);
        GameManager.Inst.DestroyMovePlates();
        SkillManager.Inst.SetUsingCard(false);
        SkillManager.Inst.CheckSkillCancel("에로스의 사랑,수면,죽음의 땅,파도");
        selectCard = card;
        EnlargeCard(true, card);
        //cardInfo.SetActive(true);
        StartCoroutine(DontShowCards(GetTargetCards()));

        //ShowInfo();
    }

    public void CardMouseUp(Card card)
    {
        if (isUse) return;
        selectCard.ReloadCard();
        if (pointDownTime < 0.75f)
        {
            var targetRot = ComparisonPlayer("white") ? Utils.QI : Quaternion.Euler(0f, 0f, 180f);
            StartCoroutine(DontShowCards(GetTargetCards()));
            cardInfoImage.gameObject.SetActive(true);
            ShowInfo();
            isMyCardDrag = false;
            isClick = false;
            pointDownTime = 0f;
            card.MoveTransform(new PRS(new Vector2(1.7f, 0.7f), targetRot, Vector3.one * 1.9f), true, false, 0.5f);
            return;
        }

        isMyCardDrag = false;
        isClick = false;
        pointDownTime = 0f;
        isUsed = isTargeting;
        targetPicker.SetActive(false);
        if (eCardState != ECardState.CanMouseDrag || eCardState == ECardState.Nothing)
            return;
        EnlargeCard(false, card);
        if (CheckCardname("죽음의 땅,시간왜곡,바카스,정의구현,망령") && !onMyCardArea)
        {
            isUsed = true;
        }
        if (!TryPutCard(true, isUsed))
        {
            selectCard.CardEnable(false);
            selectCard = null;
            StartCoroutine(DontShowCards(GetTargetCards()));
        }
    }

    private void CardDrag()
    {
        selectCard.SelectingCard();
        TargetingChessPiece();

        if(CheckCardname("죽음의 땅,시간왜곡,바카스,정의구현,망령"))
        {
            selectCard.CardEnable(true);
        }

        if (TurnManager.Instance.CheckPlayer("white"))
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false, false);
        }
        else
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Quaternion.Euler(0f, 0f, 180f), selectCard.originPRS.scale), false, false);
        }

        cardInfoImage.gameObject.SetActive(false);
    }

    public List<PRS> ShowCards(int objCnt, Vector3 scale)
    {
        float[] objYpos = new float[0];
        float[] objLerps = new float[objCnt];
        float interval = 1f / 3;
        List<PRS> results = new List<PRS>(objCnt);
        Vector3 targetPos;
        Quaternion targetRot;
        if (ComparisonPlayer("white"))
        {
            switch (Mathf.CeilToInt((float)objCnt / 4))
            {
                case 1: objYpos = new float[1] { 1.85f }; break;
                case 2: objYpos = new float[2] { 0.16f, 1.85f }; break;
                case 3: objYpos = new float[3] { -1.63f, 0.16f, 1.85f }; break;
            }
            targetRot = Utils.QI;
        }
        else
        {
            switch (Mathf.CeilToInt((float)objCnt / 4))
            {
                case 1: objYpos = new float[1] { -1.63f }; break;
                case 2: objYpos = new float[2] { 0.16f, -1.63f }; break;
                case 3: objYpos = new float[3] { 1.85f, 0.16f, -1.63f }; break;
            }
            targetRot = Quaternion.Euler(0f, 0f, 180f);

        }

        for (int i = objYpos.Length - 1; i >= 0; i--)
        {

            if (objCnt == 0) break;

            for (int j = 0, k = objCnt > 4 ? 4 : objCnt; j < k; j++)
            {
                if (objCnt == 0) break;
                objCnt--;
                targetPos = Vector3.Lerp(new Vector3(-2.1f, objYpos[i], -3f), new Vector3(2.1f, objYpos[i], -3f), interval * j);
                results.Add(new PRS(targetPos, targetRot, scale));
            }
        }
        return results;
    }

    public IEnumerator DontShowCards(List<Card> targetCards, bool isSend = false)
    {
        Vector2 targetPos;
        Quaternion targetRot;
        if (ComparisonPlayer("white"))
        {
            targetPos = new Vector2(2f, -4f);
            targetRot = Utils.QI;
        }

        else
        {
            targetPos = new Vector2(-2f, 4f);
            targetRot = Quaternion.Euler(0f, 0f, 180f);
        }

        for (int i = 0; i < targetCards.Count; i++)
        {
            if (targetCards[i] == selectCard) continue;
            targetCards[i].MoveTransform(new PRS(targetPos, targetRot, Vector3.zero), true, isSend, 0.3f);
            yield return new WaitForSeconds(0.05f);
        }
        isShow = true;
    }

    #endregion
}