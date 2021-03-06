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
            if (skillData.posY == 7)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY + 1) == null)
            {
                ChessManager.Inst.MoveChessPiece(selectPiece, skillData.posX, skillData.posY + 1);
            }

            else
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }
        }
        else
        {
            if (skillData.posY == 0)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY - 1) == null)
            {
                ChessManager.Inst.MoveChessPiece(selectPiece, skillData.posX, skillData.posY - 1);
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
}
