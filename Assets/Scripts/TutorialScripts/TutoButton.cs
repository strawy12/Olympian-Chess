using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoButton : MonoBehaviour
{
    [SerializeField]
    private GameObject ActiveButton;
    [SerializeField]
    private GameObject UnActiveButton;

    void Start()
    {
        
    }

    void Update()
    {
        ActiveButtono();
    }

    private void ActiveButtono()
    {
        if(TutorialManager.Instance.is5Story)
        {
            UnActiveButton.SetActive(false);
            ActiveButton.SetActive(true);
            TutorialManager.Instance.is5Story = false;
        }
    }

    public void isClickedActiveButton()
    {
        ActiveButton.SetActive(false);
        UnActiveButton.SetActive(true);
        TutorialManager.Instance.is5StoryEnd = true;
        TutorialManager.Instance.is6Story = true;
        //TutorialManager.Instance.is5StoryEnd = true;
    }
}
