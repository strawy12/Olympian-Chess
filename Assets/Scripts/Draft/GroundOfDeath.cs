
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundOfDeath : SkillBase
{
    private GameObject god_Mp;
    private Chessman chosen_CP;
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
        StartCoroutine(GOD_SkillEffect());
    }
    private void GOD_UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameManager.Inst.RealAllMovePlateSpawn();
    }

    private void GOD_StandardSkill()
    {
        god_Mp = GameManager.Inst.MovePlateSpawn(posX, posY, null);
        SpriteRenderer sp = god_Mp.GetComponent<SpriteRenderer>();
        sp.material.SetColor("_Color", new Color32(95, 0, 255, 255));
        sp.sortingOrder -= 10;
        GameManager.Inst.RemoveMovePlateList(god_Mp);
        GameManager.Inst.DestroyMovePlates();
    }

    private IEnumerator GOD_SkillEffect()
    {
        if (GameManager.Inst.GetPosition(posX, posY) == null)
        {
            god_Mp.SetActive(false);
            yield return 0;
        }

        else if (GameManager.Inst.GetPosition(posX, posY) != null)
        {
            god_Mp.SetActive(true);
            chosen_CP = GameManager.Inst.GetPosition(posX, posY);
            for (int i = 0; i < 5; i++)
            {
                chosen_CP.spriteRenderer.material.SetColor("_Color", new Color(1, 0, 0, 0));
                yield return new WaitForSeconds(0.05f);
                chosen_CP.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
                yield return new WaitForSeconds(0.05f);
            }
            Destroy(chosen_CP.gameObject);
            GameManager.Inst.SetPositionEmpty(posX, posY);
            god_Mp.SetActive(false);
        }
    }

    //public GameObject GOD_MovePlateSpawn(int matrixX, int matrixY)
    //{
    //   // selectPiece.

    //    GameObject mp = Instantiate("MovePlate", new Vector3(x, y, -2.0f), Quaternion.identity);
    //    mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(95, 0, 255, 255));
    //    mp.tag = "Area";
    //    Destroy(mp.GetComponent<MovePlate>());
    //    mp.GetComponent<Collider2D>().enabled = false;
    //    return mp;
    //}
}
