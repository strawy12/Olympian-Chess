using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenlyPunishment : SkillBase
{

    private bool isBreak = false;

    public override void UsingSkill()
    {
        HP_UsingSkill();
    }

    private void HP_UsingSkill()
    {
        if (selectPiece.name == "black_king" || selectPiece.name == "white_king")
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }

        // if the opposing team has a rook or rooks,
        //Preventing Queen from being the target of HeavenlyPunishment
        if (selectPiece.name == "black_queen" || selectPiece.name == "white_queen")
        {
            if (TurnManager.Instance.CheckPlayer("white"))
                isBreak = ChessManager.Inst.CheckArr(false, "black_rook");
            else
                isBreak = ChessManager.Inst.CheckArr(true, "white_rook");

            CardManager.Inst.SetisBreak(isBreak);
            return;
        }
        photonView.RPC("StartEffect", Photon.Pun.RpcTarget.AllBuffered);
        CardManager.Inst.SetisBreak(false);
        SkillManager.Inst.AddDontClickPiece(selectPiece);
    }

    [Photon.Pun.PunRPC]
    private void StartEffect()
    {
        StartCoroutine(HP_SkillEffect());
    }


    private IEnumerator HP_SkillEffect()
    {
        int k = skillData.turnCnt + 2;
        //sparkling effect (yellow)
        while (skillData.turnCnt < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }
        // When card time is over, selected pieces turn to original color

        
        

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        }

        photonView.RPC("DestroySkill", Photon.Pun.RpcTarget.AllBuffered);
    }
}
