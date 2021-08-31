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
        photonView.RPC("WB_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    private void WB_UsingSkill()
    {
        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetUsingSkill(true);
        selectPiece.SetNoneAttack(true);
        GetOtherPieces();
        DontMoveOthercp(true);

        selectPiece.spriteRenderer.material.SetColor("_Color", new Color(1, 0, 0, 0));

        if (selectPiece.GetIsMoving())
        {
            cnt++;
            TurnManager.Instance.ButtonActive();
            ChessManager.Inst.SetIsMoving(false);
        }
    }

    private void WB_Standard()
    {
        if (cnt == 0)
        {
            cnt++;
            TurnManager.Instance.ButtonActive();
            ChessManager.Inst.SetIsMoving(false);
            return;
        }
        else
        {
            TurnManager.Instance.ButtonActive();
        }
    }

    [Photon.Pun.PunRPC]
    private void WB_ResetSkill()
    {
        DontMoveOthercp(false);
        selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
        StartCoroutine(WB_SkillEffect());
    }

    private IEnumerator WB_SkillEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(255, 0, 0, 0));
            yield return new WaitForSeconds(0.1f);
            selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
            yield return new WaitForSeconds(0.1f);
        }

        ChessManager.Inst.SetPositionEmpty(selectPiece.GetXBoard(), selectPiece.GetYBoard());
        ChessManager.Inst.UpdateArr(selectPiece);
        Destroy(selectPiece.gameObject);
        selectPiece.RemoveChosenSkill(this);

        DestroySkill();
    }

    private void DontMoveOthercp(bool isAdd)
    {
        for (int i = 0; i < chessPieces.Count; i++)
        {
            if (isAdd)
            {
                SkillManager.Inst.AddDontClickPiece(chessPieces[i], true);
            }

            if (!isAdd)
            {
                SkillManager.Inst.RemoveDontClickPiece(chessPieces[i], true);
            }
        }
    }

    private void GetOtherPieces()
    {
        ChessBase[] white = ChessManager.Inst.GetPlayerWhite();
        ChessBase[] black = ChessManager.Inst.GetPlayerBlack();

        for (int i = 0; i < white.Length; i++)
        {
            if (white[i] == null) continue;

            if (!SkillManager.Inst.dontClickPiece.Contains(white[i].GetChessData()) && white[i] != selectPiece)
            {
                chessPieces.Add(white[i]);
            }
        }

        for (int i = 0; i < black.Length; i++)
        {
            if (black[i] == null) continue;
            if (!SkillManager.Inst.dontClickPiece.Contains(black[i].GetChessData()) && black[i] != selectPiece)
            {
                chessPieces.Add(black[i]);
            }
        }
    }
}
