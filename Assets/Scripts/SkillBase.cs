using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public Chessman selectPiece { get; protected set; }
    public Chessman selectPieceTo { get; protected set; }
    protected int posX;
    protected int posY;
    protected int turnCnt = 0;
    protected string player = "white";

    public virtual void UsingSkill() {}

    public virtual void StandardSkill() {}

    public virtual void ResetSkill() {}

    public void SetSelectPiece(Chessman cp) 
    {
        selectPiece = cp;
    }

    public Chessman GetSelectPieceTo() 
    {
        return selectPieceTo;
    }

    public Chessman GetSelectPiece()
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
