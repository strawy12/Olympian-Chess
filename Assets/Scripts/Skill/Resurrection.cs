using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : SkillBase
{
    private bool isMake = false;

    public override void UsingSkill()
    {
        Gho_UsingSkill();
    }

    private void Gho_UsingSkill()
    {
        if (skillData.player == "white")
        {
            if (CheckChessPieceCnt("white_pawn") == 8)
            {
                Debug.Log("°É¸²");
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            else
            {
                PutChesspiece();
            }
        }

        else if (skillData.player == "black")
        {
            if (CheckChessPieceCnt("black_pawn") == 8)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            else
            {
                PutChesspiece();
            }
        }
    }

    private int CheckChessPieceCnt(string cp)
    {
        int cnt = 0;

        if (cp.Contains("white"))
        {
            for (int i = 0; i < ChessManager.Inst.GetPlayerWhite().Length; i++)
            {
                if (ChessManager.Inst.GetPlayerWhite()[i] == null) continue;

                if (ChessManager.Inst.GetPlayerWhite()[i].name == cp)
                    cnt++;
            }
        }

        else
        {
            for (int i = 0; i < ChessManager.Inst.GetPlayerBlack().Length; i++)
            {
                if (ChessManager.Inst.GetPlayerBlack()[i] == null) continue;

                if (ChessManager.Inst.GetPlayerBlack()[i].name == cp)
                    cnt++;
            }
        }

        return cnt;
    }

    private void PutChesspiece()
    {
        ChessBase cp;

        if (skillData.player == "white")
        {
            for (int i = 1; i < 7; i++)
            {
                if (isMake) break;

                for (int j = 0; j < 8; j++)
                {
                    if (ChessManager.Inst.GetPosition(j, i) == null)
                    {
                        cp = ChessManager.Inst.Creat(ChessManager.Inst.GetWhiteObject()[0], j, i);
                        ChessManager.Inst.SetPosition(cp);
                        ChessManager.Inst.AddArr(cp);
                        isMake = true;
                        break;
                    }
                }
            }
        }

        else if (skillData.player == "black")
        {
            for (int i = 6; i >= 0; i--)
            {
                if (isMake) break;

                for (int j = 7; j >= 0; j--)
                {
                    if (ChessManager.Inst.GetPosition(j, i) == null)
                    {
                        cp = ChessManager.Inst.Creat(ChessManager.Inst.GetBlackObject()[0], j, i);
                        ChessManager.Inst.SetPosition(cp);
                        ChessManager.Inst.AddArr(cp);
                        isMake = true;
                        break;
                    }
                }
            }
        }

        RemoveSkill();
    }

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}
