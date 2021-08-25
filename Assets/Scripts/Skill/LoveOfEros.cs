using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoveOfEros : SkillBase
{
    private bool attacked = false;
    private bool isSetting = false;
    public override void UsingSkill()
    {
        LOE_UsingSkill();
    }

    public override void StandardSkill()
    {
        if (!isSetting)
        {
            photonView.RPC("LOE_Setting", Photon.Pun.RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC("LOE_StandardSkill", Photon.Pun.RpcTarget.AllBuffered);
        }
    }

    private void LOE_UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameObject mp = GameManager.Inst.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard(), selectPiece);
        mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(29, 219, 22, 255));
        mp.GetComponent<Collider2D>().enabled = false;
        GameManager.Inst.AllMovePlateSpawn(selectPiece, true);
    }

    [Photon.Pun.PunRPC]
    private void LOE_Setting()
    {
        CardManager.Inst.NotAmolang();
        isSetting = true;
        attacked = false;
        selectPieceTo = ChessManager.Inst.GetPosition(skillData.posX, skillData.posY);

        selectPieceTo.SetAttackSelecting(true);
        selectPiece.SetAttackSelecting(true);

        selectPieceTo.AddChosenSkill(this);
        selectPiece.AddChosenSkill(this);

        StartCoroutine(LOE_SkillEffect());
        GameManager.Inst.DestroyMovePlates();
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
    }

    [Photon.Pun.PunRPC]
    private void LOE_StandardSkill()
    {
        attacked = true;
        GameManager.Inst.SetIsStop(true);
    }

    public IEnumerator LOE_SkillEffect()
    {
        while (!attacked)
        {
            if (selectPieceTo == null || selectPiece == null) yield break;

            selectPieceTo.spriteRenderer.material.color = new Color32(255, 0, 127, 0);
            selectPiece.spriteRenderer.material.color = Color.clear;
            yield return new WaitForSeconds(0.5f);
            selectPieceTo.spriteRenderer.material.color = Color.clear;
            selectPiece.spriteRenderer.material.color = new Color32(255, 0, 127, 0);
            yield return new WaitForSeconds(0.5f);
        }

        if (skillData.posX == selectPiece.GetXBoard() && skillData.posY == selectPiece.GetYBoard())
        {
            Debug.Log(1);
            MoveChessPiece(selectPiece, selectPieceTo.GetXBoard(), selectPieceTo.GetYBoard());
            ChessManager.Inst.DestroyChessPiece(selectPieceTo.GetChessData());
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            selectPiece.RemoveChosenSkill(this);
        }

        else if (skillData.posX == selectPieceTo.GetXBoard() && skillData.posY == selectPieceTo.GetYBoard())
        {
            Debug.Log(2);

            MoveChessPiece(selectPieceTo, selectPiece.GetXBoard(), selectPiece.GetYBoard());
            ChessManager.Inst.DestroyChessPiece(selectPiece.GetChessData());
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            selectPieceTo.RemoveChosenSkill(this);
        }


        if (selectPiece != null)
        {
            if (selectPieceTo != null)
            {
                selectPieceTo.RemoveChosenSkill(this);
            }
            selectPiece.RemoveChosenSkill(this);
        }
        Debug.Log(skillData.posX + ", " + skillData.posY);
        Debug.Log(ChessManager.Inst.GetPosition(skillData.posX, skillData.posY).name);
        Debug.Log(selectPiece.GetXBoard() + ", " + selectPiece.GetYBoard());
        Debug.Log(ChessManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard()).name);
        Debug.Log(selectPieceTo.GetXBoard() + ", " + selectPieceTo.GetYBoard());
        Debug.Log(ChessManager.Inst.GetPosition(selectPieceTo.GetXBoard(), selectPieceTo.GetYBoard()).name);


        DestroySkill();
    }

    private void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        ChessManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        ChessManager.Inst.SetPosition(cp);
        cp.SetCoordsAnimation();
        GameManager.Inst.DestroyMovePlates();
    }
}
