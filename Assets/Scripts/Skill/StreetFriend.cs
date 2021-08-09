using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetFriend : SkillBase
{
    public override void UsingSkill()
    {
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(129, 0, 1, 0));
    }

    public override void StandardSkill()
    {
        Chessman attacker = GameManager.Inst.GetPosition(posX, posY);

        if (attacker.name.Contains("king")) return;

        Destroy(attacker.gameObject);
        GameManager.Inst.SetPositionEmpty(posX, posY);

        GameManager.Inst.UpdateArr(attacker);
        GameManager.Inst.UpdateArr(selectPiece);
        GameManager.Inst.DestroyMovePlates();

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        SkillManager.Inst.RemoveSkillList(this);
        selectPiece.SetAttackSelecting(false);
        Destroy(gameObject);
    }
}
