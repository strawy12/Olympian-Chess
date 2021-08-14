using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : SkillBase
{
    public override void UsingSkill()
    {
        Inv_UsingSkill();
    }

    public override void ResetSkill()
    {
        if(turnCnt > 2)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);

            if (selectPiece != null)
            {
                selectPiece.RemoveChosenSkill(this);

                selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(225, 123, 0, 225));
                SkillManager.Inst.AddDontClickPiece(selectPiece);
                selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
            }

            SkillManager.Inst.RemoveSkillList(this);
            ChessManager.Inst.SetPositionEmpty(posX, posY);
            Destroy(gameObject);
        }
    }

    private void Inv_UsingSkill()
    {
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetXBoard();

        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(225, 123, 0, 225));
        SkillManager.Inst.AddDontClickPiece(selectPiece);

        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
        ChessManager.Inst.SetPositionEmpty(posX, posY);
    }
}
