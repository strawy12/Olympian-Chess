using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessBase
{
    public override void MovePlate()
    {
        Ki_MovePlate();
    }

    private void Ki_MovePlate()
    {
        PointMovePlate(chessData.xBoard, chessData.yBoard + 1);
        PointMovePlate(chessData.xBoard, chessData.yBoard - 1);
        PointMovePlate(chessData.xBoard - 1, chessData.yBoard - 1);
        PointMovePlate(chessData.xBoard - 1, chessData.yBoard - 0);
        PointMovePlate(chessData.xBoard - 1, chessData.yBoard + 1);
        PointMovePlate(chessData.xBoard + 1, chessData.yBoard - 1);
        PointMovePlate(chessData.xBoard + 1, chessData.yBoard - 0);
        PointMovePlate(chessData.xBoard + 1, chessData.yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {

        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            ChessBase cb = ChessManager.Inst.GetPosition(x, y);

            if (cb == null)
            {
                GameManager.Inst.MovePlateSpawn(x, y, this);
            }

            else if (cb.GetChessData().player != chessData.player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x, y, this);
            }

        }
    }
}
