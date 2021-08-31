using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Card : MonoBehaviourPunCallbacks
{

    #region SerializeField Var
    Sprite cardFornt;
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite enable;
    [SerializeField] Sprite unable;
    #endregion

    #region Var List
    public SpriteRenderer spriteRenderer { get; private set; }
    public SpriteRenderer cardIcon { get; private set; }
    private SpriteRenderer cardEnable;
    public Collider2D col { get; private set; }
    private PhotonView pv;


    public bool isFront = false;
    public bool isSelected;

    public Carditem carditem = new Carditem();
    public PRS originPRS;

    #endregion

    #region System
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        pv = GetComponent<PhotonView>();
        cardIcon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        cardEnable = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        cardIcon.enabled = false;
    }

    void OnMouseDown()
    {
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
        }
    }

    public void SelectingCard()
    {
        spriteRenderer.enabled = false;
        cardIcon.enabled = true;
        cardEnable.enabled = true;
    }

    public void ReloadCard()
    {
        spriteRenderer.enabled = true;
        cardIcon.enabled = false;
        cardEnable.enabled = false;
    }
    public bool CheckClickCard()
    {
        if (enabled == false) return true;
        if (GameManager.Inst.IsGameOver()) return true;
        if (!TurnManager.Instance.GetCurrentPlayerTF()) return true;
        if (GameManager.Inst.GetPlayer() != carditem.player) return true;


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
            cardFornt = carditem.sprite;
            spriteRenderer.sprite = cardFornt;
            //cardIcon.sprite = carditem.icon;
            gameObject.name = carditem.name;
        }
        else
        {
            cardFornt = carditem.sprite;
            spriteRenderer.sprite = cardBack;
            //cardIcon.sprite = carditem.icon;
            gameObject.name = carditem.name;
        }
    }

    public void SetActive(bool isActive)
    {
        spriteRenderer.enabled = isActive;
        col.enabled = isActive;
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

    public void CardEnable(bool isEnable)
    {
        if (isEnable)
        {
            cardEnable.sprite = enable;
        }

        else
        {
            cardEnable.sprite = unable;
        }
    }
    #endregion

}
