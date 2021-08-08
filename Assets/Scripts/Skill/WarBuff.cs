using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBuff : SkillBase
{
    private int cnt = 0;
    public override void UsingSkill()
    {
        WB_UsingSkill();
    }

    public override void StandardSkill()
    {
        WB_Standard();
    }
    public override void ResetSkill()
    {
        WB_ResetSkill();
    }

    private void WB_UsingSkill()
    {
        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetUsingSkill(true);
        selectPiece.SetNoneAttack(true);
        if (selectPiece.isMoving)
        {
            TurnManager.Instance.ButtonInactive();
        }
    }

    private void WB_Standard()
    {
        if (cnt == 0 && !selectPiece.isMoving)
        {
            cnt++;
            TurnManager.Instance.ButtonInactive();
            return;
        }
        else
        {
            TurnManager.Instance.ButtonColor();
        }
    }

    private void WB_ResetSkill()
    {
        StartCoroutine(WB_SkillEffect());
        
    }

    private IEnumerator WB_SkillEffect()
    {
        for (int i = 0; i < 10; i++)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(255, 0, 0, 0));
            yield return new WaitForSeconds(0.1f);
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(255, 94, 0, 0));
            yield return new WaitForSeconds(0.1f);
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(255, 228, 0, 0));
            yield return new WaitForSeconds(0.1f);
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(29, 219, 22, 0));
            yield return new WaitForSeconds(0.1f);
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 84, 255, 0));
            yield return new WaitForSeconds(0.1f);
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 183, 0));
            yield return new WaitForSeconds(0.1f);
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(167, 72, 255, 0));
            yield return new WaitForSeconds(0.1f);

        }
        GameManager.Inst.SetPositionEmpty(selectPiece.GetXBoard(), selectPiece.GetYBoard());
        GameManager.Inst.UpdateArr(selectPiece);
        Destroy(selectPiece.gameObject);
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }



}
