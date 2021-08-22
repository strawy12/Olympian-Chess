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
        if (turn <= skillData.turnCnt)
        {
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
            selectPiece.SetNoneAttack(false);
            if(selectPiece != null)
            {
                selectPiece.RemoveChosenSkill(this);
            }
            GameManager.Inst.isBacchrs = false;
            SkillManager.Inst.RemoveSkillList(this);
            Destroy(gameObject);
        }
    }

    private void Law_UsingSkill()
    {
        selectPiece.SetNoneAttack(true);
        turn = skillData.turnCnt + 2;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 144));
        skillData.posX = selectPiece.GetXBoard();
        skillData.posY = selectPiece.GetYBoard();
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
                Law_SurroundMovePlate(skillData.posX, skillData.posY);
                break;

            case "black_rook":
            case "white_rook":
                Law_LineMovePlate(1, 0);
                Law_LineMovePlate(0, 1);
                Law_LineMovePlate(-1, 0);
                Law_LineMovePlate(0, -1);
                break;

            case "black_pawn":
                Law_PawnMovePlate(skillData.posX, skillData.posY - 1);
                break;

            case "white_pawn":
                Law_PawnMovePlate(skillData.posX, skillData.posY + 1);
                break;
        }
    }
    private void Law_PawnMovePlate(int x, int y)
    {
        if (selectPiece.GetMoveCnt() != 0)
        {
            if (ChessManager.Inst.GetPosition(x, y) != null)
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
            if (selectPiece.GetChessData().player == "white")
            {
                GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
                if (ChessManager.Inst.GetPosition(x, y + 1) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y + 1, selectPiece);
                }

                else if (ChessManager.Inst.GetPosition(x, y + 1) != null && ChessManager.Inst.GetPosition(x, y + 2) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y + 2, selectPiece);
                }
            }

            else if (skillData.player == "black")
            {
                GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
                if (ChessManager.Inst.GetPosition(x, y - 1) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y - 1, selectPiece);
                }

                else if (ChessManager.Inst.GetPosition(x, y - 1) != null && ChessManager.Inst.GetPosition(x, y - 2) == null)
                {
                    GameManager.Inst.MovePlateSpawn(x, y - 2, selectPiece);
                }
            }
        }

        if (ChessManager.Inst.PositionOnBoard(x + 1, y) && ChessManager.Inst.GetPosition(x + 1, y) != null &&
           ChessManager.Inst.GetPosition(x + 1, y).GetComponent<ChessBase>().GetChessData().player != selectPiece.GetChessData().player)
        {
            GameManager.Inst.MovePlateAttackSpawn(x + 1, y, selectPiece);
        }

        if (ChessManager.Inst.PositionOnBoard(x - 1, y) && ChessManager.Inst.GetPosition(x - 1, y) != null &&
            ChessManager.Inst.GetPosition(x - 1, y).GetComponent<ChessBase>().GetChessData().player != selectPiece.GetChessData().player)
        {
            GameManager.Inst.MovePlateAttackSpawn(x - 1, y, selectPiece);
        }
    }

    private void Law_LineMovePlate(int xIncrement, int yIncrement)
    {
        int x = selectPiece.GetXBoard() + xIncrement;
        int y = selectPiece.GetYBoard() + yIncrement;

        while (ChessManager.Inst.PositionOnBoard(x, y))
        {

            if (ChessManager.Inst.GetPosition(x, y) != null && cnt != 0)
            {
                cnt = 0;
                break;
            }

            else if (ChessManager.Inst.GetPosition(x, y) != null && cnt == 0)
            {
                cnt++;

                if (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y).GetChessData().player != selectPiece.GetChessData().player)
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

        if (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y).GetChessData().player != selectPiece.GetChessData().player)
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
        int x = skillData.posX;
        int y = skillData.posY;
        ChessManager.Inst.PointMovePlate(x + 1, y + 2, selectPiece);
        ChessManager.Inst.PointMovePlate(x - 1, y + 2, selectPiece);
        ChessManager.Inst.PointMovePlate(x + 2, y + 1, selectPiece);
        ChessManager.Inst.PointMovePlate(x + 2, y - 1, selectPiece);
        ChessManager.Inst.PointMovePlate(x + 1, y - 2, selectPiece);
        ChessManager.Inst.PointMovePlate(x - 1, y - 2, selectPiece);
        ChessManager.Inst.PointMovePlate(x - 2, y + 1, selectPiece);
        ChessManager.Inst.PointMovePlate(x - 2, y - 1, selectPiece);
    }

    private void Law_PointMovePlate(int x, int y)
    {
        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            ChessBase cp = ChessManager.Inst.GetPosition(x, y);

            if (cp == null)
            {
                GameManager.Inst.MovePlateSpawn(x, y, selectPiece);
            }

            else if (cp.GetChessData().player != skillData.player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x, y, selectPiece);
            }
        }
    }
}