using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    [SerializeField]
    private Sprite activeSprite;

    [SerializeField]
    private Sprite unactiveSprite;

    [SerializeField]
    List<GameObject> cards;

    [SerializeField]
    GameObject hp_Card;

    private Image image;

    private bool allowClick = false;

    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(DontShowCards());
    }

    public void isClickedActiveButton()
    {
        if (TutorialManager.Instance.card)
        {
            SoundManager.Instance.Deck();
            image.sprite = unactiveSprite;
            hp_Card = cards[0];
            ShowCardAlignment();
        }
    }

    private void ShowCardAlignment()
    {
        List<PRS> originCardPRSs = new List<PRS>();

        originCardPRSs = ShowCards(cards.Count, Vector3.one * 1.7f);
        StartCoroutine(SetPosition(originCardPRSs));
    }
    private IEnumerator SetPosition(List<PRS> originCardPRSs)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            MoveTransform(cards[i], originCardPRSs[i], true, 0.7f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public List<PRS> ShowCards(int objCnt, Vector3 scale)
    {
        float[] objYpos = new float[0];
        float[] objLerps = new float[objCnt];
        float interval = 1f / 3;
        List<PRS> results = new List<PRS>(objCnt);
        Vector3 targetPos;
        Quaternion targetRot;

        switch (Mathf.CeilToInt((float)objCnt / 4))
        {
            case 1: objYpos = new float[1] { 1.5f }; break;
            case 2: objYpos = new float[2] { 0f, 1.5f }; break;
            case 3: objYpos = new float[3] { -1.5f, 0f, 1.5f }; break;
        }
        targetRot = Utils.QI;


        for (int i = objYpos.Length - 1; i >= 0; i--)
        {

            if (objCnt == 0) break;

            for (int j = 0, k = objCnt > 4 ? 4 : objCnt; j < k; j++)
            {
                if (objCnt == 0) break;
                objCnt--;
                targetPos = Vector3.Lerp(new Vector2(-1.8f, objYpos[i]), new Vector2(1.8f, objYpos[i]), interval * j);
                results.Add(new PRS(targetPos, targetRot, scale));
            }
        }
        return results;


    }
    public void MoveTransform(GameObject card, PRS prs, bool useDotween, float dotweemTime = 0) // Card Move
    {
        if (useDotween)
        {
            card.transform.DOMove(prs.pos, dotweemTime);
            card.transform.DORotateQuaternion(prs.rot, dotweemTime);
            card.transform.DOScale(prs.scale, dotweemTime);
        }
        else
        {
            card.transform.position = prs.pos;
            card.transform.rotation = prs.rot;
            card.transform.localScale = prs.scale;
        }
    }
    public IEnumerator DontShowCards()
    {
        Vector2 targetPos = new Vector2(2f, -4f);
        Quaternion targetRot = Utils.QI;



        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == hp_Card) continue;
            MoveTransform(cards[i], new PRS(targetPos, targetRot, Vector3.zero), true, 0.7f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
