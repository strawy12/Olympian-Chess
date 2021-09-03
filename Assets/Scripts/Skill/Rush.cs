using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rush : SkillBase
{
    public override void UsingSkill()
    {
        RS_UsingSkill();
    }

    private void RS_UsingSkill()
    {
        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();

        //if color of selected piece is white,
        //selected piece moves up one space
        if (selectPiece.GetChessData().player == "white")
        {
            if (skillData.posY == 7)
            {
                CardManager.Inst.SetisBreak(true);
                StartCoroutine(RemoveSkill());
                return;
            }

            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY + 1) == null)
            {
                ChessManager.Inst.MoveChessPiece(selectPiece, skillData.posX, skillData.posY + 1);
            }

            else
            {
                CardManager.Inst.SetisBreak(true);
                StartCoroutine(RemoveSkill());
                return;
            }
        }
        else
        {
            if (skillData.posY == 0)
            {
                CardManager.Inst.SetisBreak(true);
                StartCoroutine(RemoveSkill());
                return;
            }

            if (ChessManager.Inst.GetPosition(skillData.posX, skillData.posY - 1) == null)
            {
                ChessManager.Inst.MoveChessPiece(selectPiece, skillData.posX, skillData.posY - 1);
            }

            // if the space to go is not empty, Use of the card is canceled.
            else
            {
                CardManager.Inst.SetisBreak(true);
                StartCoroutine(RemoveSkill());
                return;
            }
        }
        TurnManager.Instance.ButtonActive();
        ChessManager.Inst.SetIsMoving(false);

        StartCoroutine(RemoveSkill());
    }

    private IEnumerator RemoveSkill()
    {
        base.StartEffect();
        skillEffect.transform.Rotate(0f, 0f, skillEffect.transform.rotation.z + 90f);
        animator.Play("Rush_Anim");
        yield return new WaitForSeconds(1f);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        RPC_DestroySkill();
    }
}
