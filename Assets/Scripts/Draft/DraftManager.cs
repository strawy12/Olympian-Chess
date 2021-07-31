using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DraftManager : MonoBehaviour
{
    [SerializeField] CardItemSO cardItemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject black;
    [SerializeField] List<DraftCard> myCards;
    [SerializeField] List<DraftCard> otherCards;

    [SerializeField] List<DraftCard> myDraftCards;
    [SerializeField] List<DraftCard> otherDraftCards;

    DraftCard card;

    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardRight;
    [SerializeField] Transform otherCardLeft;
    [SerializeField] Transform myCardLeftDown;
    [SerializeField] Transform myCardRightDown;
    [SerializeField] Transform otherCardLeftDown;
    [SerializeField] Transform otherCardRightDown;

    [SerializeField] GameObject whiteArea;
    [SerializeField] GameObject blackArea;
    [SerializeField] GameObject leftInfoButton;
    [SerializeField] GameObject leftBtn;
    [SerializeField] GameObject rightInfoButton;
    [SerializeField] GameObject rightBtn;
    [SerializeField] GameObject startBtn;

    [SerializeField] Text leftInfoText;
    [SerializeField] GameObject leftInfoTextobj;
    [SerializeField] Text rightInfoText;
    [SerializeField] GameObject rightInfoTextobj;
    [SerializeField] Text countText;

    [SerializeField] Text leftNameText;
    [SerializeField] Text rightNameText;

    [SerializeField] Text leftGodText;
    [SerializeField] Text rightGodText;

    [SerializeField] Slider timerBar;
    [SerializeField] GameObject timeBarobj;

    private int setTime = 15;
    private float countTime = 0f;
    int rand;

    public List<Carditem> myCardBuffer;
    public List<Carditem> otherCardBuffer;

    public bool isMine = false;
    public bool isFirst = false;
    private bool isBackR = false;
    private bool isBackL = false;

    private IEnumerator countCoroutine;
    private CardbufferManager cardbufferManager = null;
    private void Awake()
    {
        
    }
    private void Start()
    {
        cardPrefab.GetComponent<Card>().enabled = false;
        cardPrefab.GetComponent<DraftCard>().enabled = true;
        cardbufferManager = FindObjectOfType<CardbufferManager>();
        countCoroutine = CountTime();
        SetUpCardBuffer();
        Mix();
        InstantiateCards();
        StartCoroutine(DelayTime());
    }

    private void Update()
    {
        if (setTime < 0)
        {
            rand = Random.Range(0, 11);

            if (rand < 5)
                LeftCard();
            else
                RightCard();
        }
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
            int i = otherCardBuffer.Count - 1;
            carditem = otherCardBuffer[i];
            otherCardBuffer.RemoveAt(i);
            return carditem;
        }
    }

    private void SetUpCardBuffer()
    {
        myCardBuffer = new List<Carditem>();
        otherCardBuffer = new List<Carditem>();

        for (int i = 0; i < cardItemSO.cardItems.Length; i++)
        {
            Carditem carditem = cardItemSO.cardItems[i];
            myCardBuffer.Add(carditem);
        }

        for (int i = 0; i < cardItemSO.cardItems.Length; i++)
        {
            Carditem carditem = cardItemSO.cardItems[i];
            otherCardBuffer.Add(carditem);
        }
    }

    private DraftCard[] CardOneTwo(bool isMine)
    {
        if (isMine)
        {
            var cardOne = otherCards[0];
            var cardTwo = otherCards[1];
            DraftCard[] cards = { cardOne, cardTwo };

            return cards;
        }

        else
        {
            var cardOne = myCards[0];
            var cardTwo = myCards[1];
            DraftCard[] cards = { cardOne, cardTwo };

            return cards;
        }
    }
    private void InstantiateCards()
    {
        for (int i = 0; i < 10; i++)
        {
            isMine = true;

            GameObject cardObject = Instantiate(cardPrefab, Vector2.zero, Utils.QI);
            var card = cardObject.GetComponent<DraftCard>();
            card.SetUp(PopCard(true));
            myCards.Add(card);
            SetOriginOrder(isMine);

            Alignment(true, false);
        }

        for (int i = 0; i < 10; i++)
        {
            isMine = false;

            GameObject cardObject = Instantiate(cardPrefab, Vector2.zero, Utils.QI);
            var card = cardObject.GetComponent<DraftCard>();
            card.SetUp(PopCard(false));
            otherCards.Add(card);
            SetOriginOrder(isMine);

            Alignment(false, false);
        }
    }

    private void DestroyCard()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].gameObject.SetActive(false);
            otherCards[i].gameObject.SetActive(false);
        }
    }

    private void MoveToCenter()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].gameObject.transform.DOMove(Vector2.zero, 0.3f);
            otherCards[i].gameObject.transform.DOMove(Vector2.zero, 0.3f);
        }
    }

    private void Mix()
    {
        List<Carditem> mylist = new List<Carditem>();
        List<Carditem> otherlist = new List<Carditem>();

        int count = myCardBuffer.Count;

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, myCardBuffer.Count);

            mylist.Add(myCardBuffer[rand]);
            myCardBuffer.RemoveAt(rand);
        }

        for (int i = 0; i < count; i++)
        {
            int rand2 = Random.Range(0, otherCardBuffer.Count);

            otherlist.Add(otherCardBuffer[rand2]);
            otherCardBuffer.RemoveAt(rand2);
        }
        for (int i = 0; i < 5; i++)
            mylist.RemoveAt(i);
        for (int i = 0; i < 5; i++)
            otherlist.RemoveAt(i);
        myCardBuffer = mylist;
        otherCardBuffer = otherlist;
        
    }

    void Alignment(bool isMine, bool isRound)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        List<PRS> roundCardPRSs = new List<PRS>();

        if (isRound && isMine)
        {
            roundCardPRSs = RoundAlignment(myCardLeftDown, myCardRightDown, myDraftCards.Count, 0.5f, Vector3.one * 1.9f);
        }

        else if (isRound && !isMine)
        {
            roundCardPRSs = RoundAlignment(otherCardLeftDown, otherCardRightDown, otherDraftCards.Count, -0.5f, Vector3.one * 1.9f);
        }

        if (isMine)
        {
            originCardPRSs = StraightAlignment(myCardLeft, myCardRight, 10, 0.5f, Vector3.one * 1.9f);
        }

        else if (!isMine)
        {
            originCardPRSs = StraightAlignment(otherCardLeft, otherCardRight, 10, -0.5f, Vector3.one * 1.9f);
        }

        if (!isRound)
        {
            var targetCards = isMine ? myCards : otherCards;

            for (int i = 0; i < targetCards.Count; i++)
            {
                var targetCard = targetCards[i];

                if (originCardPRSs[i] == null)
                    continue;

                targetCard.originPRS = originCardPRSs[i];
                targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
            }

        }

        if (isRound)
        {
            var targetCards = isMine ? myDraftCards : otherDraftCards;

            for (int i = 0; i < targetCards.Count; i++)
            {
                var targetCard = targetCards[i];

                if (roundCardPRSs[i] == null)
                    continue;

                targetCard.originPRS = roundCardPRSs[i];
                targetCard.MoveTransform(targetCard.originPRS, true, 0.3f);
            }
        }
    }
    List<PRS> StraightAlignment(Transform leftTr, Transform rightTr, int objCnt, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCnt];
        List<PRS> results = new List<PRS>(objCnt);
        int cnt = 0;

        float rotz = 15f;

        if (!isMine)
            rotz = -165f;

        float interval = 1f / (objCnt - 1);
        for (int i = 0; i < objCnt; i++)
            objLerps[i] = interval * i;

        for (int i = 0; i < objCnt; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.Euler(0f, 0f, rotz);

            if (i % 2 == 0)
                rotz = -15f;
            else
                rotz = 15f;

            if (!isMine)
            {
                if (i % 2 == 0)
                    rotz = -15f;

                else
                    rotz = 15f;

                rotz += -180f;
            }

            if (isMine)
            {
                if (i == 2 || i == 3 || i == 6 || i == 7 )
                    targetPos.y -= 1.6f;
            }

            else
            {
                if (i == 2 || i == 3 || i == 6 || i == 7)
                    targetPos.y += 1.6f;
            }

            float curve = height;
            curve = height >= 0 ? curve : -curve;
            targetPos.y += curve;

            results.Add(new PRS(targetPos, targetRot, scale));
            cnt++;
        }
        return results;
    }

    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(3f);

        black.SetActive(true);
        MoveToCenter();

        yield return new WaitForSeconds(0.3f);

        DestroyCard();
        leftBtn.SetActive(true);
        rightBtn.SetActive(true);
        DraftCard();

        whiteArea.SetActive(false);
        blackArea.SetActive(false);
        leftInfoButton.SetActive(true);
        rightInfoButton.SetActive(true);
    }

    private void DraftCard()
    {
        StartCoroutine(countCoroutine);
        InfoName(leftNameText, "");
        InfoName(rightNameText, "");
        InfoGod(leftGodText, "");
        InfoGod(rightGodText, "");


        timeBarobj.SetActive(true);
        if (myDraftCards.Count > 4)
        {
            myCards[0].gameObject.SetActive(true);
            myCards[0].gameObject.transform.DOScale(3.7f, 0.3f);
            myCards[0].gameObject.transform.DOMove(new Vector2(-1.3f, 0f), 0.3f);
            myCards[0].gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            myCards[1].gameObject.SetActive(true);
            myCards[1].gameObject.transform.DOScale(3.7f, 0.3f);
            myCards[1].gameObject.transform.DOMove(new Vector2(1.3f, 0f), 0.3f);
            myCards[1].gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (otherCards.Count == 0)
            return;

        else
        {
            otherCards[0].gameObject.SetActive(true);
            otherCards[0].gameObject.transform.DOScale(3.7f, 0.3f);
            otherCards[0].gameObject.transform.DOMove(new Vector2(-1.3f, 0f), 0.3f);
            otherCards[0].gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            otherCards[1].gameObject.SetActive(true);
            otherCards[1].gameObject.transform.DOScale(3.7f, 0.3f);
            otherCards[1].gameObject.transform.DOMove(new Vector2(1.3f, 0f), 0.3f);
            otherCards[1].gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void Last()
    {
        startBtn.SetActive(true);
        leftInfoButton.SetActive(false);
        rightInfoButton.SetActive(false);
        timeBarobj.SetActive(false);
        Alignment(true, true);
        Alignment(false, true);
        countText.text = string.Format("");
    }

    public void LeftCard()
    {
        StopCoroutine(countCoroutine);
        countCoroutine = CountTime();

        leftInfoTextobj.SetActive(false);
        rightInfoTextobj.SetActive(false);
        isBackR = false;
        isBackL = false;

        if (myDraftCards.Count > 4)
        {
            var cards = CardOneTwo(false);

            myCards.RemoveAt(0);
            myCards.RemoveAt(0);

            myDraftCards.Add(cards[1]);
            myCardBuffer.Add(cards[1].carditem);
            otherDraftCards.Add(cards[0]);
            otherCardBuffer.Add(cards[0].carditem);

            SetDraftOriginOrder();

            cards[0].CardFront();
            cards[1].CardFront();
            cards[0].SetUp(cards[0].carditem);
            cards[1].SetUp(cards[1].carditem);

            if (myDraftCards.Count > 9)
            {
                Last();
                return;
            }
            DraftCard();
        }

        else
        {
            var cards = CardOneTwo(true);

            otherCards.RemoveAt(0);
            otherCards.RemoveAt(0);

            myDraftCards.Add(cards[0]);
            myCardBuffer.Add(cards[0].carditem);
            otherDraftCards.Add(cards[1]);
            otherCardBuffer.Add(cards[1].carditem);

            SetDraftOriginOrder();

            cards[0].SetUp(cards[0].carditem);
            cards[1].SetUp(cards[1].carditem);
            cards[0].CardFront();
            cards[1].CardFront();

            DraftCard();
        }
        Alignment(true, true);
        Alignment(false, true);
    }

    public void RightCard()
    {
        StopCoroutine(countCoroutine);
        countCoroutine = CountTime();

        isBackR = false;
        isBackL = false;

        leftInfoTextobj.SetActive(false);
        rightInfoTextobj.SetActive(false);

        if (myDraftCards.Count > 4)
        {
            var cards = CardOneTwo(false);

            myCards.RemoveAt(0);
            myCards.RemoveAt(0);

            myDraftCards.Add(cards[0]);
            myCardBuffer.Add(cards[0].carditem);
            otherDraftCards.Add(cards[1]);
            otherCardBuffer.Add(cards[1].carditem);

            SetDraftOriginOrder();

            cards[0].CardFront();
            cards[1].CardFront();
            cards[0].SetUp(cards[0].carditem);
            cards[1].SetUp(cards[1].carditem);

            if (myDraftCards.Count > 9)
            {
                Last();
                return;
            }

            DraftCard();
        }

        else
        {
            var cards = CardOneTwo(true);

            cards[0].CardFront();
            cards[1].CardFront();

            otherCards.RemoveAt(0);
            otherCards.RemoveAt(0);

            myDraftCards.Add(cards[1]);
            myCardBuffer.Add(cards[1].carditem);
            otherDraftCards.Add(cards[0]);
            otherCardBuffer.Add(cards[0].carditem);

            SetDraftOriginOrder();

            cards[0].SetUp(cards[0].carditem);
            cards[1].SetUp(cards[1].carditem);

            DraftCard();
        }

        Alignment(true, true);
        Alignment(false, true);
    }

    public void LeftInfo()
    {
        if (isBackL)
            return;

        isBackL = true;
        if (myDraftCards.Count > 4)
        {
            var cards = CardOneTwo(false);

            cards[0].CardBack();
            Debug.Log(cards[0].carditem.name);

            Info(leftInfoText, cards[0].carditem.info);
            InfoName(leftNameText, cards[0].carditem.name);
            InfoGod(leftGodText, cards[0].carditem.god);
            leftInfoTextobj.SetActive(true);
        }

        else
        {
            var cards = CardOneTwo(true);

            cards[0].CardBack();
            Debug.Log(cards[0].carditem.name);

            Info(leftInfoText, cards[0].carditem.info);
            InfoName(leftNameText, cards[0].carditem.name);
            InfoGod(leftGodText, cards[0].carditem.god);
            leftInfoTextobj.SetActive(true);
        }
    }

    public void RightInfo()
    {
        if (isBackR)
            return;

        isBackR = true;

        if (myDraftCards.Count > 4)
        {
            var cards = CardOneTwo(false);

            cards[1].CardBack();
            Debug.Log(cards[1].carditem.name);

            Info(rightInfoText, cards[1].carditem.info);
            InfoName(rightNameText, cards[1].carditem.name);
            InfoGod(rightGodText, cards[1].carditem.god);
            rightInfoTextobj.SetActive(true);
        }

        else
        {
            var cards = CardOneTwo(true);

            cards[1].CardBack();
            Debug.Log(cards[1].carditem.name);

            Info(rightInfoText, cards[1].carditem.info);
            InfoName(rightNameText, cards[1].carditem.name);
            InfoGod(rightGodText, cards[1].carditem.god);
            rightInfoTextobj.SetActive(true);
        }
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
            var targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
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

    private void Info(Text infoText, string info)
    {
        infoText.text = string.Format("{0}", info);
    }

    private void InfoName(Text text, string name)
    {
        NameColor(text, name);
        text.text = string.Format("{0}", name);
    }

    private void InfoGod(Text text, string god)
    {
        text.text = string.Format("{0}", god);
    }

    private void NameColor(Text text, string name)
    {
        switch(name)
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

    public void OnClickStart()
    {
        cardbufferManager.SetMyCardBuffer(myCardBuffer);
        cardbufferManager.SetOhterCardBuffer(otherCardBuffer);
        SceneManager.LoadScene("Game");
    }

    void SetOriginOrder(bool isMine)
    {
        int cnt = isMine ? myCards.Count : otherCards.Count;
        for (int i = 0; i < cnt; i++)
        {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void SetDraftOriginOrder()
    {
        int cnt = myDraftCards.Count;

        for (int i = 0; i < cnt; i++)
        {
            var targetCard1 = myDraftCards[i];
            var targetCard2 = otherDraftCards[i];

            targetCard1?.GetComponent<Order>().SetOriginOrder(i);
            targetCard2?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    private IEnumerator CountTime()
    {
        

        setTime = 30;

        while (setTime >= 0)
        {
            timerBar.value = setTime;

            countText.text = string.Format("{0}", setTime);
            setTime--;
            yield return new WaitForSecondsRealtime(1f);

            if (setTime < 0)
                countText.text = string.Format("");
        }
    }
}