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
        ChessManager.Inst.PointMovePlate(xBoard + 1, yBoard + 2, this);
        ChessManager.Inst.PointMovePlate(xBoard - 1, yBoard + 2, this);
        ChessManager.Inst.PointMovePlate(xBoard + 2, yBoard + 1, this);
        ChessManager.Inst.PointMovePlate(xBoard + 2, yBoard - 1, this);
        ChessManager.Inst.PointMovePlate(xBoard + 1, yBoard - 2, this);
        ChessManager.Inst.PointMovePlate(xBoard - 1, yBoard - 2, this);
        ChessManager.Inst.PointMovePlate(xBoard - 2, yBoard + 1, this);
        ChessManager.Inst.PointMovePlate(xBoard - 2, yBoard - 1, this);
    }

}
