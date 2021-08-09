using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessBase
{
    public override void Move()
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

        if (GameManager.Inst.PositionOnBoard(x, y))
        {
            Chessman cp = GameManager.Inst.GetPosition(x, y);

            if (cp == null)
            {
                ChessManager.Inst.MovePlateSpawn(this, x, y);
            }

            else if (cp.player != player)
            {
                ChessManager.Inst.MovePlateAttackSpawn(x, y);
            }

        }
    }
}
