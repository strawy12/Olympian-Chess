using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
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
    [SerializeField] Transform myCardLeft; // White Player Card Area Left 
    [SerializeField] Transform myCardRight; // White Player Card Area Right
    [SerializeField] Transform otherCardRight;  // Black Player Card Area Right
    [SerializeField] Transform otherCardLeft; // Black Player Card Area Left

    [SerializeField] List<Card> myCards;
    [SerializeField] List<Card> otherCards;

    [SerializeField] ECardState eCardState; // now Game system state

    [SerializeField] Text infoText;
    [SerializeField] Text nameText;
    [SerializeField] Text godText;

    [SerializeField] Image godImage;

    #endregion

    #region Var List
    // Later these var name retouch
    private List<Carditem> myCardBuffer; //white Card Buffer
    private List<Carditem> otherCardBuffer; //Balck Card Buffer

    public List<Carditem> usedCards = new List<Carditem>();

    private Vector3 localPosition = Vector3.zero;
    private Chessman chessPiece;
    private Card selectCard;
    [SerializeField] private GameObject cards;
    private CardbufferManager cardbufferManager;

    public bool isMyCardDrag { get; private set; }
    public bool onMyCardArea { get; private set; }
    private bool isTargeting = false;
    [SerializeField] private bool isMine = false;
    [SerializeField] private bool isBreak = false;
    [SerializeField] private bool isStop = false;
    [SerializeField] private bool isUse = false;

    int myPutCount;
    [SerializeField] private bool isUsed;

    enum ECardState { Nothing, CanMouseDrag }
    #endregion

    #region System
    void Awake()
    {
        cardPrefab.GetComponent<Card>().enabled = true;
        cardPrefab.GetComponent<DraftCard>().enabled = false;
    }

    private void Start()
    {
        cardbufferManager = FindObjectOfType<CardbufferManager>();
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
                chessPiece = hit.collider.gameObject.GetComponent<Chessman>();
                isMine = chessPiece.CheckIsMine();
                isTargeting = true;
                localPosition = hit.collider.transform.position;

                if (CheckCardname("여행자") || (CheckCardname("전쟁광") && !CheckPlayer(hit.collider.name))) // Only Pawn Targeting
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
            if (CheckCardname("음악,천벌,수면,서풍,수중감옥")) // Can not be used for my ChessPiece
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
            if (CheckCardname("여행자,에로스의 사랑,질서,달빛,제물,아테나의 방패,돌진,길동무")) //Can not be used for other ChessPiece

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

        else if (TurnManager.Instance.myTurn)
            eCardState = ECardState.CanMouseDrag;
    }

    #endregion

    #region System Check

    void DetectCardArea() // CardArea in out Check
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("CardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    public bool CheckCard(string name, bool isMine) // same card in CardBuffer Check
    {
        List<Card> targetCards;

        targetCards = isMine ? myCards : otherCards;

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


    private bool CheckPlayer(string name) // CurrentPlayer Check
    {
        if (GameManager.Inst.GetCurrentPlayer() == name)
        {
            return true;
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

    public Chessman GetChessPiece()
    {
        return chessPiece;
    }

    public void SetisBreak(bool isBreak)
    {
        this.isBreak = isBreak;
    }

    public List<Card> GetMyCards()
    {
        return myCards;
    }
    public List<Carditem> GetUsedCards()
    {
        return usedCards;
    }

    public List<Card> GetOtherCards()
    {
        return otherCards;
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
            carditem = myCardBuffer[0];
            myCardBuffer.RemoveAt(0);
            return carditem;
        }
        else
        {
            Carditem carditem = null;
            carditem = otherCardBuffer[0];
            otherCardBuffer.RemoveAt(0);
            return carditem;
        }

    }

    void SetUpCardBuffer() // Card Buffer value Get, Set
    {
        if (cardbufferManager == null)
        {
            myCardBuffer = new List<Carditem>();
            for (int i = 0; i < cardItemSO.cardItems.Length; i++)
            {
                Carditem carditem = cardItemSO.cardItems[i];
                myCardBuffer.Add(carditem);
            }
        }
        else
        {
            myCardBuffer = cardbufferManager.GetMyCardBuffer();
        } // It does come from Draft Scene, cardbufferManager == null
          // So, if cardbufferManager == null, my.other CardBuffer is default value
          // CardBufferManager have value from Draft Scene's selectCards

        if (cardbufferManager == null)
        {
            otherCardBuffer = new List<Carditem>();
            for (int i = 0; i < cardItemSO.cardItems.Length; i++)
            {
                Carditem carditem = cardItemSO.cardItems[i];
                otherCardBuffer.Add(carditem);
            }
        }
        else
        {
            otherCardBuffer = cardbufferManager.GetOhterCardBuffer();
        }
    }

    public void AddCard(bool isMine) // CardBuffer value => GameObjectCard conversion
    {
        if (myCardBuffer.Count == 0 && otherCardBuffer.Count == 0) return;

        if (isMine)
        {
            var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
            cardObject.transform.SetParent(cards.transform);
            var card = cardObject.GetComponent<Card>();
            card.SetUp(PopCard(isMine), isMine);
            card.isFront = isMine;
            myCards.Add(card);
            SetOriginOrder(isMine);
            CardAlignment(isMine);
        }
        else
        {
            var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
            cardObject.transform.SetParent(cards.transform);
            var card = cardObject.GetComponent<Card>();
            card.SetUp(PopCard(isMine), isMine);
            card.isFront = isMine;
            otherCards.Add(card);
            SetOriginOrder(isMine);
            CardAlignment(isMine);
        }
    }

    public void NotAmolang()
    {
        if (selectCard != null)
        {
            var targetCards = GetMyCards();
            DestroyCard(selectCard, targetCards);
            isUse = true;
        }
    }

    public void DestroyCard(Card card, List<Card> targetCards) // Using Card Destroy
    {
        bool isSame = false;

        if (card == null) return;

        targetCards.Remove(card);
        card.transform.DOKill();
        card.transform.SetParent(GameManager.Inst.pool.transform);
        card.gameObject.SetActive(false);
        CardAlignment(true);

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
        for (int i = 0; i < myCards.Count; i++)
        {
            Destroy(myCards[i].gameObject);
        }
        for (int i = 0; i < otherCards.Count; i++)
        {
            Destroy(otherCards[i].gameObject);
        }
    }

    public void ChangeCard(bool isColor) // After EndTurn, Change the values ​​of myCards and otherCards
    {
        List<Card> temp = new List<Card>();

        temp = myCards;
        myCards = otherCards;
        otherCards = temp;
        for (int i = 0; i < otherCards.Count; i++)
        {
            otherCards[i].card.sprite = null;
            otherCards[i].ChangePrime(false);
            otherCards[i].isFront = false;
        }
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].card.sprite = myCards[i].carditem.sprite;
            myCards[i].ChangePrime(true);
            myCards[i].isFront = true;
        }

        if (isColor)
        {
            cardArea.transform.position = new Vector3(0f, 0f, 3f);
        }
        else
        {
            cardArea.transform.position = new Vector3(0f, 7.7f, 3f);
        }
        SetOriginOrder(true);
    }

    public void UpdateCard() // After EndTurn, Error Check and CardBuffer Update
    {
        RotationBoard.ohtercards = otherCards;
        RotationBoard.mycards = myCards;
    }

    #endregion

    #region CardObject Setting
    void SetOriginOrder(bool isMine) // OrderInLayer Setting
    {
        if (isMine) // my Cards
        {
            int cnt = myCards.Count;
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                for (int i = 0; i < cnt; i++)
                {
                    var targetCard = myCards[i];
                    targetCard?.GetComponent<Order>().SetOriginOrder(i);
                }
            }
            else
            {
                int j = 0;
                for (int i = cnt - 1; i >= 0; i--)
                {
                    var targetCard = myCards[i];
                    targetCard?.GetComponent<Order>().SetOriginOrder(j);
                    j++;
                }
            }
        }
        else // other Cards
        {
            int cnt = otherCards.Count;
            if (GameManager.Inst.GetCurrentPlayer() == "black")
            {
                for (int i = 0; i < cnt; i++)
                {
                    var targetCard = otherCards[i];
                    targetCard?.GetComponent<Order>().SetOriginOrder(i);
                }
            }
            else
            {
                int j = 0;
                for (int i = cnt - 1; i >= 0; i--)
                {
                    var targetCard = otherCards[i];
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

        if (isMine)
        {
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 1.9f);

            }
            else
            {
                originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, myCards.Count, -0.5f, Vector3.one * 1.9f);
            }
        }


        else
        {
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 1.9f);
            }

            else
            {
                originCardPRSs = RoundAlignment(myCardLeft, myCardRight, otherCards.Count, 0.5f, Vector3.one * 1.9f);
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
        if (isMine)
        {
            cnt = myCards.Count;
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                for (int i = 0; i < cnt; i++)
                {
                    var targetCard = myCards[i];
                    targetCard.originPRS = originCardPRSs[i];
                    targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);

                    zPosition++;
                }
                return;
            }
            else
            {
                zPosition = 10;
                for (int i = 0; i < cnt; i++)
                {
                    var targetCard = myCards[i];
                    targetCard.originPRS = originCardPRSs[i];
                    targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);

                    zPosition--;
                }
                return;
            }

        }
        else
        {
            cnt = otherCards.Count;
            if (GameManager.Inst.GetCurrentPlayer() != "white")
            {
                for (int i = 0; i < cnt; i++)
                {
                    var targetCard = otherCards[i];
                    targetCard.originPRS = originCardPRSs[i];
                    targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);

                    zPosition++;
                }
                return;
            }
            else
            {
                zPosition = 10;
                for (int i = 0; i < cnt; i++)
                {
                    var targetCard = otherCards[i];
                    targetCard.originPRS = originCardPRSs[i];
                    targetCard.originPRS.pos.z = originCardPRSs[i].pos.z - zPosition;
                    targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);

                    zPosition--;
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

        if (!isMine && otherCards.Count <= 0)
            return false;

        Card card = isMine ? selectCard : otherCards[Random.Range(0, otherCards.Count)];
        var targetCards = isMine ? myCards : otherCards;
        if (isUsed)
        {

            isTargeting = false;

            SkillManager.Inst.SpawnSkillPrefab(card, chessPiece);
            if (isBreak)
            {
                isBreak = false;
                return false;
            }
            if(CheckCardname("에로스의 사랑,수면,죽음의 땅,파도")) return true;
            DestroyCard(card, targetCards);
            isUse = true;

            if (isMine)
            {
                selectCard = null;
                targetPicker.SetActive(false);
            }

            CardAlignment(isMine);

            return true;
        }
        return false;
    }

    private void EnlargeCard(bool isEnlarge, Card card) // Card enlarged when clicked
    {
        if (isEnlarge)
        {
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -3.5f, -10f);
                card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.5f), false);
            }
            else
            {
                Vector3 enlargePos = new Vector3(card.originPRS.pos.x, 3.5f, -10f);
                Quaternion rot = Quaternion.Euler(180f, 180f, 0f);
                card.MoveTransform(new PRS(enlargePos, rot, Vector3.one * 2.5f), false);
            }
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    public void AddUsedCard(int randomNum) // sacrificial function, Among the used cards, randomly add them back to the CardBuffer
    {
        if (usedCards.Count == 0) return;

        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        cardObject.transform.SetParent(cards.transform);
        var card = cardObject.GetComponent<Card>();
        card.SetUp(usedCards[randomNum], true);
        myCards.Add(card);

        SetOriginOrder(true);
        CardAlignment(true);
    }

    public void RemoveCard(int rand)
    {
        Destroy(otherCards[rand].gameObject);
        otherCards.RemoveAt(rand);
        CardAlignment(!isMine);
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
        if (CheckCardname("죽음의 땅,시간왜곡,바카스") && !onMyCardArea)
        {
            isUsed = true;
        }
        if(!TryPutCard(true, isUsed))
        {
            selectCard = null;
        }
    }

    public void CardClick(Card card)
    {
        if (CheckCard(card.carditem.name, false))
        {
            otherCards.Remove(card);
            Destroy(card.gameObject);
            CardAlignment(false);
        }
        else
        {
            return;
        }
    }

    private void CardDrag()
    {
        TargetingChessPiece();
        if (!onMyCardArea)
        {
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
            }
            else
            {
                selectCard.MoveTransform(new PRS(Utils.MousePos, Quaternion.Euler(0f, 0f, 180f), selectCard.originPRS.scale), false);
            }
            cardInfo.SetActive(false);
        }
    }

    #endregion
}