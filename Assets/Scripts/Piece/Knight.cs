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
        ChessManager.Inst.PointMovePlate(chessData.xBoard + 1, chessData.yBoard + 2, this);
        ChessManager.Inst.PointMovePlate(chessData.xBoard - 1, chessData.yBoard + 2, this);
        ChessManager.Inst.PointMovePlate(chessData.xBoard + 2, chessData.yBoard + 1, this);
        ChessManager.Inst.PointMovePlate(chessData.xBoard + 2, chessData.yBoard - 1, this);
        ChessManager.Inst.PointMovePlate(chessData.xBoard + 1, chessData.yBoard - 2, this);
        ChessManager.Inst.PointMovePlate(chessData.xBoard - 1, chessData.yBoard - 2, this);
        ChessManager.Inst.PointMovePlate(chessData.xBoard - 2, chessData.yBoard + 1, this);
        ChessManager.Inst.PointMovePlate(chessData.xBoard - 2, chessData.yBoard - 1, this);
    }

}
