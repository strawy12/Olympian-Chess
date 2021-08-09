using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBirth : SkillBase
{
    public override void UsingSkill()
    {
        selectPiece.SetIsAttackSelecting(true);
        selectPiece.AddChosenSkill(this);
        selectPiece.spriteRenderer.material.color = new Color32(71, 200, 62, 225);
    }

    public override void StandardSkill()
    {
        Chessman baby;

        if (selectPiece.player == "white")
        {
            if (GameManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1) == null)
            {
                movePlate.SetCoords(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1);
            }
            else
            {
                movePlate.SetCoords(posX, posY);
            }

            baby = GameManager.Inst.Creat("white_pawn", selectPiece.GetXBoard(), selectPiece.GetYBoard());
            baby.transform.Rotate(0f, 0f, 180f);
        }
        else
        {
            if (GameManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1) == null)
            {
                movePlate.SetCoords(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1);
            }
            else
            {
                Debug.Log("sdf");
                movePlate.SetCoords(posX, posY);
            }

            baby = GameManager.Inst.Creat("black_pawn", selectPiece.GetXBoard(), selectPiece.GetYBoard());
        }

        GameManager.Inst.SetPosition(baby);
        GameManager.Inst.AddArr(baby);

        selectPiece.SetIsAttackSelecting(false);
        selectPiece.RemoveChosenSkill(this);

        Destroy(gameObject);
        SkillManager.Inst.RemoveSkillList(this);
    }

}