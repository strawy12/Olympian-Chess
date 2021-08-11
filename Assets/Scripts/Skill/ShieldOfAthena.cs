using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOfAthena : SkillBase
{

    private bool isAttack = false;
    private int moveCnt = 0;

    public override void UsingSkill()
    {
        SOA_UsingSkill();
    }
    public override void StandardSkill()
    {
        CheckSOA();
    }
    public override void ResetSkill()
    {
        SOA_ResetSkill();
    }

    private void SOA_UsingSkill()
    {
        if (selectPiece.name.Contains("king"))
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }
        moveCnt = selectPiece.GetMoveCnt();
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 1, 0));
    }

    private void CheckSOA()
    {
        isAttack = true;
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(false);
        GameManager.Inst.SetIsStop(true);
        GameManager.Inst.DestroyMovePlates();
        TurnManager.Instance.ButtonColor();
        SOA_ResetSkill();
    }

    public void SOA_ResetSkill()
    {
        if (selectPiece.GetMoveCnt() != moveCnt || isAttack)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
            SkillManager.Inst.RemoveSkillList(this);
            selectPiece.SetAttackSelecting(false);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            if (selectPiece != null)
            {
                selectPiece.RemoveChosenSkill(this);
            }
            Destroy(gameObject);
        }
    }
}
