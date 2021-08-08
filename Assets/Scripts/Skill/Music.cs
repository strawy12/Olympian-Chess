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

        StartCoroutine(MC_SkillEffect());
        CardManager.Inst.SetisBreak(false);
        SkillManager.Inst.AddDontClickPiece(selectPiece);
    }

    private IEnumerator MC_SkillEffect()
    {
        int k = turnCnt;
        int cnt = 1;

        // Randomly determines whether or not there is a sparkling effect
        while (turnCnt < k + 4)
        {
            if (turnCnt > k + 1)
            {
                if (turnCnt == k + 2 && cnt == 1)
                {
                    cnt++;
                    if (RandomPercent(turnCnt, k))
                        break;
                }
                else if (turnCnt == k + 3 && cnt == 2)
                {
                    cnt++;
                    if (RandomPercent(turnCnt, k))
                        break;
                }
            }
            
            selectPiece.spriteRenderer.material.color = new Color32(237, 175, 140, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }

        // if use of this card is over,
        // remove this from skill list and destroy this game object
        SkillManager.Inst.RemoveSkillList(this);
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        selectPiece = null;
        Destroy(gameObject);
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
