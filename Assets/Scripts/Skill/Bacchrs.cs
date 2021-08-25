using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bacchrs : SkillBase
{
    ChessBase[] white;
    ChessBase[] black;

    public override void UsingSkill()
    {
        photonView.RPC("BC_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void BC_UsingSkill()
    {
        GameManager.Inst.isBacchrs = true;
        ChosenSkill(true);
    }

    public override void ResetSkill()
    {
        if (skillData.turnCnt < 2) return;
        photonView.RPC("BC_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void BC_ResetSkill()
    {
        Debug.Log("ÀÀ¾Ö");
        ChosenSkill(false);
        DestroySkill();
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
        ChessManager.Inst.PointMovePlate(x, y, cp);
    }

    private void ChosenSkill(bool isAdd)
    {
        white = ChessManager.Inst.GetPlayerWhite();
        black = ChessManager.Inst.GetPlayerBlack();

        for (int i = 0; i < white.Length; i++)
        {
            if (white[i] == null) continue;
            if (isAdd)
            {
                white[i].AddChosenSkill(this);
            }

            if (!isAdd)
            {

                white[i].RemoveChosenSkill(this);
            }
        }

        for (int i = 0; i < black.Length; i++)
        {
            if (black[i] == null) continue;

            if (isAdd)
            {
                black[i].AddChosenSkill(this);
            }

            if (!isAdd)
            {

                black[i].RemoveChosenSkill(this);
            }
        }
    }
}