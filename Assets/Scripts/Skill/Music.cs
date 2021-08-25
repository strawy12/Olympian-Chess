using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : SkillBase
{

    //Music function
    public override void UsingSkill()
    {
        //Preventing King from being the target of Music
        if (selectPiece.name == "black_king" || selectPiece.name == "white_king")
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }
        photonView.RPC("MC_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);

    }

    [Photon.Pun.PunRPC]
    private void MC_UsingSkill()
    {
        StartCoroutine(MC_SkillEffect());
        CardManager.Inst.SetisBreak(false);
        SkillManager.Inst.AddDontClickPiece(selectPiece);
    }
    private IEnumerator MC_SkillEffect()
    {
        int k = skillData.turnCnt;
        int cnt = 1;

        // Randomly determines whether or not there is a sparkling effect
        while (skillData.turnCnt < k + 4)
        {
            if(photonView.IsMine)
            {
                if (skillData.turnCnt > k + 1)
                {
                    if (skillData.turnCnt == k + 2 && cnt == 1)
                    {
                        cnt++;
                        if (RandomPercent(skillData.turnCnt, k))
                            break;
                    }
                    else if (skillData.turnCnt == k + 3 && cnt == 2)
                    {
                        cnt++;
                        if (RandomPercent(skillData.turnCnt, k))
                            break;
                    }
                }
            }
            
            
            selectPiece.spriteRenderer.material.color = new Color32(237, 175, 140, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }

        photonView.RPC("StopEffect", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    public void StopEffect()
    {
        StopAllCoroutines();

        selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        }
        DestroySkill();
    }

    //Functions setting true or false randomly
    private bool RandomPercent(int turnTime, int k)
    {
        int percent = Random.Range(1, 11);
        if (turnTime == k + 2)
        {
            if (percent % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (turnTime == k + 3)
        {
            if (percent > 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }
}
