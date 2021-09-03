using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drunkenness : SkillBase
{
    int random;

    public override void StandardSkill()
    {
        DR_StandardSkill();
    }

    public override void ResetSkill()
    {
        if(skillData.turnCnt > 1)
        {
            photonView.RPC("DR_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
            return;
        }

        GameManager.Inst.SetMoving(false);
        GameManager.Inst.SetUsingSkill(true);
    }

    [Photon.Pun.PunRPC]
    private void DR_ResetSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        DestroySkill();
    }

    private void DR_StandardSkill()
    {
        int x, y;
        List<MovePlate> movePlates = GameManager.Inst.GetMovePlates();
        random = Random.Range(0, movePlates.Count + 1);
        MovePlate movePlate = movePlates[random];

        x = movePlate.GetPosX();
        y = movePlate.GetPosY();

        GameManager.Inst.SetMoving(false);
        GameManager.Inst.SetUsingSkill(false);

        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(97, 23, 128, 225));

        photonView.RPC("DR_Effect", Photon.Pun.RpcTarget.AllBuffered);
        MoveChessPiece(selectPiece, x, y);
        selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);

        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetUsingSkill(false);
    }

    [Photon.Pun.PunRPC]
    private void DR_Effect()
    {
        base.StartEffect();
        animator.Play("DR_Anim");
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