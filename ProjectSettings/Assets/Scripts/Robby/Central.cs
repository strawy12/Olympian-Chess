using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Central : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void BeginDrag(Transform card)
    {
        Debug.Log("BeginDrag" + card.name);
    }
    public void Drag(Transform card)
    {
        Debug.Log("Drag" + card.name);
    }
    public void EndDrag(Transform card)
    {
        Debug.Log("EndDrag" + card.name);
    }
}
