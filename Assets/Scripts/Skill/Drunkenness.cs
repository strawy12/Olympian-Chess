using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drunkenness : SkillBase
{
    private int random;


    public override void StandardSkill()
    {
        DR_StandardSkill();
    }

    public override void ResetSkill()
    {
        photonView.RPC("DR_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void DR_ResetSkill()
    {
        if (skillData.turnCnt > 1)
        {
            if (selectPiece != null)
            {
                selectPiece.spriteRenderer.material.color = Color.clear;
                selectPiece.RemoveChosenSkill(this);
            }

            DestroySkill();
            return;
        }

        StartCoroutine(ChangeRoutine());
    }

    private void DR_StandardSkill()
    {
        int x, y;
        List<MovePlate> movePlates = GameManager.Inst.GetMovePlates();
        random = Random.Range(0, movePlates.Count + 1);
        MovePlate movePlate = movePlates[random];

        x = movePlate.GetPosX();
        y = movePlate.GetPosY();

        photonView.RPC("DR_Effect", Photon.Pun.RpcTarget.AllBuffered);
        ChessManager.Inst.MoveChessPiece(selectPiece, x, y);
        TurnManager.Instance.ButtonActive();
    }

    [Photon.Pun.PunRPC]
    private void DR_Effect()
    {
        base.StartEffect();
        animator.Play("DR_Anim");

        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetUsingSkill(false);
        selectPiece.spriteRenderer.material.color = new Color32(255, 204, 0, 0);
    }


    private IEnumerator ChangeRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        GameManager.Inst.SetMoving(false);
        GameManager.Inst.SetUsingSkill(true);
        yield break;
    }
}