using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : SkillBase
{
    public override void UsingSkill()
    {
        photonView.RPC("Inv_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
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
                photonView.RPC("TW_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
            }
        }
    }

    [Photon.Pun.PunRPC]
    private void TW_ResetSkill()
    {
        selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        ChessManager.Inst.SetChessPiecePosition(skillData.posX, skillData.posY, selectPiece);
        selectPiece.RemoveChosenSkill(this);
        DestroySkill();
    }

    [Photon.Pun.PunRPC]
    private void Inv_UsingSkill()
    {
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(225, 123, 0, 225));

        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();

        SkillManager.Inst.AddDontClickPiece(selectPiece);
        ChessManager.Inst.SetChessPiecePosition(skillData.posX, skillData.posY, selectPiece);
    }
}
