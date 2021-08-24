using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;

    public Slider tabSlider;
    public RectTransform[] BtnRect;
    public RectTransform[] BtnImageRect;

    const int SIZE = 3;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag;
    int targetIndex;

    void Start()
    {
        scrollbar.value = 0.5f;
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++)
        {
            pos[i] = distance * i;
        }
        targetPos = pos[1];
        targetIndex = 1;
        Debug.Log("1" + targetPos);
    }
    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();
    public void OnDrag(PointerEventData eventData) => isDrag = true;
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        targetPos = SetPos();
        Debug.Log("2" + targetPos);

        if (curPos == targetPos)
        {

            if (eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
                Debug.Log("1" + targetPos);
            }
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
                Debug.Log("2" + targetPos);
            }
        }
        for (int i = 0; i < SIZE; i++)
        {
            if (contentTr.GetChild(i).GetComponent<ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
        }

    }
    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                Debug.Log("3" + targetPos);
                return pos[i];
            }
        return 0.5f;
    }
    void Update()
    {
        tabSlider.value = scrollbar.value;

        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
            //Debug.Log("4" + targetPos);

            for (int i = 0; i < SIZE; i++) BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 400 : 340, BtnRect[i].sizeDelta.y);
        }

        if (Time.time < 0.1f) return;

        for (int i = 0; i < SIZE; i++)
        {
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;
            bool textActive = false;

            if (i == targetIndex)
            {
                BtnTargetPos.y = -23f;
                BtnTargetScale = new Vector3(1.2f, 1.2f, 1);
                textActive = true;
            }

            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.1f);
            BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, BtnTargetScale, 0.25f);
            BtnImageRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }

    }
    public void TabClicker(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
        Debug.Log("3" + targetPos);
    }
}