using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Card : MonoBehaviour
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

    private string player;
    public bool isFront = false;
    public bool isSelected;

    public Carditem carditem;
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
        if (CardManager.Inst.isMyCardDrag && !CardManager.Inst.onMyCardArea) return;

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
            CardManager.Inst.CardMouseDown(this);

    }

    void OnMouseUp()
    {
        // if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (CheckClickCard()) return;

        isSelected = false;
        if (isFront)
            CardManager.Inst.CardMouseUp(this);

    }
    public bool CheckClickCard()
    {
        if (enabled == false) return true;
        if (GameManager.Inst.IsGameOver()) return true;
        if (!TurnManager.Instance.GetCurrentPlayerTF()) return true;
        if (NetworkManager.Inst.GetPlayer() != player) return true;


        return false;
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
    public void MoveTransform(PRS prs, bool useDotween, float dotweemTime = 0) // Card Move
    {
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

    public void SetPlayer(string player)
    {
        this.player = player;
    }
    #endregion

}
