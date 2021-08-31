using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WestWind : SkillBase
{
    int turn;
    public override void UsingSkill()
    {
        photonView.RPC("WW_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }
    public override void ResetSkill()
    {
        photonView.RPC("WW_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void WW_UsingSkill()
    {
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(215, 199, 176, 144));
        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();
        selectPiece.spriteRenderer.sortingOrder = -2;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
        if (photonView.IsMine)
        {
            ChessManager.Inst.SetPositionEmpty(skillData.posX, skillData.posY);
        }
        turn = skillData.turnCnt + 2;
    }

    [Photon.Pun.PunRPC]
    public void WW_ResetSkill()
    {
        if (turn > skillData.turnCnt) return;
        selectPiece.spriteRenderer.sortingOrder = 0;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
        ChessManager.Inst.SetChessPiecePosition(skillData.posX, skillData.posY, selectPiece);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        selectPiece.RemoveChosenSkill(this);

        DestroySkill();
    }
}
