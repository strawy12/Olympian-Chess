using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessBase
{
    public override void MovePlate()
    {
        P_MovePlate();
    }

    private void P_MovePlate()
    {
        if(chessData.player == "white")
        {
            PawnMovePlate(chessData.xBoard, chessData.yBoard + 1);
        }

        else
        {
            PawnMovePlate(chessData.xBoard, chessData.yBoard - 1);
        }
    }
    public void PawnMovePlate(int x, int y)
    {
        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            if (ChessManager.Inst.GetPosition(x, y) == null)
            {
                if (chessData.moveCnt != 0)
                    GameManager.Inst.MovePlateSpawn(x, y, this);

                else
                {
                   
                    if (chessData.player == "white")
                    {
                        GameManager.Inst.MovePlateSpawn(x, y, this);
                        if (ChessManager.Inst.GetPosition(x, y + 1) == null)
                            GameManager.Inst.MovePlateSpawn(x, y + 1, this);
                    }

                    else if (chessData.player == "black")
                    {
                        GameManager.Inst.MovePlateSpawn(x, y, this);
                        if (ChessManager.Inst.GetPosition(x, y - 1) == null)
                            GameManager.Inst.MovePlateSpawn(x, y - 1, this);
                    }
                }
            }

            if (ChessManager.Inst.PositionOnBoard(x + 1, y) && ChessManager.Inst.GetPosition(x + 1, y) != null &&
               ChessManager.Inst.GetPosition(x + 1, y).GetChessData().player != chessData.player)
            {
                Debug.Log("응애");
                GameManager.Inst.MovePlateAttackSpawn(x + 1, y, this);
            }

            if (ChessManager.Inst.PositionOnBoard(x - 1, y) && ChessManager.Inst.GetPosition(x - 1, y) != null &&
                ChessManager.Inst.GetPosition(x - 1, y).GetChessData().player != chessData.player)
            {
                Debug.Log("응애");

                GameManager.Inst.MovePlateAttackSpawn(x - 1, y, this);
            }
        }
    }
}
