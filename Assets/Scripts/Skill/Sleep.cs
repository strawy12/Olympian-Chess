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
        CheckSleep();
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
        selectPieceTo = ChessManager.Inst.GetPosition(skillData.posX, skillData.posY);
        moveCnt = selectPiece.GetMoveCnt();
        moveCnt2 = selectPieceTo.GetMoveCnt();
        StartCoroutine(SP_SkillEffect());
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
        GameManager.Inst.DestroyMovePlates();
        turn = 3;
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
        CheckSPChessPiece();
        skillData.turnCnt = 0;
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

    public void CheckSPChessPiece()
    {
        cp_Player = selectPiece.GetChessData().player;
        if (CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            //particle = selectPiece.gameObject.transform.GetChild(0).gameObject;
            //particle.SetActive(true);
            //particle2 = selectPieceTo.gameObject.transform.GetChild(0).gameObject;
            //particle2.SetActive(true);
            selectPiece.spriteRenderer.material.SetColor("_Color", Color.cyan);
            selectPieceTo.spriteRenderer.material.SetColor("_Color", Color.cyan);
            SkillManager.Inst.AddDontClickPiece(selectPiece);
            SkillManager.Inst.AddDontClickPiece(selectPieceTo);
            return;
        }
        else if (CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && !CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            selectPieceTo = null;
            //particle = selectPiece.gameObject.transform.GetChild(0).gameObject;
            //particle.SetActive(true);

            selectPiece.spriteRenderer.material.SetColor("_Color", Color.cyan);

            SkillManager.Inst.AddDontClickPiece(selectPiece); ;
            return;
        }

        else if (!CheckMoveCnt(moveCnt, selectPiece.GetMoveCnt()) && CheckMoveCnt(moveCnt2, selectPieceTo.GetMoveCnt()))
        {
            selectPiece = null;
            //particle2 = selectPieceTo.gameObject.transform.GetChild(0).gameObject;
            //particle2.SetActive(true);

            selectPieceTo.spriteRenderer.material.SetColor("_Color", Color.cyan);

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
            if (TurnManager.Instance.CheckPlayer("black"))
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
            if (TurnManager.Instance.CheckPlayer("black"))
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
        if (turn > skillData.turnCnt)
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
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        Destroy(gameObject);
    }

    private void CheckSleep()
    {
        if (turn > skillData.turnCnt)
        {
            return;
        }

        if (selectPiece != null && selectPieceTo != null)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
            selectPieceTo.spriteRenderer.material.SetColor("_Color", Color.clear);

            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);
        }
        else if (selectPiece != null)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);

        }

        else if (selectPieceTo != null)
        {
            selectPieceTo.spriteRenderer.material.SetColor("_Color", Color.clear);
            SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);

        }
        SkillManager.Inst.RemoveSkillList(this);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        Destroy(gameObject);
    }
}
