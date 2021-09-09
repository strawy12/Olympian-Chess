using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : SkillBase
{
    int cnt = 0;
    //Music function
    public override void UsingSkill()
    {
        //Preventing King from being the target of Music
        if (selectPiece.name == "black_king" || selectPiece.name == "white_king")
        {
            CardManager.Inst.SetisBreak(true);
            RemoveSkill();
            return;
        }
        photonView.RPC("MC_UsingSkill", Photon.Pun.RpcTarget.AllBuffered);

    }

    [Photon.Pun.PunRPC]
    private void MC_UsingSkill()
    {
        base.StartEffect();
        animator.Play("MC_Anim");
        CardManager.Inst.SetisBreak(false);
        SkillManager.Inst.AddDontClickPiece(selectPiece);
    }
    private bool MC_CheckTurn()
    {

        // Randomly determines whether or not there is a sparkling effect
        if (skillData.turnCnt < 4)
        {
            if(photonView.IsMine)
            {
                if (skillData.turnCnt > 1)
                {
                    if (skillData.turnCnt == 2 && cnt == 1)
                    {
                        cnt++;
                        if (RandomPercent(skillData.turnCnt, 2))
                            return true;
                    }
                    else if (skillData.turnCnt == 3 && cnt == 2)
                    {
                        cnt++;
                        if (RandomPercent(skillData.turnCnt, 3))
                            return true;
                    }
                }
            }
        }
        return false;
    }

    public override void ResetSkill()
    {
        if (!MC_CheckTurn()) return;

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        }
        RPC_DestroySkill();
    }

    //Functions setting true or false randomly
    private bool RandomPercent(int turnTime, int k)
    {
        int percent = Random.Range(1, 11);
        if (turnTime == k)
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
        else if (turnTime == k)
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

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        RPC_DestroySkill();
    }
}
