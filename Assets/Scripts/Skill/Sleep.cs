using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : SkillBase
{
    GameObject particle;
    GameObject particle2;
    private int turn = 0;
    private int moveCnt = 0;
    private int moveCnt2 = 0;
    private string cp_Player;
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
        CheckParticle();
    }

    private void SP_UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameObject mp =  selectPiece.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard());
        mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(95, 0, 255, 255));
        GameManager.Inst.AllMovePlateSpawn(selectPiece, false);
    }

    private void SleepSetting()
    {
        selectPieceTo = GameManager.Inst.GetPosition(posX, posY);
        moveCnt = selectPiece.GetMoveCnt();
        moveCnt2 = selectPieceTo.GetMoveCnt();
        selectPiece.DestroyMovePlates();
        StartCoroutine(SP_SkillEffect());
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
        turn = 3;
    }

    public IEnumerator SP_SkillEffect()
    {
        int k  = turnCnt + 2;
        while (turnCnt < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(0, 216, 255, 0);
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 216, 255, 0);
            yield return new WaitForSeconds(0.5f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.5f);
        }
        selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
        selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
        checkSPChessPiece();
        turnCnt = 0;
        turn = 2;
    }

    private bool CheckMoveCnt(int moveCnt, int nowMoveCnt)
    {
        if(moveCnt != nowMoveCnt)
        {
            return false;
        }
        return true;
    }

    public void checkSPChessPiece()
    {
        cp_Player = selectPiece.player;
        if (CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            particle = selectPiece.gameObject.transform.GetChild(0).gameObject;
            particle.SetActive(true);
            particle2 = selectPieceTo.gameObject.transform.GetChild(0).gameObject;
            particle2.SetActive(true);
            SkillManager.Inst.AddDontClickPiece(selectPiece);
            SkillManager.Inst.AddDontClickPiece(selectPieceTo);
            return;
        }
        else if (CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && !CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            selectPieceTo = null;
            particle = selectPiece.gameObject.transform.GetChild(0).gameObject;
            
            particle.SetActive(true);
            SkillManager.Inst.AddDontClickPiece(selectPiece); ;
            return;
        }

        else if (!CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            selectPiece = null;
            particle2 = selectPieceTo.gameObject.transform.GetChild(0).gameObject;
            particle2.SetActive(true);
            SkillManager.Inst.AddDontClickPiece(selectPieceTo);
            return;
        }

        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }

    public void SleepParticle()
    {
        if (cp_Player == "black")
        {
            if (GameManager.Inst.GetCurrentPlayer() == "black")
            {
                particle.GetComponent<ParticleSystem>().gravityModifier = -0.01f;
                if (particle2 != null)
                    particle2.GetComponent<ParticleSystem>().gravityModifier = -0.01f;
            }
            else
            {
                particle.GetComponent<ParticleSystem>().gravityModifier = 0.005f;
                if (particle2 != null)
                    particle2.GetComponent<ParticleSystem>().gravityModifier = -0.01f;
            }
        }
        else
        {
            if (GameManager.Inst.GetCurrentPlayer() == "black")
            {
                particle.GetComponent<ParticleSystem>().gravityModifier = 0.005f;
                if (particle2 != null)
                    particle2.GetComponent<ParticleSystem>().gravityModifier = -0.01f;
            }
            else
            {
                particle.GetComponent<ParticleSystem>().gravityModifier = -0.01f;
                if (particle2 != null)
                    particle2.GetComponent<ParticleSystem>().gravityModifier = -0.01f;
            }
        }

    }

    public void CheckParticle()
    {
        if (turn > turnCnt)
        {
            if (particle == null) return;
            SleepParticle();
            return;
        }

        if (selectPiece != null && selectPieceTo != null)
        {
            particle.SetActive(false);
            particle2.SetActive(false);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);
        }
        else if (selectPiece != null)
        {
            particle.SetActive(false);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);

        }

        else if (selectPieceTo != null)
        {
            particle2.SetActive(false);
            SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);

        }
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}
