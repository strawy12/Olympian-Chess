using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Justice : SkillBase
{
    List<ChessBase> attacking = GameManager.Inst.GetAttackings();
    List<ChessBase> justiceCP = new List<ChessBase>();

    public override void UsingSkill()
    {
        photonView.RPC("JT_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void JT_UsingSkill()
    {
        if (selectPiece != null) selectPiece = null;
        for (int i = 0; i < attacking.Count; i++)
        {
            if (attacking[i] == null) continue;
            justiceCP.Add(attacking[i]);
            SkillManager.Inst.AddDontClickPiece(attacking[i]);
            StartCoroutine(JT_Effect(attacking[i]));
        }
    }

    private IEnumerator JT_Effect(ChessBase cp)
    {
        base.StartEffect();
        animator.transform.SetParent(cp.transform);
        if (GameManager.Inst.GetPlayer() == "white")
        {
            animator.transform.position = new Vector2(cp.transform.position.x, cp.transform.position.y + 0.4f);
        }
        else
        {
            animator.transform.position = new Vector2(cp.transform.position.x, cp.transform.position.y - 0.4f);
        }

        animator.Play("JT_Anim");
        yield return new WaitForSeconds(0.75f);
        cp.spriteRenderer.material.color = new Color32(70, 60, 0, 0);
    }
    public override void ResetSkill()
    {

        if (skillData.turnCnt > 2)
        {
            for (int i = 0; i < justiceCP.Count; i++)
            {
                if(justiceCP[i] == null) continue;

                justiceCP[i].spriteRenderer.material.color = new Color32(0, 0, 0, 0);
                SkillManager.Inst.RemoveDontClickPiece(justiceCP[i]);
            }

            if (selectPiece != null)
            {
                selectPiece.RemoveChosenSkill(this);
            }

            RPC_DestroySkill();
        }
    }
}