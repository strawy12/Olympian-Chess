using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : SkillBase
{

    public override void UsingSkill()
    {
        Back_UsingSkill();
    }

    private void Back_UsingSkill()
    {
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();
        Debug.Log(posY);

        //if color of selected piece is white,
        //selected piece moves up one space
        if (selectPiece.player == "white")
        {
            if(posY == 0)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

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
        else
        {
            if (posY == 7)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            //if color of selected piece is black,
            //selected piece moves down one space
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
