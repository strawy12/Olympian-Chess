using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessBase
{
    private int originPosX;
    private bool isEnpassant = false;

    private void Start()
    {
        originPosX = xBoard;
    }

    public override void MovePlate()
    {
        P_MovePlate();
    }

    private void P_MovePlate()
    {
        if (player == "white")
        {
            PawnMovePlate(xBoard, yBoard + 1);
        }

        else
        {
            PawnMovePlate(xBoard, yBoard - 1);
        }
    }
    public void PawnMovePlate(int x, int y)
    {
        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            if (ChessManager.Inst.GetPosition(x, y) == null)
            {
                if (moveCnt != 0)
                    GameManager.Inst.MovePlateSpawn(x, y, this);

                else
                {
                    if (player == "white")
                    {
                        GameManager.Inst.MovePlateSpawn(x, y, this);
                        if (ChessManager.Inst.GetPosition(x, y + 1) == null)
                            GameManager.Inst.MovePlateSpawn(x, y + 1, this);
                    }

                    else if (player == "black")
                    {
                        GameManager.Inst.MovePlateSpawn(x, y, this);
                        if (ChessManager.Inst.GetPosition(x, y - 1) == null)
                            GameManager.Inst.MovePlateSpawn(x, y - 1, this);
                    }
                }
            }

            if (ChessManager.Inst.PositionOnBoard(x + 1, y) && ChessManager.Inst.GetPosition(x + 1, y) != null &&
               ChessManager.Inst.GetPosition(x + 1, y).GetComponent<ChessBase>().player != player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x + 1, y, this);
            }

            if (ChessManager.Inst.PositionOnBoard(x - 1, y) && ChessManager.Inst.GetPosition(x - 1, y) != null &&
                ChessManager.Inst.GetPosition(x - 1, y).GetComponent<ChessBase>().player != player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x - 1, y, this);
            }
        }

        if (!isEnpassant)
        {
            if (moveCnt == 2 && xBoard == originPosX && yBoard == 4)
            {
                isEnpassant = true;
                if (player == "white") y -= 1;
                else if (player == "black") y += 1;

                if (ChessManager.Inst.PositionOnBoard(x + 1, y) && ChessManager.Inst.GetPosition(x + 1, y) != null &&
               ChessManager.Inst.GetPosition(x + 1, y).GetComponent<ChessBase>().player != player)
                {
                    GameManager.Inst.MovePlateAttackSpawn(x + 1, y, this);
                }

                if (ChessManager.Inst.PositionOnBoard(x - 1, y) && ChessManager.Inst.GetPosition(x - 1, y) != null &&
               ChessManager.Inst.GetPosition(x - 1, y).GetComponent<ChessBase>().player != player)
                {
                    GameManager.Inst.MovePlateAttackSpawn(x + 1, y, this);
                }
            }
        }
    }
}
