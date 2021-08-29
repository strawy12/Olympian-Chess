using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImage : MonoBehaviour
{
    private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
        Color();
    }

    void Update()
    {
        
    }

    private void Color()
    {
        Color color = _image.color;
        color.a = 0f;

        float t = 0;
        while(t<1)
        {
            t += 0.05f;
            color.a = t;
            Debug.Log(t);
        }
    }
}
