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
        SetEChessState();
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
        // card attack 
        if (attack)
        {
            ChessManager.Inst.AttackChessPiece(this);
        }

        SetEChessState();

        if (eChessState == EChessState.Moving)
        {
            Debug.Log(1);

            ChessManager.Inst.MoveChessPiece(reference, matrixX, matrixY);
        }

        else if (eChessState == EChessState.Skill)
        {
            Debug.Log(2);

            SkillManager.Inst.UsingSkill(this);
        }

        else if (eChessState == EChessState.MovingAndSkill)
        {
            Debug.Log(3);
            ChessManager.Inst.MoveChessPiece(reference, matrixX, matrixY);
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
