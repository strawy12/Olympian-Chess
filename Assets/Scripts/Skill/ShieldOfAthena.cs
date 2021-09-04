using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldOfAthena : SkillBase
{
    private bool isAttack = false;
    private int moveCnt = 0;

    public override void UsingSkill()
    {
        photonView.RPC("SOA_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }
    public override void StandardSkill()
    {
        CheckSOA();
    }
    public override void ResetSkill()
    {
        if (selectPiece.GetMoveCnt() == moveCnt && !isAttack) return;

        photonView.RPC("SOA_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void SOA_UsingSkill()
    {
        if (selectPiece.name.Contains("king"))
        {
            CardManager.Inst.SetisBreak(true);
            DestroySkill();
            return;
        }
        moveCnt = selectPiece.GetMoveCnt();
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.color = new Color32(111, 79, 40, 0);
    }

    private void CheckSOA()
    {
        isAttack = true;
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(false);
        GameManager.Inst.SetIsStop(true);
        GameManager.Inst.DestroyMovePlates();
        TurnManager.Instance.ButtonActive();
        ResetSkill();
    }

    [Photon.Pun.PunRPC]
    public IEnumerator SOA_ResetSkill()
    {
        base.StartEffect();
        animator.transform.localScale = new Vector3(3f, 3f, 3f);
        animator.Play("AS_Anim");
        yield return new WaitForSeconds(1.1f);
        selectPiece.spriteRenderer.material.color = Color.clear;
        selectPiece.SetAttackSelecting(false);
        if (selectPiece != null)
        {
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            selectPiece.SetAttackSelecting(false);
            selectPiece.RemoveChosenSkill(this);
        }
        DestroySkill();
    }
}

