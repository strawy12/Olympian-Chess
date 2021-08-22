using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : SkillBase
{
    int originX;
    int originY;
    int moveCnt = 0;

    public override void UsingSkill()
    {
        ML_UsingSkill();
    }

    private void ML_UsingSkill()
    {
        originX = selectPiece.GetXBoard();
        originY = selectPiece.GetYBoard();

        selectPiece.spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);
        selectPiece.SetNoneAttack(true);
    }

    public override void ResetSkill()
    {
        if (moveCnt < 2)
        {
            if(selectPiece == null)
            {
                DestroySkill();
                return;
            }

            if(originX != selectPiece.GetXBoard() || originY != selectPiece.GetYBoard())
            {
                originX = selectPiece.GetXBoard();
                originY = selectPiece.GetYBoard();
                moveCnt++;
            }

            if (TurnManager.Instance.CheckPlayer(skillData.player))
            {
                selectPiece.spriteRenderer.enabled = false;
                selectPiece.spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);
            }

            else
                selectPiece.spriteRenderer.enabled = true;
        }

        else
        {
            selectPiece.spriteRenderer.enabled = true;
            selectPiece.spriteRenderer.material.color = new Color(0f, 0f, 0f, 0f);
            selectPiece.SetIsSelecting(false);
            DestroySkill();
        }
    }

    private void DestroySkill()
    {
        SkillManager.Inst.RemoveSkillList(this);
        if (selectPiece != null)
            selectPiece.RemoveChosenSkill(this);

        Destroy(gameObject);
    }
}
