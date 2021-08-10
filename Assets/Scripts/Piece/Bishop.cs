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
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y) == null)
        {
            ChessManager.Inst.MovePlateSpawn(this, x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y).player != player)
        {
            ChessManager.Inst.MovePlateAttackSpawn(x, y);
        }
    }
}
