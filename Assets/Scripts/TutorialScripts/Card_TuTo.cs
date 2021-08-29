using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_TuTo : MonoBehaviour
{
    private bool isClick = false;
    private bool isMyCardDrag = false;
    private float pointDownTime = 0f;
    private Vector3 localPosition;
   [SerializeField] private GameObject cardInfo;
    [SerializeField]private GameObject targetPicker;
    private void Update()
    {
        if (isClick)
        {
            if (pointDownTime > 1f)
            {
                isMyCardDrag = true;
            }

            pointDownTime += Time.deltaTime;

        }

        if(isMyCardDrag)
        {
            CardDrag();
        }
    }
    void OnMouseDown()
    {
        if(TutorialManager.Instance.card)
        {
            isClick = true;
        }
    }

    void OnMouseUp()
    {
        if (TutorialManager.Instance.card)
        {
            isClick = false;
            TutorialManager.Instance.card = false;

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
                }

            }
            else
            {
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
        TargetingChessPiece();
        transform.DOMove(Utils.MousePos, 0.7f);

        cardInfo.SetActive(false);
    }
}
