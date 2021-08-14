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
        StartCoroutine(HP_SkillEffect());
        CardManager.Inst.SetisBreak(false);
        SkillManager.Inst.AddDontClickPiece(selectPiece);
    }

    private IEnumerator HP_SkillEffect()
    {
        int k = turnCnt + 2;
        //sparkling effect (yellow)
        while (turnCnt < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }
        // When card time is over, selected pieces turn to original color

        SkillManager.Inst.RemoveSkillList(this);
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }

        Destroy(gameObject); 
    }
}
