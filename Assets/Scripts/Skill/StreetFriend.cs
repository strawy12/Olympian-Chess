using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetFriend : SkillBase
{
    public override void UsingSkill()
    {
        photonView.RPC("SF_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void SF_UsingSkill()
    {
        selectPiece.SetAttackSelecting(true);
        base.StartEffect();
        if(GameManager.Inst.GetPlayer() == "white")
        {
            animator.transform.position = new Vector2(selectPiece.transform.position.x, selectPiece.transform.position.y + 0.5f);
        }
        else
        {
            animator.transform.position = new Vector2(selectPiece.transform.position.x, selectPiece.transform.position.y - 0.5f);
        }

        animator.SetFloat("speed", -1f);
        animator.Play("ST_Anim", -1, 1f);
    }

    public override void StandardSkill()
    {
        StartCoroutine(ST_StandardSkill());
    }

    private IEnumerator ST_StandardSkill()
    {
        ChessBase attacker = ChessManager.Inst.GetPosition(skillData.posX, skillData.posY);

        if (attacker.name.Contains("king"))
        {
            photonView.RPC("RemoveSkill", Photon.Pun.RpcTarget.AllBuffered);
            yield break;
        }
        selectPiece = null;
        photonView.RPC("ST_Effect", Photon.Pun.RpcTarget.AllBuffered);


        yield return new WaitForSeconds(1.1f);
        ChessManager.Inst.DestroyChessPiece(attacker.GetChessData());
        GameManager.Inst.DestroyMovePlates();

        photonView.RPC("RemoveSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void ST_Effect()
    {
        base.StartEffect();
        animator.transform.position = Vector2.zero;
        animator.transform.localScale = new Vector3(3f, 3f, 3f);
        animator.Play("ST_Anim");
    }

    [Photon.Pun.PunRPC]
    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            selectPiece.SetAttackSelecting(false);
        }
        DestroySkill();
    }
}
