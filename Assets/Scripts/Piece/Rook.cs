using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessBase
{
    public override void MovePlate()
    {
        R_MovePlate();
    }

    private void R_MovePlate()
    {
        LineMovePlate(1, 0);
        LineMovePlate(0, 1);
        LineMovePlate(-1, 0);
        LineMovePlate(0, -1);
    }
    public void LineMovePlate(int xIncrement, int yIncrement)
    {

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y) == null)
        {
            ChessManager.Inst.MovePlateSpawn(this, x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y).player != player)
        {
            ChessManager.Inst.MovePlateAttackSpawn(x, y);
        }
    }
}
