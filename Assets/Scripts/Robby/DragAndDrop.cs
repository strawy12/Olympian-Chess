using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    [SerializeField] private Canvas canvas;

    private Transform root;
    private CanvasGroup canvasGroup;
    [SerializeField] private Transform[] cards;
    private void Awake()
    {
        root = transform.root;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        root.BroadcastMessage("BeginDrag", transform, SendMessageOptions.DontRequireReceiver);
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        root.BroadcastMessage("Drag", transform, SendMessageOptions.DontRequireReceiver);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        root.BroadcastMessage("EndDrag", transform, SendMessageOptions.DontRequireReceiver);

    }

}
