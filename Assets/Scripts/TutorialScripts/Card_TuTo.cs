using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_TuTo : MonoBehaviour
{
    private bool isMyCardDrag = false;
    private bool targeting = false;
    private float pointDownTime = 0f;
    private Vector3 localPosition;
    [SerializeField]private GameObject targetPicker;
    [SerializeField]private CardButton cardButton;
    [SerializeField]private SpriteRenderer blackPiece;
    [SerializeField]private SpriteRenderer cardIcon;
    [SerializeField] private GameObject showCardBtn;
    [SerializeField] private GameObject showCard;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Debug.Log(TutorialManager.Instance.iscardPush);
        Show();

        if (isMyCardDrag)
        {
            CardDrag();
        }
    }

    private void Show()
    {
        if(TutorialManager.Instance.iscardPush)
        {
            showCardBtn.SetActive(true);
            showCard.SetActive(true);
        }
        else
        {
            showCardBtn.SetActive(false);
            showCard.SetActive(false);
        }
    }
    void OnMouseDown()
    {
        if(TutorialManager.Instance.card)
        {
            isMyCardDrag = true;
            StartCoroutine(cardButton.DontShowCards());

        }
    }

    void OnMouseUp()
    {
        if (TutorialManager.Instance.card)
        {
            TutorialManager.Instance.iscardPush = false;
            isMyCardDrag = false;
            spriteRenderer.enabled = true;
            showCard.SetActive(true);
            StartCoroutine(cardButton.DontShowCards());
            TryPutCard();
        }
    }
    public void TargetingChessPiece()
    {
        bool isMine;
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward))
        {

            if (hit.collider.CompareTag("ChessPiece"))
            {
                isMine = hit.collider.gameObject.name.Contains("white");
                if(!isMine)
                {
                    localPosition = hit.collider.transform.position;
                    SpawnTargetPicker(true);
                    targeting = true;
                }

            }
            else
            {
                targeting = false;
                SpawnTargetPicker(false);
            }
        }
    }
    public void SpawnTargetPicker(bool isShow)
    {
        targetPicker.transform.position = localPosition;
        targetPicker.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1));
        targetPicker.SetActive(isShow);
    }
    private void CardDrag()
    {
        spriteRenderer.enabled = false;
        cardIcon.enabled = true;
        showCard.SetActive(false);

        TargetingChessPiece();
        transform.DOMove(Utils.MousePos, 0.1f);
    }

    private void TryPutCard()
    {
        if(targeting)
        {
            showCard.SetActive(false);

            TutorialManager.Instance.card = false;
            StartCoroutine(HP_SkillEffect());
            spriteRenderer.enabled = false;
            cardIcon.enabled = false;
            targetPicker.SetActive(false);

        }
        else
        {
            transform.DOMove(new Vector2(2f, -4f), 0.3f);
            transform.DORotateQuaternion(Utils.QI, 0.3f);
            transform.DOScale(Vector3.zero, 0.3f);

        }
    }

    private IEnumerator HP_SkillEffect()
    {
        while(!TutorialManager.Instance.blackPawn2)
        {
            blackPiece.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            blackPiece.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }

    }
}
