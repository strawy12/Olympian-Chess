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
        SuperSkillManager.Inst.UnUsingSkill("Zeus");
        GameManager.Inst.DestroyMovePlates();

        if (!isSetting)
        {
            originPosX = skillData.posX;
            originPosY = skillData.posY;

            GameManager.Inst.SetUsingSkill(true);
            GameManager.Inst.SetMoving(false);

            if (skillData.posX == 7)
                GameManager.Inst.MovePlateSpawn(skillData.posX - 1, skillData.posY, null);
            else
                GameManager.Inst.MovePlateSpawn(skillData.posX + 1, skillData.posY, null);

            if (skillData.posY == 7)
                GameManager.Inst.MovePlateSpawn(skillData.posX, skillData.posY - 1, null);
            else
                GameManager.Inst.MovePlateSpawn(skillData.posX, skillData.posY + 1, null);

            photonView.RPC("Z_SettingOriginPos", Photon.Pun.RpcTarget.Others, originPosX, originPosY);
            isSetting = true;
        }

        else if (isSetting)
        {
            photonView.RPC("Z_StandardSkill", Photon.Pun.RpcTarget.AllBuffered);
            GameManager.Inst.SetUsingSkill(false);
            GameManager.Inst.SetMoving(true);
        }
    }

    [Photon.Pun.PunRPC]
    private void Z_SettingOriginPos(int posX, int PosY)
    {
        originPosX = posX;
        originPosY = PosY;
    }

    [Photon.Pun.PunRPC]
    private void Z_StandardSkill()
    {
        if (skillData.posY == originPosY + 1 || skillData.posY == originPosY - 1)
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
            SkillManager.Inst.AddDontClickPiece(cps[i], true);
        }

        StartCoroutine(Z_SkillEffect());
    }

    private IEnumerator Z_SkillEffect()
    {
        int k = skillData.turnCnt + 2;
        //if (cp == ChessManager.Inst.GetPosition(originPosX, originPosY))
        //{
        //    if (ChessManager.Inst.GetPosition(originPosX, originPosY) == null) yield break;
        //    k = 3;
        //}
        while (skillData.turnCnt < k)
        {
            for(int i = 0; i < cps.Count; i++)
            {
                cps[i].spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < cps.Count; i++)
            {
                cps[i].spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            }
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i < cps.Count; i++)
        {
            SkillManager.Inst.RemoveDontClickPiece(cps[i], false);
        }

        SuperSkillManager.Inst.RemoveSuperList(this);
        Destroy(gameObject);
    }
}