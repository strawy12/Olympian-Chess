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
        PawnMovePlate(xBoard, yBoard + 1);
    }
    public void PawnMovePlate(int x, int y)
    {
        //if (OD_PawnMovePlate(x, y))
        //    return;


        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            if (ChessManager.Inst.GetPosition(x, y) == null)
            {
                if (moveCnt != 0)
                    GameManager.Inst.MovePlateSpawn(x, y, this);

                else
                {
                    GameManager.Inst.MovePlateSpawn(x, y, this);
                    if (ChessManager.Inst.GetPosition(x, y + 1) == null)
                        GameManager.Inst.MovePlateSpawn(x, y + 1, this);
                    //if (player == "white")
                    //{
                    //    ChessManager.Inst.MovePlateSpawn(this, x, y);
                    //    if (GameManager.Inst.GetPosition(x, y + 1) == null)
                    //        ChessManager.Inst.MovePlateSpawn(this, x, y + 1);
                    //}

                    //else if (player == "black")
                    //{
                    //    ChessManager.Inst.MovePlateSpawn(this, x, y);
                    //    if (GameManager.Inst.GetPosition(x, y - 1) == null)
                    //        ChessManager.Inst.MovePlateSpawn(this, x, y - 1);
                    //}
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
    }
}
