using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offering : SkillBase
{
    public override void UsingSkill()
    {
        StartCoroutine(OF_UsingSkill());
    }

    private IEnumerator OF_UsingSkill()
    {
        var targetCards = skillData.player == "white" ? CardManager.Inst.GetBlackCards() : CardManager.Inst.GetWhiteCards();
        //Preventing Pawns from being the target of Offering
        if (selectPiece.name.Contains("pawn") || selectPiece.name.Contains("king") || targetCards.Count == 0)
        {
            RemoveSkill();
            CardManager.Inst.SetisBreak(true);
            yield break;
        }

        photonView.RPC("OF_Effect", Photon.Pun.RpcTarget.AllBuffered);
        yield return new WaitForSeconds(1f);
        ChessManager.Inst.DestroyChessPiece(selectPiece.GetChessData());

        int rand;
        rand = Random.Range(0, targetCards.Count);
        CardManager.Inst.RemoveCard(rand, targetCards);
        RPC_DestroySkill();
    }

    [Photon.Pun.PunRPC]
    private void OF_Effect()
    {
        base.StartEffect();
        animator.transform.localScale = new Vector3(3f, 3f, 3f);
        animator.Play("OF_Anim");
    }

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        RPC_DestroySkill();
    }
}