using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public Text text;
    private int gold = 1000;

    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        text.text = "$" + gold;
    }
}
