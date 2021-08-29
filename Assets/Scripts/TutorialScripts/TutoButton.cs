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
        if (TutorialManager.Instance.is5Story)
        {
            ActiveButtono();

        }
    }

    private void ActiveButtono()
    {

            image.sprite = activeSprite;
            TutorialManager.Instance.is5Story = false;
            allowClick = true;
        
    }

    public void isClickedActiveButton()
    {
        if (allowClick)
        {
            TutorialManager.Instance.turnEnd = true;
            TutorialManager.Instance.ButtonClickSound();
            image.sprite = unactiveSprite;
            //TutorialManager.Instance.is5StoryEnd = true;
        }
    }
}
