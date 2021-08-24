using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offering : SkillBase
{
    public override void UsingSkill()
    {
        OF_UsingSkill();
    }

    private void OF_UsingSkill()
    {
        var targetCards = skillData.player == "white" ? CardManager.Inst.GetBlackCards() : CardManager.Inst.GetWhiteCards();
        //Preventing Pawns from being the target of Offering
        if (selectPiece.name == "black_pawn" || selectPiece.name == "white_pawn" || targetCards.Count == 0)
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }

        int rand;
        rand = Random.Range(0, targetCards.Count);
        CardManager.Inst.RemoveCard(rand, targetCards);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        photonView.RPC("DestroySkill", Photon.Pun.RpcTarget.AllBuffered);

    }
}