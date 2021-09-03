using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarp : SkillBase
{
    public override void UsingSkill()
    {
        TW_UsingSkill();
    }

    private void TW_UsingSkill()
    {
        // if number of used card is zero, 
        // use of this card is canceled
        if (CardManager.Inst.GetUsedCards().Count == 0)
        {
            CardManager.Inst.SetisBreak(true);
            Debug.Log("사용한 카드가 0개입니다.");
            if(selectPiece != null)
            {
                selectPiece.RemoveChosenSkill(this);
            }
            RPC_DestroySkill();
            return;
        }

        photonView.RPC("TW_Effect", Photon.Pun.RpcTarget.AllBuffered);
        int random;

        random = Random.Range(0, CardManager.Inst.GetUsedCards().Count);
        CardManager.Inst.AddUsedCard(random);
    }

    [Photon.Pun.PunRPC]
    private IEnumerator TW_Effect()
    {
        base.StartEffect();
        animator.transform.SetParent(null);
        animator.transform.position = Vector2.zero;
        animator.transform.localScale = new Vector3(12f, 12f, 12f);
        animator.Play("TW_Anim");
        yield return new WaitForSeconds(1.2f);
        RemoveSkill();

    }
    private void RemoveSkill()
    {
        SkillManager.Inst.RemoveSkillList(this);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        DestroySkill();

    }
}