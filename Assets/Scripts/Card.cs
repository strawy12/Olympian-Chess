using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditorInternal.VersionControl;

public class Card : MonoBehaviour
{

    #region SerializeField Var
    [SerializeField] Sprite cardFornt;
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite cardDefault;
    [SerializeField] ECardState eCardState;
    #endregion

    #region Var List
    public SpriteRenderer cardPrame;
    public SpriteRenderer card;
    private SpriteRenderer spriteRenderer = null;

    public bool isFront= false;
    public bool isSelected;

    public Carditem carditem;
    public PRS originPRS;
    enum ECardState { Nothing, CanMouseDrag }

    #endregion

    #region System
    private void Start()
    {
        if (enabled == false) // DraftCard Script overlap Prevention 
            return;

        spriteRenderer = GetComponent<SpriteRenderer>();
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

        SetECardState();
    }

    private void OnMouseOver()
    {
        if (eCardState == ECardState.Nothing)
            return;
        //if (PilSalGi.Inst.GetisUsePilSalGi()) return; // Card cannot be used while using PilSalGi
        if (enabled == false)
            return;
        if (TurnManager.Instance.isLoading) return; // when Turn Loading Card cannot be used
        CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseDown()
    {
        if (eCardState == ECardState.Nothing)
            return;
        //if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (enabled == false)
            return;
        if (TurnManager.Instance.isLoading) return;
        if (isSelected) return; // reselection Prevention 
        isSelected = true;
        if (isFront)
            CardManager.Inst.CardMouseDown(this);

    }

    //private void OnMouseUpAsButton()
    //{
       // if (enabled == false)
         //   return;
       // if (PilSalGi.Inst.GetisUsePilSalGi()) return;
       //if (SkillManager.Inst.CheckSkillList("Á¦¹°", SkillManager.Inst.GetCurrentPlayer(true)))
       // {
       //     CardManager.Inst.CardClick(this);
       // }
   // }

    void OnMouseUp()
    {
        if (eCardState == ECardState.Nothing)
            return;
        // if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (enabled == false)
            return;
        if (TurnManager.Instance.isLoading) return;
        isSelected = false;
        if (isFront)
            CardManager.Inst.CardMouseUp(this);

        //if(!CardManager.Inst.isMine)
        //{
        //    Destroy(gameObject);
        //    CardManager.Inst.CardAlignment(false);
        //}

    }

    void SetECardState() // enum event Setting
    {
        if (TurnManager.Instance.isLoading)
            eCardState = ECardState.Nothing;

        else if (TurnManager.Instance.myTurn)
            eCardState = ECardState.CanMouseDrag;
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
        }
        else
        {
            cardPrame.sprite = cardBack;
            card.sprite = null;
        }
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweemTime = 0) // Card Move
    {
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
    }


    public void ChangePrime(bool isMine)
    {
        if (isMine)
            cardPrame.sprite = cardFornt;
        else
            cardPrame.sprite = emptySprite;
    }
    #endregion

}
