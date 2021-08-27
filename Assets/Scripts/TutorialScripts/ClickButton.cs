using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    [SerializeField]
    private GameObject ActiveButton;
    [SerializeField]
    private GameObject UnActiveButton;
    
    public void isClickedActiveButton()
    {
        ActiveButton.SetActive(false);
        UnActiveButton.SetActive(true);
        TutorialManager.Instance.is5StoryEnd = true;
    }
}
