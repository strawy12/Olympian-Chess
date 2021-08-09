using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacchrs : SkillBase
{
    public override void UsingSkill()
    {
        GameManager.Inst.isBacchrs = true;
    }

    public override void ResetSkill()
    {
        if (turnCnt > 1)
        {
            GameManager.Inst.isBacchrs = false;
        }
    }

    public override void StandardSkill()
    {
        InitiateMovePlates_Bacchrs();
    }

    public void InitiateMovePlates_Bacchrs()
    {
        switch (selectPiece.name)
        {
            case "black_queen":
            case "white_queen":
            case "black_king":
            case "white_king":
                selectPiece.SurroundMovePlate();
                break;

            case "black_bishop":
            case "white_bishop":
                selectPiece.PointMovePlate(selectPiece.GetXBoard() + 1, selectPiece.GetYBoard() + 1);
                selectPiece.PointMovePlate(selectPiece.GetXBoard() + 1, selectPiece.GetYBoard() - 1);
                selectPiece.PointMovePlate(selectPiece.GetXBoard() - 1, selectPiece.GetYBoard() + 1);
                selectPiece.PointMovePlate(selectPiece.GetXBoard() - 1, selectPiece.GetYBoard() - 1);
                break;

            case "black_knight":
            case "white_knight":
            case "black_rook":
            case "white_rook":
                selectPiece.PointMovePlate(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1);
                selectPiece.PointMovePlate(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1);
                selectPiece.PointMovePlate(selectPiece.GetXBoard() - 1, selectPiece.GetYBoard());
                selectPiece.PointMovePlate(selectPiece.GetXBoard() + 1, selectPiece.GetYBoard());
                break;

            case "black_pawn":
                selectPiece.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1);
                break;

            case "white_pawn":
                selectPiece.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1);
                break;
        }
    }
}