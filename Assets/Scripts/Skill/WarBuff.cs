using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBuff : SkillBase
{
    private int cnt = 0;
    private int turn = 0;
    public override void UsingSkill()
    {
        WB_UsingSkill();
    }

    public override void ResetSkill()
    {
        WB_Standard();
    }

    private void WB_UsingSkill()
    {
        if (selectPiece.isMoving)
        {
            TurnManager.Instance.SetIsActive(false);
        }
        turn = 1;
    }

    private void WB_Standard()
    {
        if(turn == turnCnt)
        {
            if (cnt == 0 && !selectPiece.isMoving)
            {
                cnt++;
                TurnManager.Instance.SetIsActive(false);
                return;
            }

            else
            {
                TurnManager.Instance.ButtonColor();
                ChessManager.Inst.SetPositionEmpty(selectPiece.GetXBoard(), selectPiece.GetYBoard());
                //ChessManager.Inst.UpdateArr(selectPiece);
                Destroy(selectPiece.gameObject);
                WB_ResetSkill();
            }
        }
    }

    private void WB_ResetSkill()
    {
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }



}
