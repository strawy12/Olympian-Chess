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
    enum ECardState { Moving, Skill, MovingAndSkill }

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
        reference.PlusMoveCnt();

        //reference.SetCoords(); --> change to anmiation

        yield return reference.SetCoordsAnimation();
        GameManager.Inst.DestroyMovePlates();
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
    }

    private IEnumerator ReturnMovingCard()
    {
        TurnManager.Instance.ButtonColor();
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

        if (cp.GetAttackSelecting())
        {
            Debug.Log("³È");
            SkillManager.Inst.AttackUsingSkill(this);
            return;
        }

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

        else if (eCardState == ECardState.Skill)
        {
            Debug.Log("¸» ÀÌµ¿ ¼º°ø");
            SkillManager.Inst.UsingSkill(this);
        }

        if (eCardState == ECardState.MovingAndSkill)
        {
            StartCoroutine(ReturnMovingCard());
            SkillManager.Inst.UsingSkill(this);
        }
    }

  
    public void OnMouseUp()
    {
        if (TurnManager.Instance.isLoading) return;
        if (TurnManager.Instance.GetIsActive()) return;
        if (isSelected) return;
        //if(PilSalGi.Inst.GetisUsePilSalGi())
        //{
        //    PilSalGi.Inst.SetselectPiece(GameManager.Inst.GetPosition(matrixX, matrixY));
        //    return;
        //}
        
        // change to coroutine for moving animation
        OnMouseUpEvent();
    }

    void SetECardState()
    {
        // Change current state to usingCard if card was used
        if (GameManager.Inst.GetMoving() && GameManager.Inst.GetUsingSkill())
        {
            eCardState = ECardState.MovingAndSkill;
        }
            
        // If not, change the current state to moving
        else if(GameManager.Inst.GetMoving() && !GameManager.Inst.GetUsingSkill())
        {
            eCardState = ECardState.Moving;
        }
            
        else if (!GameManager.Inst.GetMoving() && GameManager.Inst.GetUsingSkill())
        {
            eCardState = ECardState.Skill;
        }
    }





    public void SetIsSelected(bool isSelected)
    {
        this.isSelected = isSelected;
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public int GetPosX()
    {
        return matrixX;
    }

    public int GetPosY()
    {
        return matrixY;
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
