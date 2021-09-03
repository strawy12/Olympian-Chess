using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBirth : SkillBase
{
    public override void UsingSkill()
    {
        photonView.RPC("GB_UsingSkill", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void GB_UsingSkill()
    {
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.color = new Color32(71, 200, 62, 225);
    }

    public override void StandardSkill()
    {
        photonView.RPC("GB_StandardSkill", RpcTarget.AllBuffered);

    }

    [PunRPC]
    private void ChangePiece(int num)
    {
        GameObject obj = PhotonView.Find(num).gameObject;
        ChessManager.Inst.AddArr(obj.GetComponent<ChessBase>());
        if (GameManager.Inst.GetPlayer() == "white") return;
        obj.transform.Rotate(0f, 0f, 180f);
    }

    [PunRPC]
    private IEnumerator GB_StandardSkill()
    {
        ChessBase baby;
        ChessBase attacker = ChessManager.Inst.GetPosition(skillData.posX, skillData.posY);

        if (selectPiece.GetChessData().player == "white")
        {
            GameManager.Inst.SetMoving(false);
            GameManager.Inst.SetUsingSkill(false);

            AttackerPosition(attacker);

            base.StartEffect();
            animator.transform.SetParent(null);
            animator.Play("GB_Anim");
            yield return new WaitForSeconds(1f);


            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetWhiteObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
        }
        else
        {
            GameManager.Inst.SetMoving(false);
            GameManager.Inst.SetUsingSkill(false);

            AttackerPosition(attacker);

            base.StartEffect();
            animator.transform.SetParent(null);
            animator.Play("GB_Anim");
            yield return new WaitForSeconds(1f);

            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetBlackObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
        }
        photonView.RPC("ChangePiece", RpcTarget.AllBuffered, baby.gameObject.GetPhotonView().ViewID);
        ChessManager.Inst.SetPosition(baby);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            selectPiece.SetAttackSelecting(false);
        }

        DestroySkill();
    }


    private void AttackerPosition(ChessBase attacker)
    {
        int x = selectPiece.GetXBoard();
        int y = selectPiece.GetYBoard();

        if (skillData.posX == x)
        {
            if (skillData.posY < y)
            {
                MoveChessPiece(attacker, x, y - 1);
            }

            else if (skillData.posY > y)
            {
                MoveChessPiece(attacker, x, y + 1);
            }
        }

        else if (skillData.posY == y)
        {
            if (skillData.posX < x)
            {
                MoveChessPiece(attacker, x - 1, y);
            }

            else if (skillData.posX > x)
            {
                MoveChessPiece(attacker, x + 1, y);
            }
        }

        else if (Mathf.Abs(skillData.posX - x) == Mathf.Abs(skillData.posY - y))
        {
            if (skillData.posX > x && skillData.posY > y)
            {
                MoveChessPiece(attacker, x + 1, y + 1);
            }

            else if (skillData.posX > x && skillData.posY < y)
            {
                MoveChessPiece(attacker, x + 1, y - 1);
            }

            else if (skillData.posX < x && skillData.posY < y)
            {
                MoveChessPiece(attacker, x - 1, y - 1);
            }

            else if (skillData.posX < x && skillData.posY > y)
            {
                MoveChessPiece(attacker, x - 1, y + 1);
            }
        }

        else
        {
            MoveChessPiece(attacker, skillData.posX, skillData.posY);
        }
    }

    private void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        ChessManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        ChessManager.Inst.SetPosition(cp);
        cp.SetCoordsAnimationCo();
        TurnManager.Instance.ButtonActive();
        GameManager.Inst.DestroyMovePlates();
    }
}