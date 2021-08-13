using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public string name;
    public string color;
    public Card[] cards;
    public ChessBase[] chesses;

    public PlayerData(string name, string color, Card[] cards, ChessBase[] chesses)
    {
        this.name = name;
        this.color = color;
        this.cards = cards;
        this.chesses = chesses;
    }
}

public class PositionData
{
    public ChessBase[,] position;

    public PositionData(ChessBase[,] position)
    {
        this.position = position;
    }
}
