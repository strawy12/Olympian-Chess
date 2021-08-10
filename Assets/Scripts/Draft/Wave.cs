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
        if (CardManager.Inst.GetSelectCard() != null)
        {
            var targetCards = CardManager.Inst.GetMyCards();
            CardManager.Inst.DestroyCard(CardManager.Inst.GetSelectCard(), targetCards);
        }
        Destroy(gameObject);
        
    }

    private void WV_UsingSkill()
    {
        SkillManager.Inst.SetUsingCard(true);
        TurnManager.Instance.ButtonInactive();
        WV_MovePlate(selectPiece, selectPiece.GetXBoard(), selectPiece.GetYBoard());
    }

    private void WV_MovePlate(Chessman chessPiece, int x, int y)
    {
        //if (CheckNull(true, true, y) > 0 && x != 7) //right
        //    chessPiece.MovePlateSpawn(x + 1, y);

        //if (CheckNull(true, false, y) > 0 && x != 0) //left
        //    chessPiece.MovePlateSpawn(x - 1, y);

        //if (CheckNull(false, true, x) > 0 && y != 7) //up
        //    chessPiece.MovePlateSpawn(x, y + 1);

        //if (CheckNull(false, false, x) > 0 && y != 0) //down
        //    chessPiece.MovePlateSpawn(x, y - 1);
    }

    private void WaveCheck()
    {
        //if (posX == selectPiece.GetXBoard() + 1)
        //    WaveMove(true, true);
        //else if (posX == selectPiece.GetXBoard() - 1)
        //    WaveMove(true, false);
        //else if (posY == selectPiece.GetYBoard() + 1)
        //    WaveMove(false, true);
        //else if (posY == selectPiece.GetYBoard() - 1)
        //    WaveMove(false, false);
        //selectPiece.DestroyMovePlates();
        //ResetSkill();
    }

    //void WaveMove(bool isXY, bool isPlma)
    //{
    //    Debug.Log(isXY);
    //    Debug.Log(isPlma);

    //    List<Chessman> cmList = new List<Chessman>();
    //    int cnt = 0;
    //    cnt = CheckNull(isXY, isPlma, isXY ? posX : posY);


    //    if (isXY && isPlma)
    //    {
    //        for (int i = 0; i < cnt; i++)
    //        {
    //            cmList.Add(WV_Move(isXY, i, isPlma));
    //        }
    //    }

    //    else if (isXY && !isPlma)
    //    {
    //        for (int i = 7; i >= cnt; i--)
    //        {
    //            cmList.Add(WV_Move(isXY, i, isPlma));
    //        }
    //    }

    //    else if (!isXY && isPlma)
    //    {
    //        for (int i = 0; i < cnt; i++)
    //        {
    //            cmList.Add(WV_Move(isXY, i, isPlma));
    //        }
    //    }

    //    else if (!isXY && !isPlma)
    //    {
    //        for (int i = 7; i >= cnt; i--)
    //        {
    //            cmList.Add(WV_Move(isXY, i, isPlma));
    //        }
    //    }

    //    for (int i = 0; i < cmList.Count; i++)
    //    {
    //        //GameManager.Inst.SetPosition(cmList[i]);
    //    }

    //}
    //Chessman WV_Move(bool isXY, int i, bool isPlma)
    //{
    //    if (isXY)
    //    {
    //        Chessman cm = GameManager.Inst.GetPosition(i, posY);
    //        if (cm == null) return cm;
    //        ChessManager.Inst.SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());

    //        if (isPlma)
    //            cm.SetXBoard(i + 1);
    //        else
    //            cm.SetXBoard(i - 1);

    //        cm.SetCoords();
    //        cm.DestroyMovePlates();
    //        cm.PlusMoveCnt();
    //        return cm;
    //    }
    //    else
    //    {
    //        Chessman cm = GameManager.Inst.GetPosition(posX, i);
    //        if (cm == null) return cm;
    //        GameManager.Inst.SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());

    //        if (isPlma)
    //            cm.SetYBoard(i + 1);
    //        else
    //            cm.SetYBoard(i - 1);

    //        //cm.SetCoords();
    //        cm.PlusMoveCnt();
    //        return cm;
    //    }

    //}

    //// Function checking whether pos is empty or not empty
    //public int CheckNull(bool isXY, bool isPlma, int pos)
    //{
    //    //isXY => true => X
    //    //isPlma => true => Plus

    //    int cnt = 0;
    //    //+X
    //    if (isXY && isPlma)
    //    {
    //        for (int i = 7; i >= 0; i--)
    //        {
    //            if (GameManager.Inst.GetPosition(i, pos) == null)
    //            {
    //                cnt = i + 1;
    //                return cnt;
    //            }
    //        }
    //    }

    //    //-X
    //    else if (isXY && !isPlma)
    //    {
    //        for (int i = 0; i < 8; i++)
    //        {
    //            if (GameManager.Inst.GetPosition(i, pos) == null)
    //            {
    //                cnt = i + 1;
    //                return cnt;
    //            }

    //        }
    //    }

    //    //+Y
    //    else if (!isXY && isPlma)
    //    {
    //        for (int i = 7; i >= 0; i--)
    //        {
    //            if (GameManager.Inst.GetPosition(pos, i) == null)
    //            {
    //                cnt = i + 1;
    //                return cnt;
    //            }
    //        }
    //    }

    //    //-Y
    //    else if (!isXY && !isPlma)
    //    {
    //        for (int i = 0; i < 8; i++)
    //        {
    //            if (GameManager.Inst.GetPosition(pos, i) == null)
    //            {
    //                cnt = i + 1;
    //                return cnt;
    //            }
    //        }
    //    }
    //    return 0;
    //}


}
