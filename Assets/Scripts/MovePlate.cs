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
    enum EChessState { Moving, Skill, MovingAndSkill, Stop }

    ChessBase reference = null;

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

<<<<<<< HEAD
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

        reference.isMoving = true;
    }

    private IEnumerator ReturnMovingCard()
    {
        TurnManager.Instance.ButtonColor();
        yield return MovingCard();
    }


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
=======

>>>>>>> suan
    private void OnMouseUpEvent()
    {

        // set card state

        // card attack 
        if (attack)
        {
<<<<<<< HEAD
            Card();
            if (reference.isAttacking)
            {
                GameManager.Inst.attackings.Remove(reference);
                reference.attackCount = 0;
            }

            GameManager.Inst.attackings.Add(reference);
            reference.isAttacking = true;
        }
=======
            ChessManager.Inst.AttackChessPiece(matrixX, matrixY);
>>>>>>> suan

        SetEChessState();

        if (eChessState == EChessState.Moving)
        {
            ChessManager.Inst.MoveChessPiece(reference, matrixX, matrixY);
        }

        else if (eChessState == EChessState.Skill)
        {
            SkillManager.Inst.UsingSkill(this);
        }

        else if (eChessState == EChessState.MovingAndSkill)
        {
            StartCoroutine(ReturnMovingCard());
            SkillManager.Inst.UsingSkill(this);
        }

        else
        {
            return;
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
<<<<<<< HEAD

=======
>>>>>>> suan
        // change to coroutine for moving animation
        OnMouseUpEvent();
    }

    void SetEChessState()
    {
        // Change current state to usingCard if card was used
        if (GameManager.Inst.GetMoving() && GameManager.Inst.GetUsingSkill())
        {
            eChessState = EChessState.MovingAndSkill;
        }
        // If not, change the current state to moving
        else if (GameManager.Inst.GetMoving() && !GameManager.Inst.GetUsingSkill())
        {
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

    public void Setreference(ChessBase obj)
    {
        reference = obj;
    }

    public ChessBase Getreference()
    {
        return reference;
    }
    public ChessBase GetChessPiece()
    {
        return ChessManager.Inst.GetPosition(matrixX, matrixY);
    }

}
