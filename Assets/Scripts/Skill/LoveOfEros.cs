using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveOfEros : SkillBase
{
    private bool attacked = false;
    private bool isSetting = false;
    public override void UsingSkill()
    {
        LOE_UsingSkill();
    }

    public override void StandardSkill()
    {
        if (!isSetting)
        {
            photonView.RPC("LOE_Setting", Photon.Pun.RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC("LOE_StandardSkill", Photon.Pun.RpcTarget.AllBuffered);
        }
    }

    private void LOE_UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameObject mp = GameManager.Inst.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard(), selectPiece);
        mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(29, 219, 22, 255));
        mp.GetComponent<Collider2D>().enabled = false;
        GameManager.Inst.AllMovePlateSpawn(selectPiece, true);
    }

    [Photon.Pun.PunRPC]
    private void LOE_Setting()
    {
        CardManager.Inst.NotAmolang();
        isSetting = true;
        attacked = false;
        selectPieceTo = ChessManager.Inst.GetPosition(skillData.posX, skillData.posY);

        selectPieceTo.SetAttackSelecting(true);
        selectPiece.SetAttackSelecting(true);

        selectPieceTo.AddChosenSkill(this);
        selectPiece.AddChosenSkill(this);

        StartCoroutine(LOE_SkillEffect());
        GameManager.Inst.DestroyMovePlates();
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
    }

    [Photon.Pun.PunRPC]
    private void LOE_StandardSkill()
    {
        attacked = true;
        GameManager.Inst.SetIsStop(true);
    }

    public IEnumerator LOE_SkillEffect()
    {
        while (!attacked)
        {
            if (selectPieceTo == null || selectPiece == null) yield break;

            selectPieceTo.spriteRenderer.material.color = new Color32(255, 0, 127, 0);
            selectPiece.spriteRenderer.material.color = Color.clear;
            yield return new WaitForSeconds(0.5f);
            selectPieceTo.spriteRenderer.material.color = Color.clear;
            selectPiece.spriteRenderer.material.color = new Color32(255, 0, 127, 0);
            yield return new WaitForSeconds(0.5f);
        }
        if(photonView.IsMine)
        {
            StartCoroutine(LOE_ResetSkill());
        }
        
    }

    [Photon.Pun.PunRPC]
    private void ChangeColor()
    {
        selectPieceTo.spriteRenderer.material.color = Color.clear;
        selectPiece.spriteRenderer.material.color = Color.clear;
    }

    private IEnumerator LOE_ResetSkill()
    {
        base.StartEffect();
        animator.transform.SetParent(null);
        animator.transform.position = Vector2.zero;
        animator.transform.localScale = new Vector3(12f, 12f, 12f);
        animator.Play("LOE_Anim");

        yield return new WaitForSeconds(1f);

        photonView.RPC("ChangeColor", Photon.Pun.RpcTarget.AllBuffered);

        if (skillData.posX == selectPiece.GetXBoard() && skillData.posY == selectPiece.GetYBoard())
        {
            MoveChessPiece(selectPiece, selectPieceTo.GetXBoard(), selectPieceTo.GetYBoard());
            ChessManager.Inst.DestroyChessPiece(selectPieceTo.GetChessData());
            selectPiece.RemoveChosenSkill(this);
        }

        else if (skillData.posX == selectPieceTo.GetXBoard() && skillData.posY == selectPieceTo.GetYBoard())
        {
            MoveChessPiece(selectPieceTo, selectPiece.GetXBoard(), selectPiece.GetYBoard());
            ChessManager.Inst.DestroyChessPiece(selectPiece.GetChessData());
            selectPieceTo.RemoveChosenSkill(this);
        }

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        if (selectPieceTo != null)
        {
            selectPieceTo.RemoveChosenSkill(this);
        }

        RPC_DestroySkill();
    }

    private void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        ChessManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        ChessManager.Inst.SetPosition(cp);
        cp.SetCoordsAnimation();
        GameManager.Inst.DestroyMovePlates();
    }
}
