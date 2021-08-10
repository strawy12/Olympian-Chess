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

            ChessManager.Inst.SetPositionEmpty(selectPiece.GetXBoard(), selectPiece.GetYBoard());
            selectPiece.SetXBoard(randomX);
            selectPiece.SetYBoard(randomY);
            //selectPiece.SetCoords();

            //GameManager.Inst.SetPosition(selectPiece);
        }

        //if the pieces are not pawns, use of card is canceled
        else
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}