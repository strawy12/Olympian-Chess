
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundOfDeath : SkillBase
{
    private GameObject god_Mp;
    private ChessBase chosen_CP;
    private int turn;
    public override void UsingSkill()
    {
        GOD_UsingSkill();
    }

    public override void StandardSkill()
    {
        GOD_StandardSkill();
    }
    public override void ResetSkill()
    {
        if (turnCnt < 3) return;
        StartCoroutine(GOD_SkillEffect());
    }
    private void GOD_UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameManager.Inst.RealAllMovePlateSpawn();
        if(selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            selectPiece = null;
        }
    }

    private void GOD_StandardSkill()
    {
        CardManager.Inst.NotAmolang();
        god_Mp = GameManager.Inst.MovePlateSpawn(posX, posY, null);
        SpriteRenderer sp = god_Mp.GetComponent<SpriteRenderer>();
        sp.material.SetColor("_Color", new Color32(95, 0, 255, 255));
        sp.sortingOrder -= 10;
        god_Mp.GetComponent<Collider2D>().enabled = false;
        GameManager.Inst.RemoveMovePlateList(god_Mp);
        GameManager.Inst.DestroyMovePlates();
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
    }

    private IEnumerator GOD_SkillEffect()
    {
        if (ChessManager.Inst.GetPosition(posX, posY) == null)
        {
            god_Mp.SetActive(false);
            yield return 0;
        }

        else if (ChessManager.Inst.GetPosition(posX, posY) != null)
        {
            god_Mp.SetActive(true);
            chosen_CP = ChessManager.Inst.GetPosition(posX, posY);
            for (int i = 0; i < 5; i++)
            {
                chosen_CP.spriteRenderer.material.SetColor("_Color", new Color(1, 0, 0, 0));
                yield return new WaitForSeconds(0.05f);
                chosen_CP.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
                yield return new WaitForSeconds(0.05f);
            }
            Destroy(chosen_CP.gameObject);
            ChessManager.Inst.SetPositionEmpty(posX, posY);
            god_Mp.SetActive(false);
        }
    }
}
