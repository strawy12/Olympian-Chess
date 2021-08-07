using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveler : SkillBase
{
    SkillController skillController;

    private void Start()
    {
        skillController = GetComponent<SkillController>();
    }

    public override void UsingSkill()
    {
        TravelerSkill();
    }

    private void TravelerSkill()
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
            } while (GameManager.Inst.GetPosition(randomX, randomY) != null);

            selectPiece.SetXBoard(randomX);
            selectPiece.SetYBoard(randomY);
            selectPiece.SetCoords();

            GameManager.Inst.SetPosition(selectPiece);
        }

        //if the pieces are not pawns, use of card is canceled
        else
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }
        //SkillManager.Inst.DeleteSkillList(this);
    }
}