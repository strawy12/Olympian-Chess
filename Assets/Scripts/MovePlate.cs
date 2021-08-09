using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    #region 변수
    [SerializeField] EChessState eChessState;
    // Board positions(not wolrd Positions) 
    int matrixX;
    int matrixY;
    // false: movement, true: attacking
    [Header("속성")]
    public bool attack = false;
    public bool isStop = false;

    private bool isSelected = false;
<<<<<<< HEAD
    enum EChessState { Moving, Skill, MovingAndSkill, Stop }
=======
    enum ECardState { Moving, Skill, MovingAndSkill, Stop }
>>>>>>> minyoung

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
<<<<<<< HEAD
        reference.PlusMoveCnt();

=======
        reference.SetIsMoved(true);
>>>>>>> minyoung
        //reference.SetCoords(); --> change to anmiation

        yield return reference.SetCoordsAnimation();
        GameManager.Inst.DestroyMovePlates();
        GameManager.Inst.SetPosition(reference);

        reference.isMoving = true;
    }

    private IEnumerator ReturnMovingCard()
    {
        TurnManager.Instance.ButtonColor();
        yield return MovingCard();
    }
<<<<<<< HEAD

    #region isAttack일때
    //private void War()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

    //    Skill war = SkillManager.Inst.GetSkillList("전쟁광", GetCurrentPlayer(true));

    //    if (war == null)
    //    {
    //        war = SkillManager.Inst.GetSkillList("전쟁광", GetCurrentPlayer(false));
    //    }
    //    if (war.cnt != 0)
    //    {
    //        if (cp.name == "black_king" || cp.name == "white_king") return;
    //    }
    //}
    //private void Athen()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
    //    Skill athen = SkillManager.Inst.GetSkillList("아테나의 방패", GetCurrentPlayer(false));

    //    athen.IsAttack(true);
    //    athen.CheckAS();
    //    cp.DestroyMovePlates();
    //    TurnManager.Instance.ButtonColor();
    //}

    //private void Born()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
    //    Skill born = SkillManager.Inst.GetSkillList("출산", GetCurrentPlayer(false));

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
=======
>>>>>>> minyoung

    #region isAttack일때

    //private void Eros()
    //{
    //    Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
    //    Skill erosLove = SkillManager.Inst.GetSkillList("에로스의 사랑", GetCurrentPlayer(false));

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

    #endregion
    private void Card()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

<<<<<<< HEAD
        if (cp.GetAttackSelecting())
        {
            SkillManager.Inst.AttackUsingSkill(this);
        }

        if (GameManager.Inst.GetIsStop())
        {
            return;
        }

        Destroy(cp.gameObject);
        GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        GameManager.Inst.UpdateArr(cp);





        //Skill athen = SkillManager.Inst.GetSkillList("아테나의 방패", GetCurrentPlayer(false));

        //if (SkillManager.Inst.CheckSkillList("전쟁광", GetCurrentPlayer(true)) || SkillManager.Inst.CheckSkillList("전쟁광", GetCurrentPlayer(false)))
        //{
        //    War();
        //}

        //if (SkillManager.Inst.CheckSkillList("달빛", GetCurrentPlayer(true)))
        //{
        //    if (cp.name == "black_king" || cp.name == "white_king")
        //        return;
        //}

        //if (CheckSkillList("아테나의 방패", GetCurrentPlayer(false)) && cp == athen.GetSelectPiece())
        //{
        //    Athen();
        //}

        //if (CheckSkillList("출산", GetCurrentPlayer(false)))
        //{
        //    Born();
        //}

        //if (CheckSkillList("에로스의 사랑", GetCurrentPlayer(false)))
        //{
        //    Eros();
        //}
=======
        if(cp.GetAttackSelecting())
        {
            SkillManager.Inst.AttackUsingSkill(this);
        }
>>>>>>> minyoung

        SetECardState();

<<<<<<< HEAD

=======
        if(eCardState == ECardState.Stop)
        {
            return;
        }
        
        Destroy(cp.gameObject);
        SkillManager.Inst.SkillListStandard(cp);
        GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        GameManager.Inst.UpdateArr(cp);
>>>>>>> minyoung
    }


    #region 현재상태에따라서
    //private void Eros2()
    //{
    //    Skill erosLove = SkillManager.Inst.GetSkillList("에로스의 사랑", GetCurrentPlayer(true));
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
    //    if (CheckSkillList("에로스의 사랑", GetCurrentPlayer(true)))
    //    {
    //        Eros2();
    //    }

    //    if (CheckSkillList("파도", GetCurrentPlayer(true)))
    //    {
    //        Wave();
    //    }
    //    if (CheckSkillList("수면", GetCurrentPlayer(true)))
    //    {
    //        Sleep();
    //    }
    //} 
    #endregion
    private void OnMouseUpEvent()
    {
        Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);

        // set card state

        // card attack 
        if (attack)
        {
            Card();
        }

<<<<<<< HEAD
        SetEChessState();

        if (eChessState == EChessState.Moving)
        {
            StartCoroutine(ReturnMovingCard());
        }

        else if (eChessState == EChessState.Skill)
        {
            SkillManager.Inst.UsingSkill(this);
        }

        else if (eChessState == EChessState.MovingAndSkill)
=======
            if (reference.isAttacking)
            {
                GameManager.Inst.attackings.Remove(reference);
                reference.attackCount = 0;
            }

            GameManager.Inst.attackings.Add(reference);
            //GameManager.Inst.CheckDeadSkillPiece(cp);
            reference.isAttacking = true;
        }

        if (eCardState == ECardState.Moving)
>>>>>>> minyoung
        {
            StartCoroutine(ReturnMovingCard());
            SkillManager.Inst.UsingSkill(this);
        }

<<<<<<< HEAD
        else
        {
            return;
        }
    }


=======
        else if (eCardState == ECardState.Skill)
        {
            SkillManager.Inst.UsingSkill(this);
        }

        else if (eCardState == ECardState.MovingAndSkill)
        {
            StartCoroutine(ReturnMovingCard());
            SkillManager.Inst.UsingSkill(this);
        }

        else
        {
            return;
        }
    }
  
>>>>>>> minyoung
    public void OnMouseUp()
    {
        if (TurnManager.Instance.isLoading) return;
        //if (TurnManager.Instance.GetIsActive()) return;
        if (isSelected) return;
        //if(PilSalGi.Inst.GetisUsePilSalGi())
        //{
        //    PilSalGi.Inst.SetselectPiece(GameManager.Inst.GetPosition(matrixX, matrixY));
        //    return;
        //}

        // change to coroutine for moving animation
        OnMouseUpEvent();
    }

    void SetEChessState()
    {
        // Change current state to usingCard if card was used
        if (GameManager.Inst.GetMoving() && GameManager.Inst.GetUsingSkill())
        {
<<<<<<< HEAD
            eChessState = EChessState.MovingAndSkill;
=======
            eCardState = ECardState.MovingAndSkill;
>>>>>>> minyoung
        }

        // If not, change the current state to moving
        else if (GameManager.Inst.GetMoving() && !GameManager.Inst.GetUsingSkill())
        {
<<<<<<< HEAD
            eChessState = EChessState.Moving;
        }

        else if (!GameManager.Inst.GetMoving() && GameManager.Inst.GetUsingSkill())
        {
            eChessState = EChessState.Skill;
        }
        else
        {
            eChessState = EChessState.Stop;
        }
    }



=======
            eCardState = ECardState.Moving;
        }

        else if (!GameManager.Inst.GetMoving() && GameManager.Inst.GetUsingSkill())
        {
            eCardState = ECardState.Skill;
        }
        else
        {
            eCardState = ECardState.Stop;
        }
    }
>>>>>>> minyoung


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

    public int GetPosX()
    {
        return matrixX;
    }

    public int GetPosY()
    {
        return matrixY;
    }

}
