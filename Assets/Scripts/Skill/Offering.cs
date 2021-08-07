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
        //Preventing Pawns from being the target of Offering
        if (selectPiece.name == "black_pawn" || selectPiece.name == "white_pawn" || CardManager.Inst.GetOtherCards().Count == 0)
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }

        int rand;
        rand = Random.Range(0, CardManager.Inst.GetOtherCards().Count);
        CardManager.Inst.RemoveCard(rand);
        Destroy(selectPiece.gameObject);
        SkillManager.Inst.RemoveSkillList(this);
    }
}