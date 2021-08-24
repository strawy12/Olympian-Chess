using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : SkillBase
{
    int originX;
    int originY;
    int moveCnt = 0;
    int maxMove = 2;

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
        if (selectPiece.isAttacking && selectPiece.attackCount == 1 && maxMove - moveCnt == 1)
        {
            maxMove += 2;
        }

        if (moveCnt < maxMove)
        {
            if (selectPiece == null)
            {
                DestroySkill();
                return;
            }

            if (originX != selectPiece.GetXBoard() || originY != selectPiece.GetYBoard())
            {
                originX = selectPiece.GetXBoard();
                originY = selectPiece.GetYBoard();
                moveCnt++;
            }

            if (GetPlayer() != GameManager.Inst.GetCurrentPlayer())
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
