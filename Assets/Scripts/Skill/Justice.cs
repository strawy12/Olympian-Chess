using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Justice : SkillBase
{
    List<ChessBase> attacking = GameManager.Inst.GetAttackings();
    List<ChessBase> justiceCP = new List<ChessBase>();

    public override void UsingSkill()
    {
        photonView.RPC("JT_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void JT_UsingSkill()
    {
        for (int i = 0; i < attacking.Count; i++)
        {
            if (attacking[i] == null) continue;
            justiceCP.Add(attacking[i]);
            SkillManager.Inst.AddDontClickPiece(attacking[i]);
            attacking[i].spriteRenderer.material.color = new Color32(70, 60, 0, 0);
        }
    }
    public override void ResetSkill()
    {
        if (selectPiece == null)
        {
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            RPC_DestroySkill();
        }

        if (skillData.turnCnt > 2)
        {
            for (int i = 0; i < justiceCP.Count; i++)
            {
                if(justiceCP[i] == null) continue;

                justiceCP[i].spriteRenderer.material.color = new Color32(0, 0, 0, 0);
                SkillManager.Inst.RemoveDontClickPiece(justiceCP[i]);
            }

            if (selectPiece != null)
            {
                selectPiece.RemoveChosenSkill(this);
            }

            RPC_DestroySkill();
        }
    }
}