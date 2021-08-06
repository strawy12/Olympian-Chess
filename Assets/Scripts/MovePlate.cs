using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    #region º¯¼ö
    [SerializeField] ECardState eCardState;
    // Board positions(not wolrd Positions) 
    int matrixX;
    int matrixY;
    // false: movement, true: attacking
    [Header("¼Ó¼º")]
    public bool attack = false;
    public bool isOD = false;

    private bool isSelected = false;
    enum ECardState { Moving, UsingCard }

    Chessman chessMan = null;

    #endregion

    public void Start()
    {
        AttackChess();

    }
    private void AttackChess()
    {
        if (attack)
        {
            // change color
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    private IEnumerator MovingCard()
    {
        GameManager.Inst.SetPositionEmpty(chessMan.GetXBoard(), chessMan.GetYBoard());
        // setting board size
        chessMan.SetXBoard(matrixX);
        chessMan.SetYBoard(matrixY);
        // change move true
        chessMan.SetIsMoved(true);

        //reference.SetCoords(); --> change to anmiation

        yield return chessMan.SetCoordsAnimation();
        chessMan.DestroyMovePlates();
        GameManager.Inst.SetPosition(chessMan);

        
    }

    public void OnMouseUp()
    {
        if (TurnManager.Instance.isLoading) return;
        if (isSelected) return;
        if (PilSalGi.Inst.GetisUsePilSalGi())
        {
            PilSalGi.Inst.SetselectPiece(GameManager.Inst.GetPosition(matrixX, matrixY));
            return;
        }
        Debug.Log("¸» ÀÌµ¿ ¼º°ø");
        // skill card check
        Cardd();

        OnMouseUpEvent();
    }

    private void OnMouseUpEvent()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

        // set card state
        SetECardState();

        // card attack 
        if (attack)
        {
            Card();

            if (chessMan.isAttacking)
            {
                GameManager.Inst.attackings.Remove(chessMan);
                chessMan.attackCount = 0;
            }

            GameManager.Inst.attackings.Add(chessMan);
            GameManager.Inst.CheckDeadSkillPiece(cp);
            chessMan.isAttacking = true;
        }

        if (eCardState == ECardState.Moving)
        {
            StartCoroutine(ReturnMovingCard());
        }

        else if (eCardState == ECardState.UsingCard)
        {
            Card2();
        }

    }
    private void Cardd()
    {
        if (SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(true)) || SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(false)))
        {
            Skill sk = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(true));

            if (sk == null)
            {
                sk = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(false));
            }

            if (sk.cnt == 0 && !sk.GetSelectPiece().isMoving)
            {
                sk.PlusCnt();
            }

            else
            {
                Chessman cp = sk.GetSelectPiece();
                GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
                GameManager.Inst.UpdateArr(cp);
                Destroy(cp.gameObject);
                sk.DestroyObject();
            }

        }

        if (CheckSkillList("´Þºû", GetCurrentPlayer(true)))
        {
            Skill sk = SkillManager.Inst.GetSkillList("´Þºû", GetCurrentPlayer(true));

            sk.GetSelectPiece().spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);
            sk.SetIsMLMoved(true);
        }

        chessMan.isMoving = true;
        TurnManager.Instance.ButtonColor();
    }
    private IEnumerator ReturnMovingCard()
    {
        yield return MovingCard();
    }
   
    #region isAttackÀÏ¶§
    private void War()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

        Skill sk = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(true));

        if (sk == null)
        {
            sk = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(false));
        }
        if (sk.cnt != 0)
        {
            if (cp.name == "black_king" || cp.name == "white_king") return;
        }
    }
    private void Athen()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
        Skill sk = SkillManager.Inst.GetSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false));

        sk.IsAttack(true);
        sk.CheckAS();
        cp.DestroyMovePlates();
        TurnManager.Instance.ButtonColor();
    }

    private void Born()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
        Skill sk = SkillManager.Inst.GetSkillList("Ãâ»ê", GetCurrentPlayer(false));

        Chessman cm;
        GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());

        if (cp.player == "white")
        {
            cm = GameManager.Inst.Creat("white_pawn", cp.GetXBoard(), cp.GetYBoard());
            cm.transform.Rotate(0f, 0f, 180f);
            GameManager.Inst.SetPosition(cm);
        }
        else
        {
            cm = GameManager.Inst.Creat("black_pawn", cp.GetXBoard(), cp.GetYBoard());
            GameManager.Inst.SetPosition(cm);
        }

        chessMan.SetCoords();
        GameManager.Inst.SetPosition(chessMan);
        chessMan.DestroyMovePlates();
        chessMan.SetIsMoved(true);
        Destroy(cp.gameObject);
        sk.SelectPieceNull();
        SkillManager.Inst.DeleteSkillList(sk);
        //sk.
        Destroy(sk.gameObject);
        GameManager.Inst.UpdateArr(cp);
        GameManager.Inst.AddArr(cm);
        TurnManager.Instance.ButtonColor();
    }

    private void Eros()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
        Skill sk = SkillManager.Inst.GetSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(false));

        if (sk == null)
        {
            return;
        }
        if (cp != sk.GetSelectPieceTo())
        {
            return;
        }

        else
        {
            sk.GetSelectPieceTo().SetXBoard(sk.GetSelectPiece().GetXBoard());
            sk.GetSelectPieceTo().SetYBoard(sk.GetSelectPiece().GetYBoard());
            sk.GetSelectPieceTo().SetCoords();
            Destroy(sk.GetSelectPiece().gameObject);
            GameManager.Inst.SetPosition(sk.GetSelectPieceTo());
            GameManager.Inst.UpdateArr(sk.GetSelectPiece());
            sk.SetIsUsingCard(false);
            chessMan.DestroyMovePlates();
            TurnManager.Instance.ButtonColor();
        }
    }

    private void GillDongMu()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
        Skill sk = SkillManager.Inst.GetSkillList("±æµ¿¹«", GetCurrentPlayer(false));

        if (sk == null) return;

        if (chessMan.name == "black_king" || chessMan.name == "white_king")
        {
            Debug.Log("¿Õ¿Õ¿Õ");
        }

        else
        {
            chessMan.DestroyMovePlates();
            Destroy(chessMan.gameObject);
            Destroy(cp.gameObject);
            GameManager.Inst.SetPositionEmpty(chessMan.GetXBoard(), chessMan.GetYBoard());
            GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
            GameManager.Inst.UpdateArr(chessMan);
            GameManager.Inst.UpdateArr(cp);
        }

        TurnManager.Instance.ButtonColor();
        sk.ReloadStreetFriend();
        sk.SetIsUsingCard(false);

    }

    private void Card()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

        Skill sk = SkillManager.Inst.GetSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false));

        if (SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(true)) || SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(false)))
        {
            War();
        }

        if (SkillManager.Inst.CheckSkillList("´Þºû", GetCurrentPlayer(true)))
        {
            if (cp.name == "black_king" || cp.name == "white_king")
                return;
        }

        if (CheckSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false)) && cp == sk.GetSelectPiece())
        {
            Athen();
        }

        if (CheckSkillList("Ãâ»ê", GetCurrentPlayer(false)))
        {
            Born();
        }

        if (CheckSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(false)))
        {
            Eros();
        }

        if (CheckSkillList("±æµ¿¹«", GetCurrentPlayer(false)))
        {
            GillDongMu();
        }
        if (GetCurrentPlayer(false) == cp.player)
        {
            PilSalGi.Inst.attackCntPlus();
        }

        Destroy(cp.gameObject);
        GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        GameManager.Inst.UpdateArr(cp);
    }
    #endregion

    #region ÇöÀç»óÅÂ¿¡µû¶ó¼­
    private void Eros2()
    {
        Skill sk = SkillManager.Inst.GetSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(true));
        if (sk == null) return;

        sk.SetSelectPieceTo(GameManager.Inst.GetPosition(chessMan.GetXBoard(), chessMan.GetYBoard()));
        chessMan.DestroyMovePlates();
        sk.StartLOE_Effect();
        CardManager.Inst.ChangeIsUse(true);

        //TurnManager.Inst.EndTurn();
        SkillManager.Inst.SetIsUsingCard(false);
    }

    private void Wave()
    {
        Skill sk = SkillManager.Inst.GetSkillList("ÆÄµµ", GetCurrentPlayer(true));
        Debug.Log(matrixX);
        Debug.Log(matrixY);
        if (matrixX == sk.GetSelectPiece().GetXBoard() + 1)
            WaveMove(true, true);
        else if (matrixX == sk.GetSelectPiece().GetXBoard() - 1)
            WaveMove(true, false);
        else if (matrixY == sk.GetSelectPiece().GetYBoard() + 1)
            WaveMove(false, true);
        else if (matrixY == sk.GetSelectPiece().GetYBoard() - 1)
            WaveMove(false, false);
        SkillManager.Inst.SetIsUsingCard(false);
        sk.ReSetWave();
        chessMan.DestroyMovePlates();
        TurnManager.Instance.SetIsActive(true);
        CardManager.Inst.ChangeIsUse(true);
    }
    private void Sleep()
    {
        Skill sk = SkillManager.Inst.GetSkillList("¼ö¸é", GetCurrentPlayer(true));
        if (sk == null) return;
        sk.SetSelectPieceTo(GameManager.Inst.GetPosition(chessMan.GetXBoard(), chessMan.GetYBoard()));
        sk.GetSelectPiece().SetIsMoved(false);
        sk.GetSelectPieceTo().SetIsMoved(false);
        chessMan.DestroyMovePlates();
        sk.StartSP_SkillEffect();
        //TurnManager.Inst.EndTurn();
        CardManager.Inst.ChangeIsUse(true);
        SkillManager.Inst.SetIsUsingCard(false);
    }

    private void Card2()
    {
        if (CheckSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(true)))
        {
            Eros2();
        }

        if (CheckSkillList("ÆÄµµ", GetCurrentPlayer(true)))
        {
            Wave();
        }
        if (CheckSkillList("¼ö¸é", GetCurrentPlayer(true)))
        {
            Sleep();
        }
    } 
    #endregion

    void SetECardState()
    {
        // Change current state to usingCard if card was used
        if (SkillManager.Inst.UsingCard())
            eCardState = ECardState.UsingCard;
        // If not, change the current state to moving
        else
            eCardState = ECardState.Moving;
    }

    void WaveMove(bool isXY, bool isPlma)
    {
        Debug.Log(isXY);
        Debug.Log(isPlma);

        List<Chessman> cmList = new List<Chessman>();
        int cnt = 0;
        cnt = GameManager.Inst.CheckNull(isXY, isPlma, isXY ? matrixX : matrixY);

        #region Á¶°Ç¹®
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
        #endregion

        for (int i = 0; i < cmList.Count; i++)
        {
            GameManager.Inst.SetPosition(cmList[i]);
        }

    }
    Chessman WV_Move(bool isXY, int i, bool isPlma)
    {
        if (isXY)
        {
            Chessman cm = GameManager.Inst.GetPosition(i, matrixY);
            if (cm == null) return cm;
            GameManager.Inst.SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());

            if (isPlma)
                cm.SetXBoard(i + 1);
            else
                cm.SetXBoard(i - 1);

            cm.SetCoords();
            cm.DestroyMovePlates();
            cm.SetIsMoved(true);
            return cm;
        }
        else
        {
            Chessman cm = GameManager.Inst.GetPosition(matrixX, i);
            if (cm == null) return cm;
            GameManager.Inst.SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());

            if (isPlma)
                cm.SetYBoard(i + 1);
            else
                cm.SetYBoard(i - 1);

            cm.SetCoords();
            cm.SetIsMoved(true);
            return cm;
        }

    }
   
    private string GetCurrentPlayer(bool reverse)
    {
        if (!reverse)
        {
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                return "black";
            }
            else if (GameManager.Inst.GetCurrentPlayer() == "black")
            {
                return "white";
            }
        }
        else
        {
            return GameManager.Inst.GetCurrentPlayer();
        }
        return GameManager.Inst.GetCurrentPlayer();

    }
    // checking skills
    private bool CheckSkillList(string name, string player)
    { 
        if (SkillManager.Inst.CheckSkillList(name, player))
            return true;
        else
            return false;
    }
    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }
    // setting coords
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void Setreference(Chessman obj)
    {
        chessMan = obj;
    }

    public Chessman Getreference()
    {
        return chessMan;
    }
    public Chessman GetChessPiece()
    {
        return GameManager.Inst.GetPosition(matrixX, matrixY);
    }
}
