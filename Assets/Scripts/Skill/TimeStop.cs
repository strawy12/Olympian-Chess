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
        if (skillData.player == TurnManager.Instance.GetCurrentPlayer())
            ChessManager.Inst.SetChessPiecePosition(skillData.posX, skillData.posY, selectPiece);

        else
            ChessManager.Inst.SetPositionEmpty(skillData.posX, skillData.posY);

        if (skillData.turnCnt > 2)
        {
            if (selectPiece != null)
            {
                selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
                SkillManager.Inst.RemoveDontClickPiece(selectPiece);
                selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
                ChessManager.Inst.SetChessPiecePosition(skillData.posX, skillData.posY, selectPiece);
            }

            SkillManager.Inst.RemoveSkillList(this);
            selectPiece.RemoveChosenSkill(this);
            Destroy(gameObject);
        }
    }

    [Photon.Pun.PunRPC]
    private void Inv_UsingSkill()
    {
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(225, 123, 0, 225));

        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();

        SkillManager.Inst.AddDontClickPiece(selectPiece);

        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;

        if (selectPiece.GetPlayer() == TurnManager.Instance.GetCurrentPlayer())
        {
            ChessManager.Inst.SetChessPiecePosition(skillData.posX, skillData.posY, selectPiece);
        }

        else
        {
            ChessManager.Inst.SetPositionEmpty(skillData.posX, skillData.posY);
        }
    }
}
