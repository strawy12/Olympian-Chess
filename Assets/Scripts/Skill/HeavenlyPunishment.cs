using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenlyPunishment : SkillBase
{
    private bool isBreak = false;

    public override void UsingSkill()
    {
        HP_UsingSkill();
    }

    private void HP_UsingSkill()
    {
        if (selectPiece.name.Contains("king"))
        {
            CardManager.Inst.SetisBreak(true);
            RemoveSkill();
            return;
        }

        // if the opposing team has a rook or rooks,
        //Preventing Queen from being the target of HeavenlyPunishment
        if (selectPiece.name.Contains("queen"))
        {
<<<<<<< HEAD
            if (TurnManager.Instance.CheckPlayer("white"))
=======
            if (GameManager.Inst.CheckPlayer("black"))
            {
>>>>>>> minyoung
                isBreak = ChessManager.Inst.CheckArr(false, "black_rook");
            }
            else
            {
                isBreak = ChessManager.Inst.CheckArr(true, "white_rook");
            }

            Debug.Log(isBreak);
            CardManager.Inst.SetisBreak(isBreak);

            if(isBreak)
            {
                RemoveSkill();
                return;
            }
        }
        photonView.RPC("StartEffect", Photon.Pun.RpcTarget.AllBuffered);
        CardManager.Inst.SetisBreak(false);
        SkillManager.Inst.AddDontClickPiece(selectPiece);
    }

    [Photon.Pun.PunRPC]
    private void StartEffect()
    {
        StartCoroutine(HP_SkillEffect());
    }


    private IEnumerator HP_SkillEffect()
    {
        int k = skillData.turnCnt + 2;
        //sparkling effect (yellow)
        while (skillData.turnCnt < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }
        // When card time is over, selected pieces turn to original color

<<<<<<< HEAD
        
        
=======
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        RemoveSkill();
    }
>>>>>>> minyoung

    private void RemoveSkill()
    {
        SkillManager.Inst.RemoveSkillList(this);
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        }

<<<<<<< HEAD
        photonView.RPC("DestroySkill", Photon.Pun.RpcTarget.AllBuffered);
=======
        Destroy(gameObject);

>>>>>>> minyoung
    }
}
