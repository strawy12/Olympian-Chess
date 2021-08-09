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
        int posX;
        int posY;

        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();

        //if color of selected piece is white,
        //selected piece moves up one space
        if (selectPiece.player == "white")
        {
            if (GameManager.Inst.GetPosition(posX, posY + 1) == null)
            {
                selectPiece.SetYBoard(posY + 1);
            }

            // if the space to go is not empty, Use of the card is canceled.
            else
            {
                CardManager.Inst.SetisBreak(true);
                return;
            }
        }
        else
        {
            //if color of selected piece is black,
            //selected piece moves down one space
            if (GameManager.Inst.GetPosition(posX, posY - 1) == null)
            {
                selectPiece.SetYBoard(posY - 1);
            }

            // if the space to go is not empty, Use of the card is canceled.
            else
            {
                CardManager.Inst.SetisBreak(true);
                return;
            }
        }

        GameManager.Inst.SetPositionEmpty(posX, posY);
        selectPiece.SetXBoard(posX);
        selectPiece.SetCoords();
        GameManager.Inst.SetPosition(selectPiece);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}
