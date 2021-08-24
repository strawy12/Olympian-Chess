using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetFriend : SkillBase
{
    public override void UsingSkill()
    {
        photonView.RPC("SF_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void SF_UsingSkill()
    {
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(129, 0, 1, 0));
    }

    public override void StandardSkill()
    {
        ChessBase attacker = ChessManager.Inst.GetPosition(skillData.posX, skillData.posY);

        if (attacker.name.Contains("king"))
        {
            photonView.RPC("RemoveSkill", Photon.Pun.RpcTarget.AllBuffered);
            return;
        }
        ChessManager.Inst.DestroyChessPiece(attacker.GetChessData());
        GameManager.Inst.DestroyMovePlates();

        photonView.RPC("RemoveSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            selectPiece.SetAttackSelecting(false);
        }
        DestroySkill();
    }
}
