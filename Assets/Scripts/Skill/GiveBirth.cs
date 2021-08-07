using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBirth : SkillBase
{
    public override void UsingSkill()
    {
        selectPiece.SetIsAttackSelecting(true);
        selectPiece.spriteRenderer.material.color = new Color32(71, 200, 62, 225);
    }

    public override void StandardSkill()
    {
        Chessman cm;
        Chessman attacker;

        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();

        GameManager.Inst.SetPositionEmpty(selectPiece.GetXBoard(), selectPiece.GetYBoard());

        if (selectPiece.player == "white")
        {
            attacker = GameManager.Inst.GetPosition(posX, posY);
            attacker.SetYBoard(posY + 1);
            GameManager.Inst.SetPosition(attacker);

            cm = GameManager.Inst.Creat("white_pawn", selectPiece.GetXBoard(), selectPiece.GetYBoard());
            cm.transform.Rotate(0f, 0f, 180f);
            GameManager.Inst.SetPosition(cm);
        }
        else
        {
            attacker = GameManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard());
            attacker.SetYBoard(selectPiece.GetYBoard() - 1);
            GameManager.Inst.SetPosition(attacker);

            cm = GameManager.Inst.Creat("black_pawn", selectPiece.GetXBoard(), selectPiece.GetYBoard());
            GameManager.Inst.SetPosition(cm);
        }

        Destroy(gameObject);
        Destroy(selectPiece.gameObject);
        SkillManager.Inst.RemoveSkillList(this);
        GameManager.Inst.UpdateArr(selectPiece);
        GameManager.Inst.AddArr(cm);
    }
}