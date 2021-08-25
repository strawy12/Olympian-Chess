using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text text;
    private int gold = 1000;

    public Sprite[] backGroundSprites;
    public Image BGI;
    public Image BG_Shop;
    public bool[] isBGBought;


    public bool[] isGodBought;
    public Sprite[] GodSprites;
    public Image G_Shop;

    public Button BGbutton;
    public Button Godbutton;

    int num = 0;
    int Gnum = 0;

    void Start()
    {
        for (int i = 0; i < isBGBought.Length; i++)
        {
            isBGBought[i] = false;
        }
        for (int i = 0; i < isGodBought.Length; i++)
        {
            isGodBought[i] = false;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        text.text = "$" + gold;
    }

<<<<<<< HEAD
    public void OnClickStart()
    {
        SceneManager.LoadScene("Game");
=======
    public void BG_Buy(int g)
    {
        if (!isBGBought[num])
        {
            gold -= g;
            isBGBought[num] = true;
            BGI.sprite = backGroundSprites[num];
            UpdateUI();
        }
    }

    private void SetBackGround()
    {
        BG_Shop.sprite = backGroundSprites[num];
        if(isBGBought[num])
        {
            BG_Shop.color = new Color(0.3f, 0.3f, 0.3f, 1f);
           BGbutton.image.color = Color.red;
        }
        else
        {
            BG_Shop.color = new Color(1f, 1f, 1f, 1f);
            BGbutton.image.color = Color.white;
        }
    }

    public void Next()
    {
        if(!(num - 1 < 1))
        {
            num++;
            SetBackGround();
        }
    }

    public void Recent()
    {
        if (!(num - 1 < 1))
        {
            num--;
            SetBackGround();
        }
    }














    public void God_Buy(int g)
    {
        if (!isGodBought[Gnum])
        {
            gold -= g;
            isGodBought[Gnum] = true;
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
>>>>>>> ef858b9dd44eeaec44e4b55add77141e896f57b5
    }
}
