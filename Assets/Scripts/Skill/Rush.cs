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
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();

        //if color of selected piece is white,
        //selected piece moves up one space
        if (selectPiece.GetChessData().player == "white")
        {
            if (ChessManager.Inst.GetPosition(posX, posY + 1) == null)
            {
                ChessManager.Inst.MoveChessPiece(selectPiece, posX, posY + 1);
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
            //if color of selected piece is black,
            //selected piece moves down one space
            if (ChessManager.Inst.GetPosition(posX, posY - 1) == null)
            {
                ChessManager.Inst.MoveChessPiece(selectPiece, posX, posY - 1);
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
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}
