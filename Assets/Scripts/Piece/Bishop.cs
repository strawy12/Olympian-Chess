using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessBase
{
    public override void MovePlate()
    {
        B_MovePlate();
    }

    public void B_MovePlate()
    {
        LineMovePlate(1, 1);
        LineMovePlate(1, -1);
        LineMovePlate(-1, 1);
        LineMovePlate(-1, -1);
    }
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        int x = chessData.xBoard + xIncrement;
        int y = chessData.yBoard + yIncrement;

        while (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y) == null)
        {
            GameManager.Inst.MovePlateSpawn(x, y, this);
            x += xIncrement;
            y += yIncrement;
        }

        if (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y).player != player)
        {
            GameManager.Inst.MovePlateAttackSpawn(x, y, this);
        }
    }
}
