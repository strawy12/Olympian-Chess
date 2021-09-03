using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : SkillBase
{
    List<ChessData> godList = new List<ChessData>();

    public override void UsingSkill()
    {
        Gho_UsingSkill();
    }

    private void Gho_UsingSkill()
    {
        List<ChessData> cps = SkillManager.Inst.GetGodPieces();

        for (int i = 0; i < cps.Count; i++)
        {
            if (skillData.player == "white" && cps[i].player == "white")
            {
                godList.Add(cps[i]);
            }
        }

        for (int i = 0; i < cps.Count; i++)
        {
            if (skillData.player == "black" && cps[i].player == "black")
            {
                godList.Add(cps[i]);
            }
        }

        if (skillData.player == "white")
        {
            if (CheckChessPieceCnt("white_pawn") == 8 && godList.Count == 0)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            else
            {
                PutChesspiece();
                return;
            }
        }

        else if (skillData.player == "black")
        {
            if (CheckChessPieceCnt("black_pawn") == 8 && godList.Count == 0)
            {
                CardManager.Inst.SetisBreak(true);
                RemoveSkill();
                return;
            }

            else
            {
                PutChesspiece();
                return;
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
        if (skillData.player != GameManager.Inst.GetPlayer())
        {
            RemoveSkill();
            return;
        } 

        if (skillData.player == "white")
        {
            for (int i = 1; i < 7; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (ChessManager.Inst.GetPosition(j, i) == null)
                    {
                        cp = ChessManager.Inst.Creat(ChessManager.Inst.GetWhiteObject()[Selecting()], j, i);
                        ChessManager.Inst.SetPosition(cp);
                        photonView.RPC("ChangePiece", RpcTarget.AllBuffered, cp.gameObject.GetPhotonView().ViewID);
                        return;
                    }
                }
            }
        }

        else if (skillData.player == "black")
        {
            for (int i = 6; i >= 0; i--)
            {
                for (int j = 7; j >= 0; j--)
                {
                    if (ChessManager.Inst.GetPosition(j, i) == null)
                    {
                        cp = ChessManager.Inst.Creat(ChessManager.Inst.GetBlackObject()[Selecting()], j, i);
                        ChessManager.Inst.SetPosition(cp);
                        photonView.RPC("ChangePiece", RpcTarget.AllBuffered, cp.gameObject.GetPhotonView().ViewID);
                        return;
                    }
                }
            }
        }
    }

    [PunRPC]
    private IEnumerator ChangePiece(int num)
    {
        GameObject obj = PhotonView.Find(num).gameObject;
        ChessManager.Inst.AddArr(obj.GetComponent<ChessBase>());

        base.StartEffect();
        animator.transform.SetParent(obj.transform);

        if (GameManager.Inst.GetPlayer() == "white")
        {
            animator.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + 0.1f);
        }
        else
        {
            animator.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y - 0.1f);
        }
        animator.transform.localScale = Vector3.one;

        animator.Play("GS_Anim");

        if (GameManager.Inst.GetPlayer() == "black")
        {
            obj.transform.Rotate(0f, 0f, 180f);
        }
        yield return new WaitForSeconds(1f);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        DestroySkill();
    }

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        RPC_DestroySkill();
    }

    private ChessData SelectInGodPieces()
    {
        int random = Random.Range(0, godList.Count);

        return godList[random];
    }

    private int Selecting()
    {
        if (skillData.player == "white" && skillData.player == TurnManager.Instance.GetCurrentPlayer())
        {
            Debug.Log(CheckChessPieceCnt("white_pawn"));

            if (!(CheckChessPieceCnt("white_pawn") == 8) && (SkillManager.Inst.GetGodPieces().Count == 0))
                return 0;
            else if ((CheckChessPieceCnt("white_pawn") == 8) && !(SkillManager.Inst.GetGodPieces().Count == 0))
                return GODobject();
            else if (!(CheckChessPieceCnt("white_pawn") == 8) && !(SkillManager.Inst.GetGodPieces().Count == 0))
                return GodOrPawn();
        }

        else if (skillData.player == "black" && skillData.player == TurnManager.Instance.GetCurrentPlayer())
        {
            Debug.Log(CheckChessPieceCnt("black_pawn"));
            Debug.Log(SkillManager.Inst.GetGodPieces().Count);

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
        int num;
        Debug.Log(cpName);

        for (int i = 0; i < 6; i++)
        {
            Debug.Log(cps[i].name);

            if (cpName.Contains(cps[i].name))
            {
                SkillManager.Inst.RemoveGodPieces(SelectInGodPieces());
                Debug.Log("sdf");
                num = i;
                return num;
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
