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
    enum ECardState { Moving, Skill, MovingAndSkill, Stop }

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

        reference.isMoving = true;
        TurnManager.Instance.ButtonColor();
    }

    private IEnumerator ReturnMovingCard()
    {
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

        if(cp.GetAttackSelecting())
        {
            SkillManager.Inst.AttackUsingSkill(this);
        }

        SetECardState();

        if(eCardState == ECardState.Stop)
        {
            return;
        }
        
        Destroy(cp.gameObject);
        SkillManager.Inst.SkillListStandard(cp);
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

    //private void Wave()
    //{
    //    Skill wave = SkillManager.Inst.GetSkillList("파도", GetCurrentPlayer(true));
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
    //    Skill sleep = SkillManager.Inst.GetSkillList("수면", GetCurrentPlayer(true));
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
        SetECardState();

        // card attack 
        if (attack)
        {
            Card();

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
        {
            StartCoroutine(ReturnMovingCard());
        }

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
  
    public void OnMouseUp()
    {
        if (TurnManager.Instance.isLoading) return;
        if (isSelected) return;
        //if(PilSalGi.Inst.GetisUsePilSalGi())
        //{
        //    PilSalGi.Inst.SetselectPiece(GameManager.Inst.GetPosition(matrixX, matrixY));
        //    return;
        //}
        Debug.Log("말 이동 성공");
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
        else if (GameManager.Inst.GetMoving() && !GameManager.Inst.GetUsingSkill())
        {
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

    public int GetPosX()
    {
        return matrixX;
    }

    public int GetPosY()
    {
        return matrixY;
    }

}
