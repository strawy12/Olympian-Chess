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
        ChessBase attacker = ChessManager.Inst.GetPosition(posX, posY);

        if (attacker.name.Contains("king"))
        {
            RemoveSkill();
            return;
        }

        Destroy(attacker.gameObject);
        ChessManager.Inst.SetPositionEmpty(posX, posY);

        ChessManager.Inst.UpdateArr(attacker);
        ChessManager.Inst.UpdateArr(selectPiece);
        GameManager.Inst.DestroyMovePlates();

        RemoveSkill();
    }

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            selectPiece.SetAttackSelecting(false);
        }
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}
