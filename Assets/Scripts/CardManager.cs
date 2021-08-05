using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
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

    [SerializeField] CardItemSO cardItemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardRight;
    [SerializeField] Transform otherCardLeft;
    public List<Card> myCards;
    public List<Card> otherCards;
    [SerializeField] Sprite cardFornt;
    [SerializeField] Sprite cardBack;
    [SerializeField] ECardState eCardState;
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] GameObject targetPicker;
    [SerializeField] GameObject cardArea;

    [SerializeField] Text infoText;
    [SerializeField] Text nameText;
    [SerializeField] Text godText;
    [SerializeField] Image godImage;
    [SerializeField] GameObject cardInfo;


    public List<Carditem> myCardBuffer;
    public List<Carditem> otherCardBuffer;
    public List<Carditem> usedCards;
    private Vector3 localPosition = Vector3.zero;
    Chessman chessPiece;
    public Card selectCard;
    [SerializeField] private GameObject cards;
    private CardbufferManager cardbufferManager;
    public bool isMyCardDrag { get; private set; }
    public bool onMyCardArea { get; private set; }
    public bool isTargeting = false;
    public bool isMine = false;
    private bool isBreak = false;
    private bool isStop = false;
    private bool isUse = false;
    int myPutCount;

    enum ECardState { Nothing, CanMouseDrag }
    void Awake()
    {
        var objs = FindObjectsOfType<CardManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        cardPrefab.GetComponent<Card>().enabled = true;
        cardPrefab.GetComponent<DraftCard>().enabled = false;
    }
    public Carditem PopCard(bool isMine)
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
    private void Start()
    {
        cardbufferManager = FindObjectOfType<CardbufferManager>();
        SetUpCardBuffer();
    }


    private void Update()
    {
        if (TurnManager.Instance.isLoading) return;
        if (isMyCardDrag)
            CardDrag();
        if (!TurnManager.Instance.isLoading)
            DetectCardArea();

        SetECardState();
    }
    void SetUpCardBuffer()
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
        }

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

    public void AddCard(bool isMine)
    {
        if (myCardBuffer.Count == 0 && otherCardBuffer.Count == 0) return;

        if (isMine)
        {
            if (myCardBuffer.Count == 0) return;
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
            if (otherCardBuffer.Count == 0) return;
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
    public Chessman GetChessPiece()
    {
        return chessPiece;
    }
    public void SetisBreak(bool isBreak)
    {
        this.isBreak = isBreak;
    }
    void SetOriginOrder(bool isMine)
    {
        if (isMine)
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
        else
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
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCnt, float height, Vector3 scale)
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
            if (GameManager.Inst.GetCurrentPlayer() == "white")
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
    public bool TryPutCard(bool isMine, bool isUsed)
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

            Skill sk = SkillManager.Inst.SpawnSkillPrefab(card, chessPiece);
            if (isBreak)
            {
                Destroy(sk);
                return false;
            }
            if (CheckSkillList("파도", GameManager.Inst.GetCurrentPlayer())) return true;
            if (CheckSkillList("수면", GameManager.Inst.GetCurrentPlayer())) return true;
            if (CheckSkillList("에로스의 사랑", GameManager.Inst.GetCurrentPlayer())) return true;

            DestroyCard(card, targetCards);
            isUse = true;

            if (isMine)
            {
                if (selectCard.carditem.name == "전쟁광" || selectCard.carditem.name == "달빛")
                    targetPicker.SetActive(false);
                else
                {
                    selectCard = null;
                    targetPicker.SetActive(false);
                }
            }

            CardAlignment(isMine);
            if (CheckSkillList("제물", GameManager.Inst.GetCurrentPlayer())) return true;
            if (selectCard != null)
                if (selectCard.carditem.name == "전쟁광" || selectCard.carditem.name == "달빛")
                    return true;

            //TurnManager.Inst.EndTurn();
            return true;
        }
        else
        {
            targetCards.ForEach(x => x.GetComponent<Order>().SetMostFrontOrder(false));
            CardAlignment(isMine);
            return false;
        }

    }
    public void ChangeIsUse(bool _isUse)
    {
        isUse = _isUse;
    }
    public void DestroyCard(Card card, List<Card> targetCards)
    {
        if (card == null) return;

        targetCards.Remove(card);
        card.transform.DOKill();
        card.transform.SetParent(GameManager.Inst.pool.transform);
        card.gameObject.SetActive(false);
        CardAlignment(true);
        usedCards.Add(card.carditem);
    }
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
        SkillManager.Inst.SetIsUsingCard(false);
        SkillManager.Inst.CheckSkillCancel();

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
        targetPicker.SetActive(false);
        cardInfo.SetActive(false);
        if (eCardState != ECardState.CanMouseDrag || eCardState == ECardState.Nothing)
            return;
        EnlargeCard(false, card);
        if (!TryPutCard(true, isTargeting))
        {
            selectCard = null;

        }

    }

    public void CardClick(Card card)
    {
        if (CheckCards(card))
        {
            otherCards.Remove(card);
            Destroy(card.gameObject);
            CardAlignment(false);
            //TurnManager.Inst.EndTurn();

            if (CheckSkillList("제물", GameManager.Inst.GetCurrentPlayer()))
            {
                SkillManager.Inst.DeleteSkillList(SkillManager.Inst.GetSkillList("제물", GameManager.Inst.GetCurrentPlayer()));
            }
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
    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("CardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    public void EnlargeCard(bool isEnlarge, Card card)
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
    public bool CheckCard(string name)
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            if (myCards[i].carditem.name == name)
                return true;
        }
        return false;
    }
    void SetECardState()
    {
        if (TurnManager.Instance.isLoading)
            eCardState = ECardState.Nothing;

        else if (TurnManager.Instance.myTurn)
            eCardState = ECardState.CanMouseDrag;


    }
    private void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++) //무브플레이트 모두 살피고 제거
        {
            Destroy(movePlates[i]);
        }
    }

    public void TargetingChessPiece()
    {

        foreach (RaycastHit2D hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {

            if (hit.collider.CompareTag("ChessPiece"))
            {
                chessPiece = hit.collider.gameObject.GetComponent<Chessman>();
                isMine = chessPiece.CheckinsMine();
                isTargeting = true;
                localPosition = hit.collider.transform.position;

                if ((selectCard.carditem.name == "전쟁광" && GameManager.Inst.GetCurrentPlayer() != hit.collider.GetComponent<Chessman>().player) || selectCard.carditem.name == "여행자" || selectCard.carditem.name == "죽음의 땅")
                {
                    if (hit.collider.name == "white_pawn" || hit.collider.name == "black_pawn")
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
                   
                else if (selectCard.carditem.name == "제물")
                {
                    if (hit.collider.name == "white_pawn" || hit.collider.name == "black_pawn")
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
                //SkillManager.Inst.isMine = true;
                SpawnTargetPicker(isTargeting, isMine);
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
            if (selectCard.carditem.name == "음악" || selectCard.carditem.name == "천벌" || selectCard.carditem.name == "수면" || selectCard.carditem.name == "서풍" || selectCard.carditem.name == "대양")
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
            if ( selectCard.carditem.name == "여행자" || selectCard.carditem.name == "에로스의 사랑" || selectCard.carditem.name == "질서" || selectCard.carditem.name == "여행자" || selectCard.carditem.name == "달빛" || selectCard.carditem.name == "제물" || selectCard.carditem.name == "아테나의 방패" || selectCard.carditem.name == "돌진" || selectCard.carditem.name == "길동무")
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

    public void DestroyCards()
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

    private bool CheckCards(Card card)
    {
        for (int i = 0; i < otherCards.Count; i++)
        {
            if (otherCards[i] == card)
                return true;
        }
        return false;
    }
    public void ChangeCard(bool isColor)
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

    public void UpdateCard()
    {
        RotationBoard.ohtercards = otherCards;
        RotationBoard.mycards = myCards;
    }

    public void AddUsedCard(int randomNum)
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

    private bool CheckSkillList(string name, string player)
    {
        if (SkillManager.Inst.CheckSkillList(name, player))
            return true;
        else
            return false;
    }
    private void ShowInfo()
    {
        if (selectCard == null) return;
        infoText.text = string.Format("{0}", selectCard.carditem.info);
        godText.text = string.Format("{0}", selectCard.carditem.god);
        nameText.text = string.Format("{0}", selectCard.carditem.name);
        NameColor(nameText, selectCard.carditem.name);

        godImage.sprite = selectCard.carditem.sprite;
    }
    private void NameColor(Text text, string name)
    {
        switch (name)
        {
            case "천벌":
                text.color = new Color32(214, 161, 28, 255);
                break;

            case "죽음의 땅":
            case "제물":
                text.color = new Color32(205, 217, 194, 255);
                break;

            case "수중감옥":
            case "파도":
                text.color = new Color32(173, 180, 255, 255);
                break;

            case "수면":
                text.color = new Color32(255, 188, 166, 255);
                break;

            case "달빛":
                text.color = new Color32(236, 245, 247, 255);
                break;

            case "전쟁광":
                text.color = new Color32(230, 78, 109, 255);
                break;

            case "정의구현":
            case "아테나의 방패":
                text.color = new Color32(255, 245, 160, 255);
                break;

            case "질서":
                text.color = new Color32(98, 235, 173, 255);
                break;

            case "바카스":
                text.color = new Color32(30, 50, 230, 255);
                break;

            case "에로스의 사랑":
            case "출산":
            case "음악":
                text.color = new Color32(231, 163, 233, 255);
                break;

            case "길동무":
                text.color = new Color32(165, 148, 209, 255);
                break;

            case "돌진":
                text.color = new Color32(212, 109, 91, 255);
                break;

            case "서풍":
                text.color = new Color32(141, 187, 235, 255);
                break;

            case "여행자":
                text.color = new Color32(214, 184, 155, 255);
                break;

            case "시간왜곡":
                text.color = new Color32(43, 66, 71, 255);
                break;
        }
    }
}