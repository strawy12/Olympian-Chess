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
            if (GameManager.Inst.CheckPlayer("black"))
            {
                isBreak = ChessManager.Inst.CheckArr(false, "black_rook");
            }
            else
            {
                isBreak = ChessManager.Inst.CheckArr(true, "white_rook");
            }

            Debug.Log(isBreak);
            CardManager.Inst.SetisBreak(isBreak);

            if(isBreak)
            {
                RemoveSkill();
                return;
            }
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

        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        RemoveSkill();
    }

    private void RemoveSkill()
    {
        SkillManager.Inst.RemoveSkillList(this);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }

        Destroy(gameObject);

    }
}
