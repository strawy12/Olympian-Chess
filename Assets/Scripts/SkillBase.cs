using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public ChessBase selectPiece { get; protected set; }
    public ChessBase selectPieceTo { get; protected set; }
    protected int posX;
    protected int posY;
    protected int turnCnt = 0;
    protected string player = "white";

    public virtual void UsingSkill() { }

    public virtual void StandardSkill() { }

    public virtual void ResetSkill() { }

    public void SetSelectPiece(ChessBase cp)
    {
        selectPiece = cp;
    }
    public void SetSelectPieceTo(ChessBase cp) 
    {
        selectPieceTo = cp;
    }

    public ChessBase GetSelectPieceTo()
    {
        return selectPieceTo;
    }

    public ChessBase GetSelectPiece()
    {
        return selectPiece;
    }

    public string GetPlayer()
    {
        return player;
    }
    public void SetPalyer(string player)
    {
        this.player = player;
    }
    public int GetPosX()
    {
        return posX;
    }

    public int GetPosY()
    {
        return posY;
    }

    public void SetPosX(int posX)
    {
        this.posX = posX;
    }

    public void SetPosY(int posY)
    {
        this.posY = posY;
    }

    public void TurnCntPlus()
    {
        turnCnt++;
    }
}