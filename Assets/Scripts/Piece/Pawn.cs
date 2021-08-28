using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessBase
{
    private int originPosX;
    private bool isEnpassant = false;

    protected override void Start()
    {
        base.Start();
        originPosX = chessData.xBoard;
    }

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
        if (!isEnpassant)
        {
            if (chessData.moveCnt == 2 && chessData.xBoard == originPosX)
            {
                isEnpassant = true;
                if (chessData.player == "white" && chessData.yBoard == 4) { y -= 1; }
                else if (chessData.player == "black" && chessData.yBoard == 3) { y += 1; }

                else return;

                if (ChessManager.Inst.PositionOnBoard(x + 1, y) && ChessManager.Inst.GetPosition(x + 1, y) != null &&
               ChessManager.Inst.GetPosition(x + 1, y).GetComponent<ChessBase>().GetPlayer() != chessData.player &&
               ChessManager.Inst.GetPosition(x + 1, y).name.Contains("pawn"))
                {
                    GameManager.Inst.MovePlateAttackSpawn(x + 1, y, this);
                }

                if (ChessManager.Inst.PositionOnBoard(x - 1, y) && ChessManager.Inst.GetPosition(x - 1, y) != null &&
               ChessManager.Inst.GetPosition(x - 1, y).GetComponent<ChessBase>().GetPlayer() != chessData.player &&
               ChessManager.Inst.GetPosition(x - 1, y).name.Contains("pawn"))
                {
                    GameManager.Inst.MovePlateAttackSpawn(x - 1, y, this);
                }
            }
        }

        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            if (ChessManager.Inst.GetPosition(x, y) == null)
            {
                if (chessData.moveCnt != 0)
                    GameManager.Inst.MovePlateSpawn(x, y, this);

                if (GameManager.Inst.isBacchrs) return;

                if(chessData.moveCnt == 0)
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
                GameManager.Inst.MovePlateAttackSpawn(x + 1, y, this);
            }

            if (ChessManager.Inst.PositionOnBoard(x - 1, y) && ChessManager.Inst.GetPosition(x - 1, y) != null &&
                ChessManager.Inst.GetPosition(x - 1, y).GetChessData().player != chessData.player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x - 1, y, this);
            }
        }
    }
}
