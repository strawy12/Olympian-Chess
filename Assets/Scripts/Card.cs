using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Card : MonoBehaviourPunCallbacks
{

    #region SerializeField Var
    [SerializeField] Sprite cardFornt;
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite cardDefault;
    #endregion

    #region Var List
    public SpriteRenderer cardPrame;
    public SpriteRenderer card;
    private SpriteRenderer spriteRenderer = null;
    private PhotonView pv;

    
    public bool isFront = false;
    public bool isSelected;

    public Carditem carditem = new Carditem();
    public PRS originPRS;

    #endregion

    #region System
    private void Awake()
    {
        if (enabled == false) // DraftCard Script overlap Prevention 
            return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();
        //pv.enabled = false;
    }

    private void Update()
    {
        if (enabled == false)
            return;
        if (CardManager.Inst.isMyCardDrag) return;

        if (isFront)
            cardPrame.sprite = cardFornt;
        else
            cardPrame.sprite = cardBack;
    }

    private void OnMouseOver()
    {
        //if (PilSalGi.Inst.GetisUsePilSalGi()) return; // Card cannot be used while using PilSalGi
        if (enabled == false)
            return;
        if (TurnManager.Instance.isLoading) return; // when Turn Loading Card cannot be used
        CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseDown()
    {
        //if (PilSalGi.Inst.GetisUsePilSalGi()) return;

        if (CheckClickCard()) return;
        if (isSelected) return;
        // reselection Prevention 
        isSelected = true;
        if (isFront)
        {
            CardManager.Inst.CardMouseDown(this);
        }

    }

    void OnMouseUp()
    {
        // if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (CheckClickCard()) return;

        isSelected = false;
        if (isFront)
        {
            CardManager.Inst.CardMouseUp(this);
            cardPrame.sprite = cardFornt;
        }
        else
            cardPrame.sprite = cardBack;
    }
    public bool CheckClickCard()
    {
        if (enabled == false) return true;
        if (GameManager.Inst.IsGameOver()) return true;
        if (!TurnManager.Instance.GetCurrentPlayerTF()) return true;
        if (NetworkManager.Inst.GetPlayer() != carditem.player) return true;


        return false;
    }

    private void SendCardData()
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(carditem, true);
        photonView.RPC("SetCardData", RpcTarget.OthersBuffered, jsonData);
    }

    [PunRPC]
    public void SetCardData(string jsonData)
    {
        Carditem carditem = NetworkManager.Inst.LoadDataFromJson<Carditem>(jsonData);
        this.carditem = carditem;
    }
    #endregion

    #region Card Setting

    public void SetUp(Carditem carditem, bool isFront) // Card SetUp
    {
        this.carditem = carditem;
        this.isFront = isFront;

        if (this.isFront)
        {
            cardPrame.sprite = cardFornt;
            card.sprite = carditem.sprite;
            gameObject.name = carditem.name;
        }
        else
        {
            cardPrame.sprite = cardBack;
            card.sprite = null;
            gameObject.name = carditem.name;
        }
    }

    public void MoveTransform(PRS prs, bool useDotween, bool isSend, float dotweemTime = 0)
    {
        string prs_ToJson = NetworkManager.Inst.SaveDataToJson(prs, false);
        if (isSend)
        {
            photonView.RPC("SetMoveTransform", RpcTarget.All, prs_ToJson, useDotween, dotweemTime);
        }
        else
        {
            SetMoveTransform(prs_ToJson, useDotween, dotweemTime);
        }

    }

    [PunRPC]
    public void SetMoveTransform(string prs_ToJson, bool useDotween, float dotweemTime = 0) // Card Move
    {
        PRS prs = NetworkManager.Inst.LoadDataFromJson<PRS>(prs_ToJson);
        originPRS = prs;
        //pv.enabled = true;
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweemTime);
            transform.DORotateQuaternion(prs.rot, dotweemTime);
            transform.DOScale(prs.scale, dotweemTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
        //pv.enabled = false;

    }


    public void ChangePrime(bool isMine)
    {
        if (isMine)
            cardPrame.sprite = cardFornt;
        else
            cardPrame.sprite = emptySprite;
    }
    public void SetID(int ID)
    {
        carditem.ID = ID;
        SendCardData();
    }

    public void SetPlayer(string player)
    {
        carditem.player = player;
        SendCardData();
    }
    public int GetID()
    {
        return carditem.ID;
    }

    public Carditem GetCarditem()
    {
        return carditem;
    }
    #endregion

}
