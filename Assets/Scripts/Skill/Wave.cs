using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : SkillBase
{
  
    public override void UsingSkill()
    {
        WV_UsingSkill();
    }

    public override void StandardSkill()
    {
        WaveCheck();
    }
    public override void ResetSkill()
    {
        TurnManager.Instance.ButtonColor();
        SkillManager.Inst.RemoveSkillList(this);
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
        }
        Destroy(gameObject);
        
    }

    private void WV_UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        TurnManager.Instance.ButtonInactive();
        WV_MovePlate(selectPiece, selectPiece.GetXBoard(), selectPiece.GetYBoard());
    }

    private void WV_MovePlate(ChessBase chessPiece, int x, int y)
    {

        if (CheckNull(true, true, y) > 0 && x != 7) //right
            GameManager.Inst.MovePlateSpawn(x + 1, y, selectPiece);

        if (CheckNull(true, false, y) > 0 && x != 0) //left
            GameManager.Inst.MovePlateSpawn(x - 1, y, selectPiece);

        if (CheckNull(false, true, x) > 0 && y != 7) //up
            GameManager.Inst.MovePlateSpawn(x, y + 1, selectPiece);

        if (CheckNull(false, false, x) > 0 && y != 0) //down
            GameManager.Inst.MovePlateSpawn(x, y - 1, selectPiece);
    }

    private void WaveCheck()
    {
        CardManager.Inst.NotAmolang();
        if (posX == selectPiece.GetXBoard() + 1)
            WaveMove(true, true);
        else if (posX == selectPiece.GetXBoard() - 1)
            WaveMove(true, false);
        else if (posY == selectPiece.GetYBoard() + 1)
            WaveMove(false, true);
        else if (posY == selectPiece.GetYBoard() - 1)
            WaveMove(false, false);

        GameManager.Inst.DestroyMovePlates();
        ResetSkill();
    }

    void WaveMove(bool isXY, bool isPlma)
    {
        Debug.Log(isXY);
        Debug.Log(isPlma);

        List<ChessBase> cmList = new List<ChessBase>();
        int cnt = 0;
        cnt = CheckNull(isXY, isPlma, isXY ? posY : posX);


        if (isXY && isPlma)
        {
            for (int i = 0; i < cnt; i++)
            {
                cmList.Add(WV_Move(isXY, i, isPlma));
            }
        }

        else if (isXY && !isPlma)
        {
            for (int i = 7; i >= cnt; i--)
            {
                cmList.Add(WV_Move(isXY, i, isPlma));
            }
        }

        else if (!isXY && isPlma)
        {
            for (int i = 0; i < cnt; i++)
            {
                cmList.Add(WV_Move(isXY, i, isPlma));
            }
        }

        else if (!isXY && !isPlma)
        {
            for (int i = 7; i >= cnt; i--)
            {
                cmList.Add(WV_Move(isXY, i, isPlma));
            }
        }

        for (int i = 0; i < cmList.Count; i++)
        {
            ChessManager.Inst.SetPosition(cmList[i]);
        }

    }
    ChessBase WV_Move(bool isXY, int i, bool isPlma)
    {
        if (isXY)
        {
            ChessBase cb = ChessManager.Inst.GetPosition(i, posY);
            if (cb == null) return null;
            ChessManager.Inst.SetPositionEmpty(i, posY);

            if (isPlma)
                ChessManager.Inst.MoveChessPiece(cb, i + 1, posY);
            else
                ChessManager.Inst.MoveChessPiece(cb, i - 1, posY);

<<<<<<< HEAD
            cm.SetCoords();
            cm.PlusMoveCnt();

            return cm;
=======
            return cb;
>>>>>>> main
        }
        else
        {
            ChessBase cb = ChessManager.Inst.GetPosition(posX, i);
            if (cb == null) return cb;
            ChessManager.Inst.SetPositionEmpty(posX, i);

            if (isPlma)
                ChessManager.Inst.MoveChessPiece(cb, posX, i + 1);
            else
                ChessManager.Inst.MoveChessPiece(cb, posX, i - 1);

            return cb;
        }

    }

    // Function checking whether pos is empty or not empty
    public int CheckNull(bool isXY, bool isPlma, int pos)
    {
        //isXY => true => X
        //isPlma => true => Plus

        int cnt = 0;
        //+X
        if (isXY && isPlma)
        {
            for (int i = 7; i >= 0; i--)
            {
                if (ChessManager.Inst.GetPosition(i, pos) == null)
                {
                    cnt = i + 1;
                    return cnt;
                }
            }
        }

        //-X
        else if (isXY && !isPlma)
        {
            for (int i = 0; i < 8; i++)
            {
                if (ChessManager.Inst.GetPosition(i, pos) == null)
                {
                    cnt = i + 1;
                    return cnt;
                }

            }
        }

        //+Y
        else if (!isXY && isPlma)
        {
            for (int i = 7; i >= 0; i--)
            {
                if (ChessManager.Inst.GetPosition(pos, i) == null)
                {
                    cnt = i + 1;
                    return cnt;
                }
            }
        }

        //-Y
        else if (!isXY && !isPlma)
        {
            for (int i = 0; i < 8; i++)
            {
                if (ChessManager.Inst.GetPosition(pos, i) == null)
                {
                    cnt = i + 1;
                    return cnt;
                }
            }
        }
        return 0;
    }


}
