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

    [SerializeField]
    private Sprite[] backGroundSprites;
    [SerializeField]
    private Image BGI;
    [SerializeField]
    private Image BG_Shop;
    [SerializeField]
    private bool[] isBGBought;
    [SerializeField]
    private GameObject checkButton;

    public bool[] isGodBought;
    public Sprite[] GodSprites;
    public Image G_Shop;

    public Button BGbutton;
    public Button Godbutton;

    User user;

    int num = 0;
    int Gnum = 0;

    void Start()
    {
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
            BGI.sprite = backGroundSprites[num];
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
    }

    private void SetBackGround()
    {
        BG_Shop.sprite = backGroundSprites[num];
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

        if(user.backGround != backGroundSprites[num].name)
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
        NetworkManager.Inst.JoinRandomRoom();
        game.SetActive(true);
    }

    public void Next()
    {
        if (num + 1 == backGroundSprites.Length)
            num = 0;
        else
            num++;

        SetBackGround();
    }

    public void Recent()
    {
        if (num == 0)
            num = backGroundSprites.Length - 1;
        else
            num--;

        SetBackGround();
    }

    public void God_Buy(int g)
    {
        gold = DeckManager.Instance.GetGold();

        if (!isGodBought[Gnum])
        {
            gold -= g;
            isGodBought[Gnum] = true;
            DeckManager.Instance.SetGold(gold);
            UpdateUI();
        }
    }

    private void SetGod()
    {
        G_Shop.sprite = GodSprites[Gnum];
        if (isGodBought[Gnum])
        {
            G_Shop.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            Godbutton.image.color = Color.red;
        }
        else
        {
            G_Shop.color = new Color(1f, 1f, 1f, 1f);
            Godbutton.image.color = Color.white;
        }
    }

    public void GNext()
    {
        if (!(Gnum - 1 < 1))
        {
            Gnum++;
            SetGod();
        }
    }

    public void GRecent()
    {
        if (!(Gnum - 1 < 1))
        {
            Gnum--;
            SetGod();
        }
    }

    public void CheckBackGround()
    {
        user = DeckManager.Instance.GetUser();

        if (user.myBackground[num] && user.backGround != backGroundSprites[num].name)
        {
            checkButton.transform.GetChild(0).gameObject.SetActive(true);
            DeckManager.Instance.SetBackground(backGroundSprites[num].name);
        }
    }

    private void FirstSetting()
    {
        user = DeckManager.Instance.GetUser();

        if (DeckManager.Instance.GetBackground() == "")
            DeckManager.Instance.SetBackground(backGroundSprites[0].name);

        user.myBackground[0] = true;
    }
}
