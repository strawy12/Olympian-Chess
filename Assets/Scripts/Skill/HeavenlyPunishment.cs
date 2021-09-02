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
        if (selectPiece.name.Contains("king"))
        {
            CardManager.Inst.SetisBreak(true);
            RemoveSkill();
            return;
        }

        // if the opposing team has a rook or rooks,
        //Preventing Queen from being the target of HeavenlyPunishment
        if (selectPiece.name.Contains("queen"))
        {
            if (TurnManager.Instance.CheckPlayer("white"))
            {
                isBreak = ChessManager.Inst.CheckArr(false, "black_rook");

            }

            else
            {
                isBreak = ChessManager.Inst.CheckArr(true, "white_rook");
            }
        }
        CardManager.Inst.SetisBreak(isBreak);

        if (isBreak)
        {
            RemoveSkill();
            return;
        }
        photonView.RPC("StartHP_Effect", Photon.Pun.RpcTarget.AllBuffered);
        CardManager.Inst.SetisBreak(false);
    }

    public override void ResetSkill()
    {
        if (skillData.turnCnt < 2) return;
        RemoveSkill();
    }

    [Photon.Pun.PunRPC]
    public void StartHP_Effect()
    {
        SkillManager.Inst.AddDontClickPiece(selectPiece);

        base.StartEffect();

        animator.Play("HP_Anim");
    }



    private IEnumerator HP_SkillEffect()
    {
        int k = skillData.turnCnt + 2;
        Debug.Log(skillData.turnCnt);
        Debug.Log(k);
        //sparkling effect (yellow)
        while (2 < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }

    }

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        }

        DestroySkill();
    }
}
