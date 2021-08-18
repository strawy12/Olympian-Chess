using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChessListData
{
    public string player;
    public ChessBase[] chesses;

    public ChessListData(string player, ChessBase[] chesses)
    {
        this.player = player;
        this.chesses = chesses;
    }
}
[System.Serializable]
public class PositionData
{
    public ChessBase[,] position;

    public PositionData(ChessBase[,] position)
    {
        this.position = position;
    }
}
[System.Serializable]
public class GameData
{
    public List<SkillBase> skillList;
    public List<ChessBase> dontClickPiece;
    public List<Carditem> usedCards;

    public GameData(List<Carditem> usedCards, List<ChessBase> dontClickPiece, List<SkillBase> skillList)
    {
        this.usedCards = usedCards;
        this.dontClickPiece = dontClickPiece;
        this.skillList = skillList;
    }
}
