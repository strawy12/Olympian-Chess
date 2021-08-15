using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeus : SkillBase
{
    private bool isSetting = false;
    private int originPosX;
    private int originPosY;
    private List<ChessBase> cps = new List<ChessBase>();

    public override void UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameManager.Inst.RealAllMovePlateSpawn();
    }

    public override void StandardSkill()
    {
        GameManager.Inst.DestroyMovePlates();

        if (!isSetting)
        {
            originPosX = posX;
            originPosY = posY;

            GameManager.Inst.SetUsingSkill(true);
            GameManager.Inst.SetMoving(false);

            if (posX == 7)
                GameManager.Inst.MovePlateSpawn(posX - 1, posY, null);
            else
                GameManager.Inst.MovePlateSpawn(posX + 1, posY, null);

            if (posY == 7)
                GameManager.Inst.MovePlateSpawn(posX, posY - 1, null);
            else
                GameManager.Inst.MovePlateSpawn(posX, posY + 1, null);

            isSetting = true;
        }

        else if (isSetting)
        {
            Z_StandardSkill();
            GameManager.Inst.SetUsingSkill(false);
            GameManager.Inst.SetMoving(true);
        }
    }

    public override void ResetSkill()
    {
        if (turnCnt > 3)
        {
            SuperSkillManager.Inst.RemoveSuperList(this);
            Destroy(gameObject);
        }
    }
    private void Z_StandardSkill()
    {
        if (posY == originPosY + 1 || posY == originPosY - 1)
        {
            for (int i = 0; i < 8; i++)
            {
                if (ChessManager.Inst.GetPosition(originPosX, i) == null) continue;
                cps.Add(ChessManager.Inst.GetPosition(originPosX, i));
            }
        }

        else
        {
            for (int i = 0; i < 8; i++)
            {
                if (ChessManager.Inst.GetPosition(i, originPosY) == null) continue;
                cps.Add(ChessManager.Inst.GetPosition(i, originPosY));
            }
        }

        for (int i = 0; i < cps.Count; i++)
        {
            SkillManager.Inst.AddDontClickPiece(cps[i]);
            StartCoroutine(Z_SkillEffect(cps[i]));
        }
    }

    private IEnumerator Z_SkillEffect(ChessBase cp)
    {
        int k = 2;
        if (cp == ChessManager.Inst.GetPosition(originPosX, originPosY))
        {
            if (ChessManager.Inst.GetPosition(originPosX, originPosY) == null) yield break;
            k = 3;
        }

        while (turnCnt < k)
        {
            cp.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            cp.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }

        SkillManager.Inst.RemoveDontClickPiece(cp);
        yield break;
    }
}