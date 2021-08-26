using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : SkillBase
{
    int random;
    public override void UsingSkill()
    {
        random = Random.Range(0, 2);
    }

    public override void StandardSkill()
    {
        InitiateMovePlates_Dice();
    }

    public void InitiateMovePlates_Dice()
    {
        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();

        if((skillData.posX + skillData.posY) % 2 == random)

        switch (selectPiece.GetChessData().chessPiece)
        {
            case "black_queen":
            case "white_queen":


            case "black_king":
            case "white_king":
                SurroundMovePlate(selectPiece);
                break;

            case "black_bishop":
            case "white_bishop":
                PointMovePlate(selectPiece.GetXBoard() + 1, selectPiece.GetYBoard() + 1, selectPiece);
                PointMovePlate(selectPiece.GetXBoard() + 1, selectPiece.GetYBoard() - 1, selectPiece);
                PointMovePlate(selectPiece.GetXBoard() - 1, selectPiece.GetYBoard() + 1, selectPiece);
                PointMovePlate(selectPiece.GetXBoard() - 1, selectPiece.GetYBoard() - 1, selectPiece);
                break;

            case "black_knight":
            case "white_knight":
            case "black_rook":
            case "white_rook":
                PointMovePlate(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1, selectPiece);
                PointMovePlate(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1, selectPiece);
                PointMovePlate(selectPiece.GetXBoard() - 1, selectPiece.GetYBoard(), selectPiece);
                PointMovePlate(selectPiece.GetXBoard() + 1, selectPiece.GetYBoard(), selectPiece);
                break;

            case "black_pawn":
                GameManager.Inst.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1, selectPiece);
                break;

            case "white_pawn":
                GameManager.Inst.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1, selectPiece);
                break;
        }
    }

    private void SurroundMovePlate(ChessBase cp)
    {
        int xBoard = selectPiece.GetXBoard();
        int yBoard = selectPiece.GetYBoard();

        PointMovePlate(xBoard, yBoard + 1, cp);
        PointMovePlate(xBoard, yBoard - 1, cp);
        PointMovePlate(xBoard - 1, yBoard - 1, cp);
        PointMovePlate(xBoard - 1, yBoard - 0, cp);
        PointMovePlate(xBoard - 1, yBoard + 1, cp);
        PointMovePlate(xBoard + 1, yBoard - 1, cp);
        PointMovePlate(xBoard + 1, yBoard - 0, cp);
        PointMovePlate(xBoard + 1, yBoard + 1, cp);
    }

    private void PointMovePlate(int x, int y, ChessBase cp)
    {
        if((x+y)% 2 == random)
        {
            ChessManager.Inst.PointMovePlate(x, y, cp);
        }
    }
}
