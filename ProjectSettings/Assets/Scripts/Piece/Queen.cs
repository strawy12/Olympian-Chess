using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessBase
{
    public override void MovePlate()
    {
        Q_MovePlate();
    }

    private void Q_MovePlate()
    {
        LineMovePlate(1, 0);
        LineMovePlate(0, 1);
        LineMovePlate(1, 1);
        LineMovePlate(-1, 0);
        LineMovePlate(0, -1);
        LineMovePlate(-1, -1);
        LineMovePlate(-1, 1);
        LineMovePlate(1, -1);
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

        if (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y).GetChessData().player != chessData.player)
        {
            GameManager.Inst.MovePlateAttackSpawn(x, y, this);
        }
    }
}
