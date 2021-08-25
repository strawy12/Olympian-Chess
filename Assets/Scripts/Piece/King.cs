using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessBase
{
    public override void MovePlate()
    {
        CastlingMovePlate();
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

    private void CastlingMovePlate()
    {
        if (player == "white")
        {
            if (ChessManager.Inst.Castling("white", moveCnt, true))
            {
                {
                    //white king side
                    if (!CheckMovePlate("white", 5, 0) && !CheckMovePlate("white", 6, 0))
                    {
                        GameManager.Inst.MovePlateSpawn(6, 0, this);
                    }
                }
            }

            if (ChessManager.Inst.Castling("white", moveCnt, false))
            {
                //white queen side
                if (!CheckMovePlate("white", 1, 0) && !CheckMovePlate("white", 2, 0) && !CheckMovePlate("white", 3, 0))
                {
                    GameManager.Inst.MovePlateSpawn(2, 0, this);
                    Debug.Log("¹é Äý»çÀÌµå");
                }
            }
        }

        if (player == "black")
        {
            if (ChessManager.Inst.Castling("black", moveCnt, true))
            {
                //black king side
                if (!CheckMovePlate("black", 5, 7) && !CheckMovePlate("black", 6, 7))
                {
                    GameManager.Inst.MovePlateSpawn(6, 7, this);
                }
            }

            else if (ChessManager.Inst.Castling("black", moveCnt, false))
            {
                //black queen side
                if (!CheckMovePlate("black", 1, 7) && !CheckMovePlate("black", 2, 7) && !CheckMovePlate("black", 3, 7))
                {
                    GameManager.Inst.MovePlateSpawn(2, 7, this);
                }
            }
        }
    }

    private bool CheckMovePlate(string player, int x, int y)
    {
        return ChessManager.Inst.CheckMovePlate(player, x, y);
    }
}
