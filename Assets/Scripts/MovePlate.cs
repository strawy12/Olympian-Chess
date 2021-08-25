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
            Debug.Log(matrixX + ", " + matrixY);
            ChessManager.Inst.MoveChessPiece(reference, matrixX, matrixY);
            
        }

        else if (eChessState == EChessState.Skill)
        {
            SkillManager.Inst.UsingSkill(this);
            SuperSkillManager.Inst.UsingSkill(this);

        }

        else if (eChessState == EChessState.MovingAndSkill)
        {

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

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
        Debug.Log(matrixX + ", " + matrixY);
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