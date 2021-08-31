using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class Carditem
{
    public string name;
    public string className;
    public string god;
    public Sprite sprite;
    public Color color;
    [TextArea] public string info;
    [TextArea] public string detail;
    public int ID;
    public string player;
}

[CreateAssetMenu(fileName = "CardItemSO", menuName = "Sprictable Object/CardItemSO")]
public class CardItemSO : ScriptableObject
{
    public Carditem[] cardItems;
}

