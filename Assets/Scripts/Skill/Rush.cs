using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : SkillBase
{
    public override void UsingSkill()
    {
        RS_UsingSkill();
    }

    private void RS_UsingSkill()
    {
        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();

        //if color of selected piece is white,
        //selected piece moves up one space
        if (selectPiece.GetChessData().player == "white")
        {
<<<<<<< HEAD
            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY + 1) == null)
            {
                ChessManager.Inst.MoveChessPiece(selectPiece, skillData.posX, skillData.posY + 1);
=======
            if (posY == 7)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            if (ChessManager.Inst.GetPosition(posX, posY + 1) == null)
            {
                MoveChessPiece(selectPiece, posX, posY + 1);
>>>>>>> minyoung
            }

            // if the space to go is not empty, Use of the card is canceled.
            else
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }
        }
        else
        {
            if (posY == 0)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            //selected piece moves down one space
            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY - 1) == null)
            {
<<<<<<< HEAD
                ChessManager.Inst.MoveChessPiece(selectPiece, skillData.posX, skillData.posY - 1);
=======
                MoveChessPiece(selectPiece, posX, posY - 1);
>>>>>>> minyoung
            }

            // if the space to go is not empty, Use of the card is canceled.
            else
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }
        }

        RemoveSkill();
    }

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        RPC_DestroySkill();
    }

    private void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        ChessManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        ChessManager.Inst.SetPosition(cp);
        StartCoroutine(ChessManager.Inst.SetCoordsAnimation(cp));
        GameManager.Inst.DestroyMovePlates();
    }
}
