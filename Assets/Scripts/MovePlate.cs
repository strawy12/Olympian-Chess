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

    Chessman reference = null;

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
        GameManager.Inst.SetPositionEmpty(reference.GetXBoard(), reference.GetYBoard());
        reference.SetXBoard(matrixX);
        reference.SetYBoard(matrixY);
        reference.SetIsMoved(true);

        //reference.SetCoords(); --> change to anmiation

        yield return reference.SetCoordsAnimation();
        reference.DestroyMovePlates();
        GameManager.Inst.SetPosition(reference);

        //if (SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(true)) || SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(false)))
        //{
        //    SkillController sc = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(true));

        //    if (sc == null)
        //    {
        //        sc = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(false));
        //    }

        //    if (sc.cnt == 0 && !sc.GetSelectPiece().isMoving)
        //    {
        //        sc.PlusCnt();
        //        yield return 0;
        //    }

        //    else
        //    {
        //        Chessman cp = sk.GetSelectPiece();
        //        GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        //        GameManager.Inst.UpdateArr(cp);
        //        Destroy(cp.gameObject);
        //        sk.DestroyObject();
        //    }

        //}

        //if (CheckSkillList("´Þºû", GetCurrentPlayer(true)))
        //{
        //    Skill sk = SkillManager.Inst.GetSkillList("´Þºû", GetCurrentPlayer(true));

        //    sk.GetSelectPiece().spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);
        //    sk.SetIsMLMoved(true);
        //}

        reference.isMoving = true;
        TurnManager.Instance.ButtonColor();
    }

    private IEnumerator ReturnMovingCard()
    {
        yield return MovingCard();
    }
   
    #region isAttackÀÏ¶§
    //private void War()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

    //    Skill war = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(true));

    //    if (war == null)
    //    {
    //        war = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(false));
    //    }
    //    if (war.cnt != 0)
    //    {
    //        if (cp.name == "black_king" || cp.name == "white_king") return;
    //    }
    //}
    //private void Athen()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
    //    Skill athen = SkillManager.Inst.GetSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false));

    //    athen.IsAttack(true);
    //    athen.CheckAS();
    //    cp.DestroyMovePlates();
    //    TurnManager.Instance.ButtonColor();
    //}

    //private void Born()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
    //    Skill born = SkillManager.Inst.GetSkillList("Ãâ»ê", GetCurrentPlayer(false));

    //    Chessman cm;
    //    GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());

    //    if (cp.player == "white")
    //    {
    //        cm = GameManager.Inst.Creat("white_pawn", cp.GetXBoard(), cp.GetYBoard());
    //        cm.transform.Rotate(0f, 0f, 180f);
    //        GameManager.Inst.SetPosition(cm);
    //    }
    //    else
    //    {
    //        cm = GameManager.Inst.Creat("black_pawn", cp.GetXBoard(), cp.GetYBoard());
    //        GameManager.Inst.SetPosition(cm);
    //    }

    //    reference.SetCoords();
    //    GameManager.Inst.SetPosition(reference);
    //    reference.DestroyMovePlates();
    //    reference.SetIsMoved(true);
    //    Destroy(cp.gameObject);
    //    born.SelectPieceNull();
    //    SkillManager.Inst.DeleteSkillList(born);
    //    //sk.
    //    Destroy(born.gameObject);
    //    GameManager.Inst.UpdateArr(cp);
    //    GameManager.Inst.AddArr(cm);
    //    TurnManager.Instance.ButtonColor();
    //}

    //private void Eros()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
    //    Skill erosLove = SkillManager.Inst.GetSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(false));

    //    if (erosLove == null)
    //    {
    //        return;
    //    }
    //    if (cp != erosLove.GetSelectPieceTo())
    //    {
    //        return;
    //    }

    //    else
    //    {
    //        erosLove.GetSelectPieceTo().SetXBoard(erosLove.GetSelectPiece().GetXBoard());
    //        erosLove.GetSelectPieceTo().SetYBoard(erosLove.GetSelectPiece().GetYBoard());
    //        erosLove.GetSelectPieceTo().SetCoords();
    //        Destroy(erosLove.GetSelectPiece().gameObject);
    //        GameManager.Inst.SetPosition(erosLove.GetSelectPieceTo());
    //        GameManager.Inst.UpdateArr(erosLove.GetSelectPiece());
    //        erosLove.SetIsUsingCard(false);
    //        reference.DestroyMovePlates();
    //        TurnManager.Instance.ButtonColor();
    //    }
    //}

    //private void GillDongMu()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
    //    Skill gillDongMu = SkillManager.Inst.GetSkillList("±æµ¿¹«", GetCurrentPlayer(false));

    //    if (gillDongMu == null) return;

    //    if (reference.name == "black_king" || reference.name == "white_king")
    //    {
    //        Debug.Log("¿Õ¿Õ¿Õ");
    //    }

    //    else
    //    {
    //        reference.DestroyMovePlates();
    //        Destroy(reference.gameObject);
    //        Destroy(cp.gameObject);
    //        GameManager.Inst.SetPositionEmpty(reference.GetXBoard(), reference.GetYBoard());
    //        GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
    //        GameManager.Inst.UpdateArr(reference);
    //        GameManager.Inst.UpdateArr(cp);
    //    }

    //    TurnManager.Instance.ButtonColor();
    //    gillDongMu.ReloadStreetFriend();
    //    gillDongMu.SetIsUsingCard(false);

    //}

    private void Card()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

        //Skill athen = SkillManager.Inst.GetSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false));

        //if (SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(true)) || SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(false)))
        //{
        //    War();
        //}

        //if (SkillManager.Inst.CheckSkillList("´Þºû", GetCurrentPlayer(true)))
        //{
        //    if (cp.name == "black_king" || cp.name == "white_king")
        //        return;
        //}

        //if (CheckSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false)) && cp == athen.GetSelectPiece())
        //{
        //    Athen();
        //}

        //if (CheckSkillList("Ãâ»ê", GetCurrentPlayer(false)))
        //{
        //    Born();
        //}

        //if (CheckSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(false)))
        //{
        //    Eros();
        //}

        //if (CheckSkillList("±æµ¿¹«", GetCurrentPlayer(false)))
        //{
        //    GillDongMu();
        //}
        //if (GetCurrentPlayer(false) == cp.player)
        //{
        //    PilSalGi.Inst.attackCntPlus();
        //}

        Destroy(cp.gameObject);
        GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        GameManager.Inst.UpdateArr(cp);
    }
    #endregion

    #region ÇöÀç»óÅÂ¿¡µû¶ó¼­
    //private void Eros2()
    //{
    //    Skill erosLove = SkillManager.Inst.GetSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(true));
    //    if (erosLove == null) return;

    //    erosLove.SetSelectPieceTo(GameManager.Inst.GetPosition(reference.GetXBoard(), reference.GetYBoard()));
    //    reference.DestroyMovePlates();
    //    erosLove.StartLOE_Effect();
    //    CardManager.Inst.ChangeIsUse(true);

    //    //TurnManager.Inst.EndTurn();
    //    SkillManager.Inst.SetIsUsingCard(false);
    //}

    //private void Wave()
    //{
    //    Skill wave = SkillManager.Inst.GetSkillList("ÆÄµµ", GetCurrentPlayer(true));
    //    Debug.Log(matrixX);
    //    Debug.Log(matrixY);
    //    if (matrixX == wave.GetSelectPiece().GetXBoard() + 1)
    //        WaveMove(true, true);
    //    else if (matrixX == wave.GetSelectPiece().GetXBoard() - 1)
    //        WaveMove(true, false);
    //    else if (matrixY == wave.GetSelectPiece().GetYBoard() + 1)
    //        WaveMove(false, true);
    //    else if (matrixY == wave.GetSelectPiece().GetYBoard() - 1)
    //        WaveMove(false, false);
    //    SkillManager.Inst.SetIsUsingCard(false);
    //    wave.ReSetWave();
    //    reference.DestroyMovePlates();
    //    TurnManager.Instance.SetIsActive(true);
    //    CardManager.Inst.ChangeIsUse(true);
    //}
    //private void Sleep()
    //{
    //    Skill sleep = SkillManager.Inst.GetSkillList("¼ö¸é", GetCurrentPlayer(true));
    //    if (sleep == null) return;
    //    sleep.SetSelectPieceTo(GameManager.Inst.GetPosition(reference.GetXBoard(), reference.GetYBoard()));
    //    sleep.GetSelectPiece().SetIsMoved(false);
    //    sleep.GetSelectPieceTo().SetIsMoved(false);
    //    reference.DestroyMovePlates();
    //    sleep.StartSP_SkillEffect();
    //    //TurnManager.Inst.EndTurn();
    //    CardManager.Inst.ChangeIsUse(true);
    //    SkillManager.Inst.SetIsUsingCard(false);
    //}

    //private void Card2()
    //{
    //    if (CheckSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(true)))
    //    {
    //        Eros2();
    //    }

    //    if (CheckSkillList("ÆÄµµ", GetCurrentPlayer(true)))
    //    {
    //        Wave();
    //    }
    //    if (CheckSkillList("¼ö¸é", GetCurrentPlayer(true)))
    //    {
    //        Sleep();
    //    }
    //} 
    #endregion
    private void OnMouseUpEvent()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

        // set card state
        SetECardState();

        // card attack 
        if (attack)
        {
            Card();

            //if (reference.isAttacking)
            //{
            //    GameManager.Inst.attackings.Remove(reference);
            //    reference.attackCount = 0;
            //}

            //GameManager.Inst.attackings.Add(reference);
            //GameManager.Inst.CheckDeadSkillPiece(cp);
            //reference.isAttacking = true;
        }
       
        if (eCardState == ECardState.Moving)
        {
            StartCoroutine(ReturnMovingCard());
        }

        else if (eCardState == ECardState.UsingCard)
        {
            //Card2();
            Debug.Log("Ÿ¶ÇÏÁØ ¹Ùº¸");
        }
        
    }

  
    public void OnMouseUp()
    {
        if (TurnManager.Instance.isLoading) return;
        if (isSelected) return;
        //if(PilSalGi.Inst.GetisUsePilSalGi())
        //{
        //    PilSalGi.Inst.SetselectPiece(GameManager.Inst.GetPosition(matrixX, matrixY));
        //    return;
        //}
        Debug.Log("¸» ÀÌµ¿ ¼º°ø");
        // change to coroutine for moving animation
        OnMouseUpEvent();
    }

    void SetECardState()
    {
        // Change current state to usingCard if card was used
        if (SkillManager.Inst.GetUsingCard())
            eCardState = ECardState.UsingCard;
        // If not, change the current state to moving
        else
            eCardState = ECardState.Moving;
    }

    //void WaveMove(bool isXY, bool isPlma)
    //{
    //    Debug.Log(isXY);
    //    Debug.Log(isPlma);

    //    List<Chessman> cmList = new List<Chessman>();
    //    int cnt = 0;
    //    cnt = GameManager.Inst.CheckNull(isXY, isPlma, isXY ? matrixX : matrixY);


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
    //        GameManager.Inst.SetPosition(cmList[i]);
    //    }

    //}
    //Chessman WV_Move(bool isXY, int i, bool isPlma)
    //{
    //    if (isXY)
    //    {
    //        Chessman cm = GameManager.Inst.GetPosition(i, matrixY);
    //        if (cm == null) return cm;
    //        GameManager.Inst.SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());

    //        if (isPlma)
    //            cm.SetXBoard(i + 1);
    //        else
    //            cm.SetXBoard(i - 1);

    //        cm.SetCoords();
    //        cm.DestroyMovePlates();
    //        cm.SetIsMoved(true);
    //        return cm;
    //    }
    //    else
    //    {
    //        Chessman cm = GameManager.Inst.GetPosition(matrixX, i);
    //        if (cm == null) return cm;
    //        GameManager.Inst.SetPositionEmpty(cm.GetXBoard(), cm.GetYBoard());

    //        if (isPlma)
    //            cm.SetYBoard(i + 1);
    //        else
    //            cm.SetYBoard(i - 1);

    //        cm.SetCoords();
    //        cm.SetIsMoved(true);
    //        return cm;
    //    }

    //}
   
   
    
    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void Setreference(Chessman obj)
    {
        reference = obj;
    }

    public Chessman Getreference()
    {
        return reference;
    }
    public Chessman GetChessPiece()
    {
        return GameManager.Inst.GetPosition(matrixX, matrixY);
    }
}
