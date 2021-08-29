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
        if (TurnManager.Instance.GetCurrentPlayer() == GetPlayer())
            CastlingMovePlate();

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

    private void CastlingMovePlate()
    {
        if (chessData.player == "white")
        {
            if (ChessManager.Inst.Castling("white", chessData.moveCnt, true))
            {
                {
                    //white king side
                    if (!Check("white", 5, 0) && !Check("white", 6, 0))
                    {
                        GameManager.Inst.MovePlateSpawn(6, 0, this);
                    }
                }
            }

            if (ChessManager.Inst.Castling("white", chessData.moveCnt, false))
            {
                //white queen side
                if (!Check("white", 1, 0) && !Check("white", 2, 0) && !Check("white", 3, 0))
                {
                    GameManager.Inst.MovePlateSpawn(2, 0, this);
                    Debug.Log("¹é Äý»çÀÌµå");
                }
            }
        }

        if (chessData.player == "black")
        {
            if (ChessManager.Inst.Castling("black", chessData.moveCnt, true))
            {
                //black king side
                if (!Check("black", 5, 7) && !Check("black", 6, 7))
                {
                    GameManager.Inst.MovePlateSpawn(6, 7, this);
                }
            }

            else if (ChessManager.Inst.Castling("black", chessData.moveCnt, false))
            {
                //black queen side
                if (!Check("black", 1, 7) && !Check("black", 2, 7) && !Check("black", 3, 7))
                {
                    GameManager.Inst.MovePlateSpawn(2, 7, this);
                }
            }
        }
    }

    private bool Check(string player, int x, int y)
    {
        return ChessManager.Inst.CheckMovePlate(player, x, y);
    }
}
