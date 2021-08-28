using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoButton : MonoBehaviour
{
    [SerializeField]
    private Sprite activeSprite;

    [SerializeField]
    private Sprite unactiveSprite;

    private Image image;

    private bool allowClick = false;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        ActiveButtono();
    }

    private void ActiveButtono()
    {
        if(TutorialManager.Instance.is5Story)
        {
            image.sprite = activeSprite;
            TutorialManager.Instance.is5Story = false;
            allowClick = true;
        }
    }

    public void isClickedActiveButton()
    {
        if (allowClick)
        {
            TutorialManager.Instance.turnEnd = true;
            image.sprite = unactiveSprite;
            //TutorialManager.Instance.is5StoryEnd = true;
        }
    }
}
