using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    Image image = null;
    void Start()
    {
        image  = GetComponent<Image>();
        StartCoroutine(FadeIn_());
    }

    private IEnumerator FadeIn_()
    {
        while(image.color.a > 0.5)
        {
            image.color = new Color(0f, 0f, 0f, image.color.a - 0.005f);
            yield return new WaitForSeconds(0.01f);
            if(image.color.a <= 0.5)
            {
                gameObject.SetActive(false);
            }
        }
        yield return new WaitForSeconds(1);
    }

}
