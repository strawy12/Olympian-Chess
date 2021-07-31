using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Carditem
{
    public string name;
    public Sprite sprite;
    public string god;
    [TextArea]
    public string info;
}

[CreateAssetMenu(fileName = "CardItemSO", menuName = "Sprictable Object/CardItemSO")]
public class CardItemSO : ScriptableObject
{
    public Carditem[] cardItems;
}

