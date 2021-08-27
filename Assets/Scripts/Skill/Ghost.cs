using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : SkillBase
{
    private bool isMake = false;

    public override void UsingSkill()
    {
        photonView.RPC("Gho_UsingSkill", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Gho_UsingSkill()
    {
        if (skillData.player == "white")
        {
            if (CheckChessPieceCnt("white_pawn") == 8 && SkillManager.Inst.GetGodPieces().Count == 0)
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
                        cp = ChessManager.Inst.Creat(ChessManager.Inst.GetWhiteObject()[Selecting()], j, i);
                        photonView.RPC("ChangePiece", RpcTarget.AllBuffered, cp.gameObject.GetPhotonView().ViewID);
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
                        cp = ChessManager.Inst.Creat(ChessManager.Inst.GetBlackObject()[Selecting()], j, i);
                        photonView.RPC("ChangePiece", RpcTarget.AllBuffered, cp.gameObject.GetPhotonView().ViewID);
                        ChessManager.Inst.AddArr(cp);
                        isMake = true;
                        break;
                    }
                }
            }
        }

        RemoveSkill();
    }

    [PunRPC]
    private void ChangePiece(int num)
    {
        if (NetworkManager.Inst.GetPlayer() == "white") return;
        PhotonView.Find(num).gameObject.transform.Rotate(0f, 0f, 180f);
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

    private ChessData SelectInGodPieces()
    {
        List<ChessData> godPieces = SkillManager.Inst.GetGodPieces();
        int random = Random.Range(0, godPieces.Count);

        return godPieces[random];
    }

    private int Selecting()
    {
        if (skillData.player == "white")
        {
            if (!(CheckChessPieceCnt("white_pawn") == 8) && (SkillManager.Inst.GetGodPieces().Count == 0))
                return 0;
            else if ((CheckChessPieceCnt("white_pawn") == 8) && !(SkillManager.Inst.GetGodPieces().Count == 0))
                return GODobject();
            else if (!(CheckChessPieceCnt("white_pawn") == 8) && !(SkillManager.Inst.GetGodPieces().Count == 0))
                return GodOrPawn();
        }

        else
        {
            if (!(CheckChessPieceCnt("black_pawn") == 8) && (SkillManager.Inst.GetGodPieces().Count == 0))
                return 0;
            else if ((CheckChessPieceCnt("black_pawn") == 8) && !(SkillManager.Inst.GetGodPieces().Count == 0))
                return GODobject();
            else if (!(CheckChessPieceCnt("black_pawn") == 8) && !(SkillManager.Inst.GetGodPieces().Count == 0))
                return GodOrPawn();
        }

        return 0;
    }

    private int GODobject()
    {
        GameObject[] cps = skillData.player == "white" ? ChessManager.Inst.GetWhiteObject() : ChessManager.Inst.GetBlackObject();
        string cpName = SelectInGodPieces().chessPiece;

        for (int i = 0; i < 6; i++)
        {
            if (cps[i].name == cpName)
            {
                return i;
            }
        }
        return 0;
    }

    private int GodOrPawn()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
            return 0;
        else
            return GODobject();
    }
}
