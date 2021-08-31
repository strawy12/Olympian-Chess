using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Text text;
    private int gold = 1000;

    private Sprite[] backs;
    private Sprite[] longBacks;

    [SerializeField]
    private Image BGI;
    [SerializeField]
    private Image BG_Shop;
    [SerializeField]
    private bool[] isBGBought;
    [SerializeField]
    private GameObject checkButton;

    public Button BGbutton;

    private User user;

    int num = 0;

    void Start()
    {
        backs = Resources.LoadAll<Sprite>("Images/lobbychess");
        longBacks = Resources.LoadAll<Sprite>("Images/ingameBackground");
        
        FirstSetting();

        UpdateUI();
        SetBackGround();
    }

    private void UpdateUI()
    {
        text.text = "$" + DeckManager.Instance.GetGold();
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void BG_Buy(int g)
    {
        gold = DeckManager.Instance.GetGold();
        user = DeckManager.Instance.GetUser();

        if (!user.myBackground[num] && gold - g > -1)
        {
            gold -= g;
            user.myBackground[num] = true;
            BGI.sprite = longBacks[num];
            DeckManager.Instance.SetGold(gold);
            UpdateUI();
            SetBackGround();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            gold = DeckManager.Instance.GetGold();
            gold += 1000;
            DeckManager.Instance.SetGold(gold);
            UpdateUI();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void SetBackGround()
    {
        BG_Shop.sprite = backs[num];
        user = DeckManager.Instance.GetUser();

        if (user.myBackground[num])
        {
            BG_Shop.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            BGbutton.image.color = Color.red;
        }

        else
        {
            BG_Shop.color = new Color(1f, 1f, 1f, 1f);
            BGbutton.image.color = Color.white;
        }

        if(user.backGround != num)
        {
            checkButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        else
        {
            checkButton.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void CheckCardList(GameObject game)
    {
        string[] myDecks = DeckManager.Instance.GetUser().myDecks;
        int cnt = 0;
        for (int i = 0; i < myDecks.Length; i++)
        {
            if (myDecks[i] != null || myDecks[i] != "")
            {
                cnt++;
            }
        }
        if (cnt != 10)
        {
            StartCoroutine(DeckManager.Instance.Message("카드가 10장보다 부족합니다"));
            return;
        }
        if(game.name.Contains("Match"))
        {
            NetworkManager.Inst.JoinRandomRoom();
        }

        game.SetActive(true);
    }

    public void Next()
    {
        if (num + 1 == backs.Length)
            num = 0;
        else
            num++;

        SetBackGround();
    }

    public void Recent()
    {
        if (num == 0)
            num = backs.Length - 1;
        else
            num--;

        SetBackGround();
    }


    public void CheckBackGround()
    {
        user = DeckManager.Instance.GetUser();

        if (user.myBackground[num] && user.backGround != num)
        {
            checkButton.transform.GetChild(0).gameObject.SetActive(true);
            DeckManager.Instance.SetBackground(num);
            BGI.sprite = longBacks[user.backGround];
        }
    }

    private void FirstSetting()
    {
        user = DeckManager.Instance.GetUser();

        if (DeckManager.Instance.GetBackground() == 0)
        {
            DeckManager.Instance.SetBackground(0);
            BGI.sprite = longBacks[0];
        }
        else
        {
            BGI.sprite = longBacks[user.backGround];
        }

        user.myBackground[0] = true;
    }
}
