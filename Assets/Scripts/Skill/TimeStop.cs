using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : SkillBase
{
    public override void UsingSkill()
    {
        Inv_UsingSkill();
    }

    public override void ResetSkill()
    {
        if (player == GameManager.Inst.GetCurrentPlayer())
            ChessManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);

        else
            ChessManager.Inst.SetPositionEmpty(posX, posY);

        if (turnCnt > 2)
        {

            if (selectPiece != null)
            {
                selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
                SkillManager.Inst.RemoveDontClickPiece(selectPiece);
                selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
                ChessManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
            }

            SkillManager.Inst.RemoveSkillList(this);
            selectPiece.RemoveChosenSkill(this);
            Destroy(gameObject);
        }
    }

    private void Inv_UsingSkill()
    {
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(225, 123, 0, 225));

        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();

        SkillManager.Inst.AddDontClickPiece(selectPiece);

        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;

        if (selectPiece.player == GameManager.Inst.GetCurrentPlayer())
        {
            ChessManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
        }

        else
        {
            ChessManager.Inst.SetPositionEmpty(posX, posY);
        }
    }
}
