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
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 0);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 0);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {

        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            ChessBase cb = ChessManager.Inst.GetPosition(x, y);

            if (cb == null)
            {
                ChessManager.Inst.MovePlateSpawn(this, x, y);
            }

            else if (cb.player != player)
            {
                ChessManager.Inst.MovePlateAttackSpawn(this, x, y);
            }

        }
    }
}
