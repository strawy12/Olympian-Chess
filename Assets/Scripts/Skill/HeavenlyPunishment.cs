using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenlyPunishment : SkillBase
{

    private bool isBreak = false;

    public override void UsingSkill()
    {
        HP_UsingSkill()
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
            if (GameManager.Inst.CheckPlayer("white"))
                isBreak = GameManager.Inst.CheckArr(false, "black_rook");
            else
                isBreak = GameManager.Inst.CheckArr(true, "white_rook");
            if ()
            {
                CardManager.Inst.SetisBreak(isBreak);
                return;
            }

        }
        StartCoroutine(HP_SkillEffect());
        CardManager.Inst.SetisBreak(false);
        isUsingCard = false;
        SkillManager.Inst.SetDontClickPiece(selectPiece);
    }

    private IEnumerator HP_SkillEffect()
    {
        int k = turnCnt + 2;
        //sparkling effect (yellow)
        while (GetTurnTime() < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }
        // When card time is over, selected pieces turn to original color
        selectPiece = null;
        SkillManager.Inst.RemoveSkillList(gameObject.);
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        Destroy(gameObject); 
    }
}
