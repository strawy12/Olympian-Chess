using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessBase
{
    public override void MovePlate()
    {
        Kn_MovePlate();
    }

    private void Kn_MovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }
    public void PointMovePlate(int x, int y)
    {

        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            ChessBase cb = ChessManager.Inst.GetPosition(x, y);

            if (cb == null)
            {
                ChessManager.Inst.MovePlateSpawn(this,x, y);
            }

            else if (cb.player != player)
            {
                ChessManager.Inst.MovePlateAttackSpawn(x, y);
            }

        }
    }
}
