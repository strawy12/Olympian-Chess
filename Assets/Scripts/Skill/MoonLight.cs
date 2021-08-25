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
        photonView.RPC("ML_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void ML_UsingSkill()
    {
        originX = selectPiece.GetXBoard();
        originY = selectPiece.GetYBoard();

        if(photonView.IsMine)
        {
            selectPiece.spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);
            selectPiece.SetNoneAttack(true);
        }

        else
        {
            selectPiece.spriteRenderer.enabled = false;
            selectPiece.spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);
        }
    }

    public override void ResetSkill()
    {
<<<<<<< HEAD
        if (selectPiece == null)
        {
            photonView.RPC("DestroySkill_RPC", Photon.Pun.RpcTarget.AllBuffered);

            return;
        }
=======
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
>>>>>>> minyoung

        if (originX != selectPiece.GetXBoard() || originY != selectPiece.GetYBoard())
        {
            originX = selectPiece.GetXBoard();
            originY = selectPiece.GetYBoard();
            moveCnt++;
        }

        if (moveCnt >= 2)
        {
            photonView.RPC("DestroySkill_RPC", Photon.Pun.RpcTarget.AllBuffered);
        }
    }

    [Photon.Pun.PunRPC]
    private void DestroySkill_RPC()
    {
        if (selectPiece != null)
        {
            selectPiece.spriteRenderer.enabled = true;
            selectPiece.spriteRenderer.material.color = new Color(0f, 0f, 0f, 0f);
            selectPiece.SetIsSelecting(false);
            selectPiece.SetNoneAttack(false);
            selectPiece.RemoveChosenSkill(this);
        }
        DestroySkill();
    }
}
