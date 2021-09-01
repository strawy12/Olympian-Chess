using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class Carditem
{
    public string name;
    public int ID;
    public string cardType;
    public string target;
    public string turn;
    public string god;
    public string player;
    public Sprite sprite;
    public Color color;
    [TextArea] public string info;
    [TextArea] public string detail;
}

[CreateAssetMenu(fileName = "CardItemSO", menuName = "Sprictable Object/CardItemSO")]
public class CardItemSO : ScriptableObject
{
    public Carditem[] cardItems;
}

