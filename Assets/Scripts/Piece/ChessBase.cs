using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBase : MonoBehaviour
{
    protected int moveCnt = 0;

    protected int xBoard = -1;
    protected int yBoard = -1;

    public bool attack = false;
    public string player;

    public virtual void MovePlate() { }

    public virtual void MovePosition() { }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    public void PlusMoveCnt()
    {
        moveCnt++;
    }

    public int GetMoveCnt()
    {
        return moveCnt;
    }
    public void OnMouseUp()
    {
        //List<Chessman> attack = GameManager.Inst.attackings;
        if (TurnManager.Instance.GetIsActive()) return;

        ChessManager.Inst.DestroyMovePlates();
        MovePlate();

        //if (!GameManager.Inst.IsGameOver() && GameManager.Inst.GetCurrentPlayer() == player)
        //{

        //}
    }

}
