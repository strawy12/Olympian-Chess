using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveler : SkillBase
{
    public override void UsingSkill()
    {
        TV_UsingSkill();  
    }

    private void TV_UsingSkill()
    {
        //Only pawns are the target of Traveler
        if (selectPiece.name == "white_pawn" || selectPiece.name == "black_pawn")
        {
            int randomX, randomY;

            // randomly set a location(x,y)
            do
            {
                randomX = Random.Range(0, 8);
                randomY = Random.Range(0, 8);
            } while (ChessManager.Inst.GetPosition(randomX, randomY) != null);

            MoveChessPiece(selectPiece, randomX, randomY);
        }

        //if the pieces are not pawns, use of card is canceled
        else
        {
            CardManager.Inst.SetisBreak(true);
            RemoveSkill();
            return;
        }

        RemoveSkill();
    }

    private void RemoveSkill()
    {
        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        RPC_DestroySkill();
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