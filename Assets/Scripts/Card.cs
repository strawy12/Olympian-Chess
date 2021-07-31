using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] Sprite cardFornt;
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite cardDefault;

    public SpriteRenderer cardPrame;
    public SpriteRenderer card;

    SpriteRenderer spriteRenderer = null;


    public bool isFront = false;

    public Carditem carditem;
    public PRS originPRS;
    public bool isSelected;

    private void Start()
    {
        if (enabled == false)
            return;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetUp(Carditem carditem, bool isFront)
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

    private void Update()
    {
        if (enabled == false)
            return;
        if (CardManager.Inst.isMyCardDrag && !CardManager.Inst.onMyCardArea) return;

        if (isFront)
            cardPrame.sprite = cardFornt;
        else
            cardPrame.sprite = cardBack;
        //if (Input.GetMouseButtonDown(0))
        //{
        //    cardClick = CheckClick();
        //    if (!cardClick)
        //        CardManager.Inst.CardMouseUp(this);
        //    else
        //        Debug.Log("냠냠");
        //}
    }
    private void OnMouseOver()
    {
        if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (enabled == false)
            return;
        if (TurnManager.Inst.isLoading) return;
        CardManager.Inst.CardMouseOver(this);
    }
    void OnMouseDown()
    {
        if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (enabled == false)
            return;
        if (TurnManager.Inst.isLoading) return;
        if (isSelected) return;
        isSelected = true;
        if (isFront)
            CardManager.Inst.CardMouseDown(this);
        
    }
    private void OnMouseUpAsButton()
    {
        if (enabled == false)
            return;
        if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (SkillManager.Inst.CheckSkillList("제물", SkillManager.Inst.GetCurrentPlayer(true)))
        {
            //카드 제거하고 otherCards 리스트에서 해당 카드 제거
            //내 카드라면 선택되지 않게
            CardManager.Inst.CardClick(this);
        }
    }

    void OnMouseUp()
    {
        if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        if (enabled == false)
            return;
        if (TurnManager.Inst.isLoading) return;
        isSelected = false;
        if (isFront)
            CardManager.Inst.CardMouseUp(this);
        //if(!CardManager.Inst.isMine)
        //{
        //    Destroy(gameObject);
        //    CardManager.Inst.CardAlignment(false);
        //}

    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweemTime = 0)
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
        if(isMine)
            cardPrame.sprite = cardFornt;
        else
            cardPrame.sprite = emptySprite;
    }
}
