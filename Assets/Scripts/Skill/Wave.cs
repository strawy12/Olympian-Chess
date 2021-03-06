using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : SkillBase
{
    bool isMoving;
    public override void UsingSkill()
    {
        WV_UsingSkill();
    }

    public override void StandardSkill()
    {
        StartCoroutine(WaveCheck());
    }
    public override void ResetSkill()
    {
        WV_ResetSkill();
    }


    private void WV_UsingSkill()
    {
        isMoving = ChessManager.Inst.GetIsMoving();
        GameManager.Inst.SetUsingSkill(true);
        GameManager.Inst.SetMoving(false);
        //TurnManager.Instance.ButtonInactive();
        WV_MovePlate(selectPiece, selectPiece.GetXBoard(), selectPiece.GetYBoard());
    }

    private void WV_ResetSkill()
    {
        if (!isMoving)
        {
            TurnManager.Instance.ButtonInactive();
            ChessManager.Inst.SetIsMoving(false);
        }
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this, true);
        }
        RPC_DestroySkill();


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

    private IEnumerator WaveCheck()
    {
        CardManager.Inst.NotAmolang();
        if (skillData.posX == selectPiece.GetXBoard() + 1)
            WaveMove(true, true);
        else if (skillData.posX == selectPiece.GetXBoard() - 1)
            WaveMove(true, false);
        else if (skillData.posY == selectPiece.GetYBoard() + 1)
            WaveMove(false, true);
        else if (skillData.posY == selectPiece.GetYBoard() - 1)
            WaveMove(false, false);

        GameManager.Inst.DestroyMovePlates();

        yield return new WaitForSeconds(3f);
        ResetSkill();
    }

    [Photon.Pun.PunRPC]
    private void WV_Effect(int x, int y)
    {
        Debug.Log("????");
        base.StartEffect();

        float xPos = x;
        float yPos = y;

        xPos *= 0.598f;
        yPos *= 0.598f;

        xPos += -2.094f;
        yPos += -2.094f;
        animator.transform.SetParent(null);
        animator.transform.position = new Vector2(xPos, yPos);
        animator.transform.localScale = new Vector3(2f, 2f, 2f);
        animator.Play("WV_Anim");
    }
    void WaveMove(bool isXY, bool isPlma)
    {
        Debug.Log(isXY);
        Debug.Log(isPlma);

        List<ChessBase> cmList = new List<ChessBase>();
        int cnt = 0;
        cnt = CheckNull(isXY, isPlma, isXY ? skillData.posY : skillData.posX);

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
            if (cmList[i] == null) continue;
            Debug.Log(cmList[i]);
            photonView.RPC("WV_Effect", Photon.Pun.RpcTarget.AllBuffered, cmList[i].GetXBoard(), cmList[i].GetYBoard());
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
                cb.SetXBoard(i + 1);
            else
                cb.SetXBoard(i - 1);

            //ChessManager.Inst.SetCoords(cb.gameObject, x, y);
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
                cb.SetYBoard(i + 1);
            else
                cb.SetYBoard(i - 1);

            //ChessManager.Inst.SetCoords(cb.gameObject, x, y);
            cb.SetCoords();
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