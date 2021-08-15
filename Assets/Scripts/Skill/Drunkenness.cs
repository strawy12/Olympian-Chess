using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drunkenness : SkillBase
{
    int random;

    public override void UsingSkill()
    {
        Drunk_UsingSkill();
    }

    public override void StandardSkill()
    {
        DR_StandardSkill();
    }

    private void DR_StandardSkill()
    {
        int x, y;
        List<GameObject> movePlates = GameManager.Inst.GetMovePlates();
        random = Random.Range(0, movePlates.Count + 1);
        MovePlate movePlate = movePlates[random].GetComponent<MovePlate>();

        x = movePlate.GetPosX();
        y = movePlate.GetPosY();

        GameManager.Inst.SetMoving(false);
        GameManager.Inst.SetUsingSkill(false);

        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(97, 23, 128, 225));
        MoveChessPiece(selectPiece, x, y);
        selectPiece.spriteRenderer.material.SetColor("_Color", Color.clear);

        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetUsingSkill(false);
    }
    private void Drunk_UsingSkill()
    {
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(97, 23, 128, 225));
    }

    private void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        ChessManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        ChessManager.Inst.SetPosition(cp);
        StartCoroutine(ChessManager.Inst.SetCoordsAnimation(cp));
        GameManager.Inst.DestroyMovePlates();
    }
}