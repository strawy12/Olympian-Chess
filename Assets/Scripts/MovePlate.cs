using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    #region 변수
    [SerializeField] ECardState eCardState;
    // Board positions(not wolrd Positions) 
    int matrixX;
    int matrixY;
    // false: movement, true: attacking
    [Header("속성")]
    public bool attack = false;
    public bool isOD = false;

    private bool isSelected = false;
    enum ECardState { Moving, Skill, MovingAndSkill }

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


    private void OnMouseUpEvent()
    {

        // set card state
        SetECardState();

        // card attack 
        if (attack)
        {
            ChessManager.Inst.AttackChessPiece(matrixX, matrixY);

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
            ChessManager.Inst.MoveChessPiece(reference, matrixX, matrixY);
        }

        else if (eCardState == ECardState.Skill)
        {
            SkillManager.Inst.UsingSkill(this);
        }
        else if(eCardState == ECardState.MovingAndSkill)
        {

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
