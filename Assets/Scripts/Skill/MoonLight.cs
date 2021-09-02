using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonLight : SkillBase
{
    int originX;
    int originY;
    int moveCnt = 0;
    int maxMove = 2;
    Animator animator => skillEffect.GetComponent<Animator>();

    public override void UsingSkill()
    {
        photonView.RPC("ML_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void ML_UsingSkill()
    {
        originX = selectPiece.GetXBoard();
        originY = selectPiece.GetYBoard();

        if (photonView.IsMine)
        {
            selectPiece.SetNoneAttack(true);

            base.StartEffect();
        animator.Play("ML_Anim");

        }

        else
        {
            StartCoroutine(EffectCo());
        }
    }

    private IEnumerator EffectCo()
    {
        selectPiece.SetNoneAttack(true);

        base.StartEffect();

        animator.Play("ML_Anim");

        yield return new WaitForSeconds(1f);
        selectPiece.spriteRenderer.enabled = false;
        animator.gameObject.SetActive(false);
    }
    public override void ResetSkill()
    {

        if (selectPiece == null)
        {
            photonView.RPC("DestroySkill_RPC", Photon.Pun.RpcTarget.AllBuffered);

            return;
        }


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
    private IEnumerator DestroySkill_RPC()
    {
        animator.gameObject.SetActive(true);
        animator.SetFloat("speed", -1);
        animator.Play("ML_Anim", -1, 1f);
        yield return new WaitForSeconds(1.25f);
        selectPiece.spriteRenderer.enabled = true;
        selectPiece.SetIsSelecting(false);
        selectPiece.SetNoneAttack(false);
        selectPiece.RemoveChosenSkill(this);
        DestroySkill();
    }
}
