using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : SkillBase
{
    private int turn = 0;
    private int moveCnt = 0;
    private int moveCnt2 = 0;
    private string cp_Player;
    private Animator animatorTo;
    private GameObject skillEffectTo;
    public override void UsingSkill()
    {
        SP_UsingSkill();
    }

    public override void StandardSkill()
    {
        SleepSetting();
    }
    public override void ResetSkill()
    {
        if (turn > skillData.turnCnt)
        {
            return;
        }
        photonView.RPC("CheckSleep", RpcTarget.AllBuffered);
    }

    private void SP_UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameObject mp = GameManager.Inst.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard(), selectPiece);
        mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(95, 0, 255, 255));
        mp.GetComponent<Collider2D>().enabled = false;
        GameManager.Inst.AllMovePlateSpawn(selectPiece, false);
    }

    private void SleepSetting()
    {
        CardManager.Inst.NotAmolang();
        SetSelectPieceTo(ChessManager.Inst.GetPosition(skillData.posX, skillData.posY));
        moveCnt = selectPiece.GetMoveCnt();
        moveCnt2 = selectPieceTo.GetMoveCnt();
        photonView.RPC("StartSkillEffect", RpcTarget.AllBuffered);
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
        GameManager.Inst.DestroyMovePlates();
    }
    
    [PunRPC]
    private void StartSkillEffect()
    {
        turn = 100;
        StartCoroutine(SP_SkillEffect());
    }

    public IEnumerator SP_SkillEffect()
    {
        int k  = skillData.turnCnt + 2;
        while (skillData.turnCnt < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(0, 216, 255, 0);
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 216, 255, 0);
            yield return new WaitForSeconds(0.5f);
            selectPiece.spriteRenderer.material.color = Color.clear;
            selectPieceTo.spriteRenderer.material.color = Color.clear;
            yield return new WaitForSeconds(0.5f);
        }
        selectPiece.spriteRenderer.material.color = Color.clear;
        selectPieceTo.spriteRenderer.material.color = Color.clear;
        skillData.turnCnt = 0;
        turn = 3;

        CheckSPChessPiece();
        
        
    }

    private bool CheckMoveCnt(int moveCnt, int nowMoveCnt)
    {
        if(moveCnt != nowMoveCnt)
        {
            return false;
        }
        return true;
    }


    public virtual void StartSPEffect()
    {
        skillEffectTo = SkillManager.Inst.SkillEffectSpawn();
        if (GameManager.Inst.GetPlayer() == "black")
        {
            skillEffect.transform.Rotate(0f, 0f, 180f);
        }
        if (selectPiece != null)
        {
            skillEffect.transform.SetParent(selectPieceTo.transform);
            skillEffect.transform.position = selectPieceTo.transform.position;
        }

        animatorTo = skillEffect.GetComponent<Animator>();
    }

    public void CheckSPChessPiece()
    {
        cp_Player = selectPiece.GetChessData().player;
        base.StartEffect();
        if (CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            StartSPEffect();
            animator.Play("SP_Anim");
            animatorTo.Play("SP_Anim");
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 1, 1, 0));
            selectPieceTo.spriteRenderer.material.SetColor("_Color", new Color(0, 1, 1, 0));
            if(photonView.IsMine)
            {
                SkillManager.Inst.AddDontClickPiece(selectPiece);
                SkillManager.Inst.AddDontClickPiece(selectPieceTo);
            }

            return;
        }
        else if (CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && !CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            selectPieceTo = null;
            animator.Play("SP_Anim");

            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 1, 1, 0));
            if (photonView.IsMine)
            {
                SkillManager.Inst.AddDontClickPiece(selectPiece);
            }

            return;
        }

        else if (!CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            selectPiece = null;
            Destroy(skillEffect);
            skillEffect = null;
            StartSPEffect();
            animatorTo.Play("SP_Anim");
            selectPieceTo.spriteRenderer.material.SetColor("_Color", new Color(0, 1, 1, 0));
            if (photonView.IsMine)
            {
                SkillManager.Inst.AddDontClickPiece(selectPieceTo);
            }
            return;
        }
    }


    [Photon.Pun.PunRPC]
    private void CheckSleep()
    {

        Debug.Log(photonView.IsMine);
        if (selectPiece != null && selectPieceTo != null)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
            selectPieceTo.spriteRenderer.material.SetColor("_Color", Color.clear);

            if(photonView.IsMine)
            {
                SkillManager.Inst.RemoveDontClickPiece(selectPiece);
                SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);
            }

            selectPiece.RemoveChosenSkill(this);
            selectPieceTo.RemoveChosenSkill(this);

        }
        else if (selectPiece != null)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
            selectPiece.RemoveChosenSkill(this);
            if (photonView.IsMine)
            {
                SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            }
        }

        else if (selectPieceTo != null)
        {
            selectPieceTo.spriteRenderer.material.SetColor("_Color", Color.clear);
            selectPieceTo.RemoveChosenSkill(this);
            if (photonView.IsMine)
            {
                SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);

            }

        }
        if (skillEffectTo != null)
        {
            Destroy(skillEffectTo);
        }
        DestroySkill();
        
    }
}
