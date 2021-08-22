using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanJail : SkillBase
{
    private int turn;
    public override void UsingSkill()
    {
        OJ_UsingSkill();
    }

    private void OJ_UsingSkill()
    {
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 255, 144));
        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();
        selectPiece.spriteRenderer.sortingOrder = -2;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
        ChessManager.Inst.SetPositionEmpty(skillData.posX, skillData.posY);
        CardManager.Inst.SetisBreak(false);
        turn = skillData.turnCnt + 2;
    }

    public override void ResetSkill()
    {
        if (turn > skillData.turnCnt) return;
        selectPiece.spriteRenderer.sortingOrder = 0;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
        if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY) != null)
        {
            Destroy(ChessManager.Inst.GetPosition(skillData.posX, skillData.posY).gameObject);
            ChessManager.Inst.SetPositionEmpty(skillData.posX, skillData.posY);
        }
        //ChessManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        SkillManager.Inst.RemoveSkillList(this);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        Destroy(gameObject);
    }
}
