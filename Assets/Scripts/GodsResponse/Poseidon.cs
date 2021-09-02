using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poseidon : SkillBase
{
    public override void UsingSkill()
    {
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        GameManager.Inst.RealAllMovePlateSpawn();
    }

    public override void StandardSkill()
    {
        PoseidonCheck();
    }

    private void PoseidonCheck()
    {
        WaveMove(true, true);
        WaveMove(true, false);
        WaveMove(false, true);
        WaveMove(false, false);

        GameManager.Inst.DestroyMovePlates();
        ResetSkill();
    }

    public override void ResetSkill()
    {
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);

        if (selectPiece != null)
            selectPiece.RemoveChosenSkill(this, true);

        photonView.RPC("P_ResetSkill", Photon.Pun.RpcTarget.AllBuffered);
    }

    [Photon.Pun.PunRPC]
    private void P_ResetSkill()
    {
        SuperSkillManager.Inst.RemoveSuperList(this);
        Destroy(gameObject);
    }

    void WaveMove(bool isXY, bool isPlma)
    {
        List<ChessBase> cmList = new List<ChessBase>();
        int cnt = 0;
        cnt = CheckNull(isXY, isPlma, isXY ? skillData.posY : skillData.posX);

        Debug.Log(cnt);
        //+X
        if (isXY && isPlma)
        {
            Debug.Log(cnt);
            for (int i = skillData.posX + 1; i < cnt; i++)
            {
                cmList.Add(WV_Move(isXY, i, isPlma));
            }
        }

        //-X
        else if (isXY && !isPlma)
        {
            Debug.Log(cnt);

            for (int i = skillData.posX - 1; i >= cnt; i--)
            {
                cmList.Add(WV_Move(isXY, i, isPlma));
            }
        }

        //+Y
        else if (!isXY && isPlma)
        {
            Debug.Log(cnt);

            for (int i = skillData.posY + 1; i < cnt; i++)
            {
                cmList.Add(WV_Move(isXY, i, isPlma));
            }
        }

        //-Y
        else if (!isXY && !isPlma)
        {
            Debug.Log(cnt);

            for (int i = skillData.posY - 1; i >= cnt; i--)
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
        ChessBase cb;
        int x;
        int y;

        if (isXY)
        {
            cb = ChessManager.Inst.GetPosition(i, skillData.posY);

            if (cb == null) return null;

            x = cb.GetXBoard();
            y = cb.GetYBoard();

            ChessManager.Inst.SetPositionEmpty(x, y);

            if (isPlma)
            {
                Debug.Log(i);
                cb.SetXBoard(i + 1);
            }

            else
            {
                Debug.Log(i);
                cb.SetXBoard(i - 1);
            }

            cb.SetCoords();
            cb.PlusMoveCnt();

            return cb;
        }
        else
        {
            cb = ChessManager.Inst.GetPosition(skillData.posX, i);

            if (cb == null) return cb;

            x = cb.GetXBoard();
            y = cb.GetYBoard();

            ChessManager.Inst.SetPositionEmpty(x, y);

            if (isPlma)
            {
                cb.SetYBoard(i + 1);
            }
            else
            {
                cb.SetYBoard(i - 1);
            }

            cb.PlusMoveCnt();

            cb.PlusMoveCnt();
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
            for (int i = 7; i > skillData.posX; i--)
            {
                if (ChessManager.Inst.GetPosition(i, pos) == null)
                {
                    Debug.Log(i);
                    cnt = i + 1;
                    return cnt;
                }
            }
        }

        //-X
        else if (isXY && !isPlma)
        {
            for (int i = 0; i < skillData.posX; i++)
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
            for (int i = 7; i > skillData.posY; i--)
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
            for (int i = 0; i < skillData.posY; i++)
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
