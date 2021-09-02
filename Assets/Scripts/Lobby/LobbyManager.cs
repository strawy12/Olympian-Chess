using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Text text;
    private int gold = 1000;

    private Sprite[] backs;
    [SerializeField]
    private Sprite[] supers;

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image back_Shop;
    [SerializeField]
    private GameObject back_checkButton;

    [SerializeField]
    private Image super_Shop;
    [SerializeField]
    private GameObject super_checkButton;

    [SerializeField]
    private Slider bgmSound;
    [SerializeField]
    private Slider effectSound;

    [SerializeField]
    private Button back_button;
    [SerializeField]
    private Button super_button;

    [SerializeField]
    private Text super_Text;


    private User user;

    #region 스크롤
    [SerializeField] private ScrollRect lobbyScroll;
    private NestedScrollManager nestedScrollManager;
    [SerializeField] private Button btn1;
    [SerializeField] private Button btn2;
    [SerializeField] private Button btn3;
    [SerializeField] private Slider slider;
    #endregion

    int back_num = 0;
    int super_num = 0;

    void Start()
    {
        backs = Resources.LoadAll<Sprite>("Images/lobbychess");
        nestedScrollManager = lobbyScroll.gameObject.GetComponent<NestedScrollManager>();
        FirstSetting();

        UpdateUI();
        SetBackGround();
        SetSoundValue();
        SetSupersImage();

        SoundManager.Instance.SetLobbyBGM(Random.Range(0, 2));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void UpdateUI()
    {
        text.text = DeckManager.Instance.GetGold().ToString();
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void BG_Buy(int g)
    {
        user = DeckManager.Instance.GetUser();
        gold = user.gold;

        if (!user.myBackground[back_num] && gold - g > -1)
        {
            gold -= g;
            user.myBackground[back_num] = true;
            backgroundImage.sprite = backs[back_num];
            DeckManager.Instance.SetGold(gold);
            UpdateUI();
            SetBackGround();
            CheckBackGround();
            SoundManager.Instance.Buy();
        }
    }

    public void Super_Buy(int gold)
    {
        user = DeckManager.Instance.GetUser();
        this.gold = user.gold;

        if(this.gold - gold > -1)
        {
            this.gold -= gold;
            DeckManager.Instance.SetGold(this.gold);
            user.superSkills[super_num].amount++;
            SoundManager.Instance.Buy();
            SetSupersImage();
            UpdateUI();
        }
    }

    private void SetBackGround()
    {
        back_Shop.sprite = backs[back_num];
        user = DeckManager.Instance.GetUser();

        if (user.myBackground[back_num])
        {
            back_Shop.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            back_button.image.color = Color.red;
        }

        else
        {
            back_Shop.color = new Color(1f, 1f, 1f, 1f);
            back_button.image.color = Color.white;
        }

        if (user.backGround != back_num)
        {
            back_checkButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        else
        {
            back_checkButton.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void SetSupersImage()
    {
        user = DeckManager.Instance.GetUser();

        super_Shop.sprite = supers[super_num];
        super_Text.text = user.superSkills[super_num].amount.ToString() + "개";

        if(!user.superSkills[super_num].isSelect)
        {
            super_checkButton.transform.GetChild(0).gameObject.SetActive(false);
        }

        else
        {
            super_checkButton.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void Matching()
    {
        lobbyScroll.enabled = false;
        nestedScrollManager.enabled = false;
        btn1.interactable = false;
        btn2.interactable = false;
        btn3.interactable = false;
        slider.enabled = false;
    }

    public void CheckCardList(GameObject game)
    {
        string[] myDecks = DeckManager.Instance.GetUser().myDecks;
        int cnt = 0;
        for (int i = 0; i < myDecks.Length; i++)
        {
            if (myDecks[i] != "")
            {
                cnt++;
            }
        }
        if (cnt != 10)
        {
            StartCoroutine(DeckManager.Instance.Message("카드가 10장보다 부족합니다"));
            return;
        }
        if (game.name.Contains("Match"))
        {
            NetworkManager.Inst.JoinRandomRoom();
        }

        SoundManager.Instance.Match();
        game.SetActive(true);
        Matching();
    }

    public void Next(int num)
    {
        if(num == 0)
        {
            if (back_num + 1 == backs.Length)
                back_num = 0;
            else
                back_num++;

            SetBackGround();
        }

        else
        {
            if (super_num + 1 == supers.Length)
                super_num = 0;
            else
                super_num++;

            SetSupersImage();
        }
    }

    public void Recent(int num)
    {
        if(num == 0)
        {
            if (back_num == 0)
                back_num = backs.Length - 1;
            else
                back_num--;

            SetBackGround();
        }

        else
        {
            if (super_num == 0)
                super_num = supers.Length - 1;
            else
                super_num--;

            SetSupersImage();
        }
    }

    public void CheckBackGround()
    {
        user = DeckManager.Instance.GetUser();

        if (user.myBackground[back_num] && user.backGround != back_num)
        {
            back_checkButton.transform.GetChild(0).gameObject.SetActive(true);
            DeckManager.Instance.SetBackground(back_num);
            backgroundImage.sprite = backs[user.backGround];
        }
    }

    public void CheckSuperSkill()
    {
        user = DeckManager.Instance.GetUser();

        if(user.superSkills[super_num].isSelect)
        {
            super_checkButton.transform.GetChild(0).gameObject.SetActive(false);
            user.superSkills[super_num].isSelect = false;
        }

        else if(!user.superSkills[super_num].isSelect)
        {
            for (int i = 0; i < user.superSkills.Length; i++)
                user.superSkills[i].isSelect = false;

            super_checkButton.transform.GetChild(0).gameObject.SetActive(true);
            user.superSkills[super_num].isSelect = true;
        }

        NetworkManager.Inst.SaveDataToJson(user, true);
    }

    private void FirstSetting()
    {
        user = DeckManager.Instance.GetUser();

        if (DeckManager.Instance.GetBackground() == 0)
        {
            DeckManager.Instance.SetBackground(0);
            backgroundImage.sprite = backs[0];
        }

        else
        {
            backgroundImage.sprite = backs[user.backGround];
            back_num = user.backGround;
            SetBackGround();
        }

        user.myBackground[0] = true;
    }

    public void SetBGMValue(Text text)
    {
        text.text = bgmSound.value.ToString();
    }

    public void SetEffectValue(Text text)
    {
        text.text = effectSound.value.ToString();
    }

    private void SetSoundValue()
    {
        User user = DeckManager.Instance.GetUser();

        bgmSound.value = user.bgmVolume * 10;
        effectSound.value = user.effectVolume * 10;
    }

    public void SaveVolume()
    {
        DeckManager.Instance.SetSoundVolume(bgmSound.value / 10f, effectSound.value / 10f);
    }
}