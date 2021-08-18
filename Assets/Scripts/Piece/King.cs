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
                GameManager.Inst.MovePlateSpawn(x, y, this);
            }

            else if (cb.player != player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x, y, this);
            }
        }
    }

    private void CastlingMovePlate()
    {
        if (player == "white")
        {
            if (ChessManager.Inst.Castling(player, moveCnt, true))
            {
                {
                    //white king side
                    if (!CheckMovePlate(player, 5, 0) && !CheckMovePlate(player, 6, 0))
                    {
                        GameManager.Inst.MovePlateSpawn(6, 0, this);
                    }
                }
            }

            if (ChessManager.Inst.Castling(player, moveCnt, false))
            {
                //white queen side
                if (!CheckMovePlate(player, 1, 0) && !CheckMovePlate(player, 2, 0) && !CheckMovePlate(player, 3, 0))
                {
                    GameManager.Inst.MovePlateSpawn(2, 0, this);
                    Debug.Log("¹é Äý»çÀÌµå");
                }
            }
        }

        if (player == "black")
        {
            if (ChessManager.Inst.Castling(player, moveCnt, true))
            {
                //black king side
                if (!CheckMovePlate(player, 5, 7) && !CheckMovePlate(player, 6, 7))
                {
                    GameManager.Inst.MovePlateSpawn(6, 7, this);
                }
            }

            else if (ChessManager.Inst.Castling(player, moveCnt, false))
            {
                //black queen side
                if (!CheckMovePlate(player, 1, 7) && !CheckMovePlate(player, 2, 7) && !CheckMovePlate(player, 3, 7))
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

    private void RookCastling()
    {
        if (moveCnt == 0)
        {
            if (player == "white")
            {
                if (xBoard == 6 && yBoard == 0)
                {
                    ChessManager.Inst.RookCastling(player, true);
                    Debug.Log("sdf");
                }
                if (xBoard == 2 && yBoard == 0)
                    ChessManager.Inst.RookCastling(player, false);
            }

            else
            {
                if (xBoard == 6 && yBoard == 7)
                    ChessManager.Inst.RookCastling(player, true);
                if (xBoard == 2 && yBoard == 7)
                    ChessManager.Inst.RookCastling(player, false);
            }
        }
    }
}
