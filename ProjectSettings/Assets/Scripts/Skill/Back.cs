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
        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();

        //if color of selected piece is white,
        //selected piece moves up one space
        if (selectPiece.GetPlayer() == "white")
        {
            if(skillData.posY == 0)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY - 1) == null)
            {
                MoveChessPiece(selectPiece, skillData.posX, skillData.posY - 1);
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
            if (skillData.posY == 7)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            //if color of selected piece is black,
            //selected piece moves down one space
            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY + 1) == null)
            {
                MoveChessPiece(selectPiece, skillData.posX, skillData.posY + 1);
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

    private void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        ChessManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        ChessManager.Inst.SetPosition(cp);
        cp.SetCoordsAnimation();
        GameManager.Inst.DestroyMovePlates();
    }
}
