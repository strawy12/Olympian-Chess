using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBirth : SkillBase
{
    public override void UsingSkill()
    {
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.color = new Color32(71, 200, 62, 225);
    }

    public override void StandardSkill()
    {
        ChessBase baby;
        ChessBase attacker = ChessManager.Inst.GetPosition(posX, posY);

        if (selectPiece.player == "white")
        {
            GameManager.Inst.SetMoving(false);
            GameManager.Inst.SetUsingSkill(false);

            AttackerPosition(attacker);

            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetWhiteObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
            baby.transform.Rotate(0f, 0f, 180f);
        }
        else
        {
            GameManager.Inst.SetMoving(false);
            GameManager.Inst.SetUsingSkill(false);

            AttackerPosition(attacker);
            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetBlackObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
        }

        ChessManager.Inst.SetPosition(baby);
        ChessManager.Inst.AddArr(baby);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }

        selectPiece.SetAttackSelecting(false);
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }

    private void AttackerPosition(ChessBase attacker)
    {
        int x = selectPiece.GetXBoard();
        int y = selectPiece.GetYBoard();

        if (posX == x)
        {
            if (posY < y)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x, y - 1);
            }

            else if (posY > y)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x, y + 1);
            }
        }

        else if (posY == y)
        {
            if (posX < x)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x - 1, y);
            }

            else if (posX > x)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x + 1, y);
            }
        }

        else if (Mathf.Abs(posX - x) == Mathf.Abs(posY - y))
        {
            if (posX > x && posY > y)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x + 1, y + 1);
            }

            else if (posX > x && posY < y)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x + 1, y - 1);
            }

            else if (posX < x && posY < y)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x - 1, y - 1);
            }

            else if (posX < x && posY > y)
            {
                ChessManager.Inst.MoveChessPiece(attacker, x - 1, y + 1);
            }
        }

        else
        {
            ChessManager.Inst.MoveChessPiece(attacker, posX, posY);
        }
    }
}