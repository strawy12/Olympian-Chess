
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundOfDeath : SkillBase
{
    private GameObject god_Mp;
    private int turn;
    public override void UsingSkill()
    {
        GOD_UsingSkill();
    }

    private void GOD_UsingSkill()
    {
        if (selectPiece.name == "white_pawn" || selectPiece.name == "black_pawn")
        {
            posX = selectPiece.GetXBoard();
            posY = selectPiece.GetYBoard();
            //god_Mp = GOD_MovePlateSpawn(posX, posY);

            turn = 1;
        }
        else
            CardManager.Inst.SetisBreak(true);
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
