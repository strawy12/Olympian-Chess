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
        if(ChessManager.Inst.Castling(player, moveCnt))
        {
            Debug.Log("캐슬링 가능");

            if (player == "white")
            {
                Debug.Log("화이트라면");
                Debug.Log(CheckMovePlate(player, 5, 0));
                //white king side
                if (!CheckMovePlate(player, 5, 0) && !CheckMovePlate(player, 6, 0))
                {
                    GameManager.Inst.MovePlateSpawn(6, 0, this);
                    Debug.Log("백 킹사이드");
                }

                //white queen side
                else if (!CheckMovePlate(player, 1, 0) && !CheckMovePlate(player, 2, 0) && !CheckMovePlate(player, 3, 0))
                {
                    GameManager.Inst.MovePlateSpawn(2, 0, this);
                    Debug.Log("백 퀸사이드");
                }
            }

            else
            {
                //black king side
                if (!CheckMovePlate(player, 5, 7) && !CheckMovePlate(player, 6, 7))
                {
                    GameManager.Inst.MovePlateSpawn(7, 6, this);
                }

                //black queen side
                else if (!CheckMovePlate(player, 1, 7) && !CheckMovePlate(player, 2, 7) && !CheckMovePlate(player, 3, 7))
                {
                    GameManager.Inst.MovePlateSpawn(7, 2, this);
                }
            }
        }
    }

    private bool CheckMovePlate(string player, int x, int y)
    {
        return ChessManager.Inst.CheckMovePlate(player, x, y);
    }
}
