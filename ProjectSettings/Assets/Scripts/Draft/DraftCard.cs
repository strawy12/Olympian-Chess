using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DraftCard : MonoBehaviour
{
    [SerializeField] Sprite cardFront;
    [SerializeField] Sprite cardBack;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite cardDefault;

    [SerializeField] SpriteRenderer card;
    public SpriteRenderer cardPrame;
    SpriteRenderer spriteRenderer = null;

    public PRS originPRS;
    public PRS roundPRS;

    public Carditem carditem;
    public bool isSelected = false;

    private void Start()
    {
        if (enabled == false)
            return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardFront;
    }

    public void SetUp(Carditem carditem)
    {
        this.carditem = carditem;
        card.sprite = carditem.sprite;
    }

    public void CardBack()
    {
        spriteRenderer.sprite = cardBack;
        card.sprite = emptySprite;
    }

    public void CardFront()
    {
        spriteRenderer.sprite = cardDefault;
    }

    private void OnMouseDown()
    {
        if (enabled == false)
            return;
        isSelected = true;
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

    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
