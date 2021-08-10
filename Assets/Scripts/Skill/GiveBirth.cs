using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBirth : SkillBase
{
    public override void UsingSkill()
    {
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.color = new Color32(71, 200, 62, 225);
    }

    public override void StandardSkill()
    {
        ChessBase baby;

        if (selectPiece.player == "white")
        {
            if (ChessManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1) == null)
            {
                movePlate.SetCoords(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1);
            }
            else
            {
                movePlate.SetCoords(posX, posY);
            }

            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetWhiteObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
            baby.transform.Rotate(0f, 0f, 180f);
        }
        else
        {
            if (ChessManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1) == null)
            {
                movePlate.SetCoords(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1);
            }
            else
            {
                Debug.Log("sdf");
                movePlate.SetCoords(posX, posY);
            }

            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetBlackObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
        }

        ChessManager.Inst.SetPosition(baby);
        ChessManager.Inst.AddArr(baby);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }

        selectPiece.SetAttackSelecting(false);
        SkillManager.Inst.RemoveSkillList(this);

        Destroy(gameObject);
    }

}