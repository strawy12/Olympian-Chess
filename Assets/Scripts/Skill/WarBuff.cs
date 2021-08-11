using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBuff : SkillBase
{
    private int cnt = 0;
    private List<ChessBase> chessPieces = new List<ChessBase>();

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
        GetOtherPieces();
        DontMoveOthercp(true);

        selectPiece.spriteRenderer.material.SetColor("_Color", new Color(1, 0, 0, 0));

        if (selectPiece.isMoving)
        {
            cnt++;
            TurnManager.Instance.ButtonInactive();
        }
    }

    private void WB_Standard()
    {
        if (cnt == 0)
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
        DontMoveOthercp(false);
        selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
        StartCoroutine(WB_SkillEffect());
    }

    private IEnumerator WB_SkillEffect()
    {
        for (int i = 0; i < 7; i++)
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

        ChessManager.Inst.SetPositionEmpty(selectPiece.GetXBoard(), selectPiece.GetYBoard());
        ChessManager.Inst.UpdateArr(selectPiece);
        Destroy(selectPiece.gameObject);
        SkillManager.Inst.RemoveSkillList(this);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }

        Destroy(gameObject);
    }

    private void DontMoveOthercp(bool isAdd)
    {
        for(int i = 0; i< chessPieces.Count; i++)
        {
            if(isAdd)
            {
                SkillManager.Inst.AddDontClickPiece(chessPieces[i]);
            }

            if(!isAdd)
            {
                SkillManager.Inst.RemoveDontClickPiece(chessPieces[i]);
            }
        }
    }

    private void GetOtherPieces()
    {
        ChessBase[] white = ChessManager.Inst.GetPlayerWhite();
        ChessBase[] black = ChessManager.Inst.GetPlayerBlack();

        for (int i = 0; i< white.Length; i++)
        {
            if(!SkillManager.Inst.dontClickPiece.Contains(white[i]) && white[i] != selectPiece)
            {
                if (white[i] == null) continue;
                chessPieces.Add(white[i]);
            }
        }

        for (int i = 0; i < black.Length; i++)
        {
            if (!SkillManager.Inst.dontClickPiece.Contains(black[i]) && black[i] != selectPiece)
            {
                if (black[i] == null) continue;
                chessPieces.Add(black[i]);
            }
        }
    }
}
