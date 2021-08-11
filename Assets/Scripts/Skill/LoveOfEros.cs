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
        if(!isSetting)
        {
            LOE_Setting();
        }
        else
        {
            LOE_StandardSkill();
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
    private void LOE_Setting()
    {
        CardManager.Inst.NotAmolang();
        isSetting = true;
        attacked = false;
        selectPieceTo = ChessManager.Inst.GetPosition(posX, posY);

        selectPieceTo.SetAttackSelecting(true);
        selectPiece.SetAttackSelecting(true);

        selectPieceTo.AddChosenSkill(this);
        selectPiece.AddChosenSkill(this);

        StartCoroutine(LOE_SkillEffect());
        GameManager.Inst.DestroyMovePlates();
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
    }
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
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            selectPiece.spriteRenderer.material.color = new Color32(255, 0, 127, 0);
            yield return new WaitForSeconds(0.5f);
        }

        if(posX == selectPiece.GetXBoard() && posY == selectPiece.GetYBoard())
        {
            ChessManager.Inst.MoveChessPiece(selectPiece, selectPieceTo.GetXBoard(), selectPieceTo.GetYBoard());
            ChessManager.Inst.SetPositionEmpty(selectPieceTo.GetXBoard(), selectPieceTo.GetYBoard());
            ChessManager.Inst.UpdateArr(selectPieceTo);
            Destroy(selectPieceTo.gameObject);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            selectPiece.RemoveChosenSkill(this);
        }

        else if (posX == selectPieceTo.GetXBoard() && posY == selectPieceTo.GetYBoard())
        {
            ChessManager.Inst.MoveChessPiece(selectPieceTo, selectPiece.GetXBoard(), selectPiece.GetYBoard());
            ChessManager.Inst.SetPositionEmpty(selectPiece.GetXBoard(), selectPiece.GetYBoard());
            ChessManager.Inst.UpdateArr(selectPiece);
            Destroy(selectPiece.gameObject);
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            selectPieceTo.RemoveChosenSkill(this);
        }

        SkillManager.Inst.RemoveSkillList(this);

        if (selectPiece != null)
        {
            if(selectPieceTo != null)
            {
                selectPieceTo.RemoveChosenSkill(this);
            }
            selectPiece.RemoveChosenSkill(this);
        }

        Destroy(gameObject);
    }
}
