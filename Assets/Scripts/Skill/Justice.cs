using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Justice : SkillBase
{
    List<Chessman> attacking = GameManager.Inst.attackings;
    List<Chessman> justiceCP = new List<Chessman>();

    public override void UsingSkill()
    {
        for (int i = 0; i < attacking.Count; i++)
        {
            if (attacking[i] == null) continue;
            justiceCP.Add(attacking[i]);
            SkillManager.Inst.AddDontClickPiece(attacking[i]);
            attacking[i].spriteRenderer.material.color = new Color32(70, 60, 0, 0);
        }
    }
    public override void ResetSkill()
    {
        if (turnCnt > 2)
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

            SkillManager.Inst.RemoveSkillList(this);
            Destroy(gameObject);
        }
    }
}