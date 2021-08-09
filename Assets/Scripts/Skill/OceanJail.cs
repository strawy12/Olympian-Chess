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
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();
        selectPiece.spriteRenderer.sortingOrder = -2;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
        GameManager.Inst.SetPositionEmpty(posX, posY);
        CardManager.Inst.SetisBreak(false);
        turn = turnCnt + 2;
    }

    public override void ResetSkill()
    {
        if (turn > turnCnt) return;
        selectPiece.spriteRenderer.sortingOrder = 0;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
        if (GameManager.Inst.GetPosition(posX, posY) != null)
        {
            Destroy(GameManager.Inst.GetPosition(posX, posY).gameObject);
            GameManager.Inst.SetPositionEmpty(posX, posY);
        }
        GameManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}