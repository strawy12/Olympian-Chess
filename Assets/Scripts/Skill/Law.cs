using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Law : SkillBase
{

    private int turn;
    private int cnt = 0;
    public override void UsingSkill()
    {
        Law_UsingSkill();
    }

    public override void StandardSkill()
    {
        Law_InitiateMovePlates();
    }

    public override void ResetSkill()
    {
        if (turn <= turnCnt)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
            selectPiece.SetNoneAttack(false);
            if(selectPiece != null)
            {
                selectPiece.RemoveChosenSkill(this);
            }
            SkillManager.Inst.RemoveSkillList(this);
            Destroy(gameObject);
        }
    }

    private void Law_UsingSkill()
    {
        selectPiece.SetNoneAttack(true);
        turn = turnCnt + 2;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 144));
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();
    }


    private void Law_InitiateMovePlates()
    {
        switch (selectPiece.name)
        {
            case "black_queen":
            case "white_queen":
                Law_LineMovePlate(1, 0);
                Law_LineMovePlate(0, 1);
                Law_LineMovePlate(1, 1);
                Law_LineMovePlate(-1, 0);
                Law_LineMovePlate(0, -1);
                Law_LineMovePlate(-1, -1);
                Law_LineMovePlate(-1, 1);
                Law_LineMovePlate(1, -1);
                break;

            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;

            case "black_bishop":
            case "white_bishop":
                Law_LineMovePlate(1, 1);
                Law_LineMovePlate(1, -1);
                Law_LineMovePlate(-1, 1);
                Law_LineMovePlate(-1, -1);
                break;

            case "black_king":
            case "white_king":
                Law_SurroundMovePlate(posX, posY);
                break;

            case "black_rook":
            case "white_rook":
                Law_LineMovePlate(1, 0);
                Law_LineMovePlate(0, 1);
                Law_LineMovePlate(-1, 0);
                Law_LineMovePlate(0, -1);
                break;

            case "black_pawn":
                Law_PawnMovePlate(posX, posY - 1);

                break;

            case "white_pawn":
                Law_PawnMovePlate(posX, posY + 1);

                break;
        }
    }
    private void Law_PawnMovePlate(int x, int y)
    {
        if (selectPiece.GetMoveCnt() != 0)
        {
            if (GameManager.Inst.GetPosition(x, y) != null)
            {
                GameManager.Inst.MovePlateSpawn(x, y + 1, selectPiece);
            }

            else
            {
                GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
            }

        }

        else
        {
            if (selectPiece.player == "white")
            {
                GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
                if (GameManager.Inst.GetPosition(x, y + 1) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y + 1, selectPiece);
                }

                else if (GameManager.Inst.GetPosition(x, y + 1) != null && GameManager.Inst.GetPosition(x, y + 2) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y + 2, selectPiece);
                }

            }

            else if (player == "black")
            {
                GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
                if (GameManager.Inst.GetPosition(x, y - 1) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y - 1, selectPiece);
                }

                else if (GameManager.Inst.GetPosition(x, y - 1) != null && GameManager.Inst.GetPosition(x, y - 2) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y - 2, selectPiece);
                }
            }
        }

        if (GameManager.Inst.PositionOnBoard(x + 1, y) && GameManager.Inst.GetPosition(x + 1, y) != null &&
           GameManager.Inst.GetPosition(x + 1, y).GetComponent<Chessman>().player != selectPiece.player)
        {
            GameManager.Inst.MovePlateAttackSpawn(x + 1, y, selectPiece);
        }

        if (GameManager.Inst.PositionOnBoard(x - 1, y) && GameManager.Inst.GetPosition(x - 1, y) != null &&
            GameManager.Inst.GetPosition(x - 1, y).GetComponent<Chessman>().player != selectPiece.player)
        {
            GameManager.Inst.MovePlateAttackSpawn(x - 1, y, selectPiece);
        }


    }

    private void Law_LineMovePlate(int xIncrement, int yIncrement)
    {
        int x = selectPiece.GetXBoard() + xIncrement;
        int y = selectPiece.GetYBoard() + yIncrement;

        while (GameManager.Inst.PositionOnBoard(x, y))
        {

            if (GameManager.Inst.GetPosition(x, y) != null && cnt != 0)
            {
                cnt = 0;
                break;
            }

            else if (GameManager.Inst.GetPosition(x, y) != null && cnt == 0)
            {
                cnt++;

                if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y).player != selectPiece.player)
                {
                    GameManager.Inst.MovePlateAttackSpawn(x, y, selectPiece);
                }
                x += xIncrement;
                y += yIncrement;
                continue;
            }

            GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
            x += xIncrement;
            y += yIncrement;
        }

        if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y).player != selectPiece.player)
        {
            GameManager.Inst.MovePlateAttackSpawn(x, y, selectPiece);
        }
    }

    private void Law_SurroundMovePlate(int xBoard, int yBoard)
    {
        Law_PointMovePlate(xBoard, yBoard + 1);
        Law_PointMovePlate(xBoard, yBoard - 1);
        Law_PointMovePlate(xBoard - 1, yBoard - 1);
        Law_PointMovePlate(xBoard - 1, yBoard - 0);
        Law_PointMovePlate(xBoard - 1, yBoard + 1);
        Law_PointMovePlate(xBoard + 1, yBoard - 1);
        Law_PointMovePlate(xBoard + 1, yBoard - 0);
        Law_PointMovePlate(xBoard + 1, yBoard + 1);

    }

    public void LMovePlate()
    {
        selectPiece.PointMovePlate(posX + 1, posY + 2);
        selectPiece.PointMovePlate(posX - 1, posY + 2);
        selectPiece.PointMovePlate(posX + 2, posY + 1);
        selectPiece.PointMovePlate(posX + 2, posY - 1);
        selectPiece.PointMovePlate(posX + 1, posY - 2);
        selectPiece.PointMovePlate(posX - 1, posY - 2);
        selectPiece.PointMovePlate(posX - 2, posY + 1);
        selectPiece.PointMovePlate(posX - 2, posY - 1);

    }

    private void Law_PointMovePlate(int x, int y)
    {

        if (GameManager.Inst.PositionOnBoard(x, y))
        {
            Chessman cp = GameManager.Inst.GetPosition(x, y);

            if (cp == null)
            {
                GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
            }

            else if (cp.player != player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x, y, selectPiece);
            }

        }
    }
}

