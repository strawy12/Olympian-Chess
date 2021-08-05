using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    Chessman reference = null;
    [SerializeField] ECardState eCardState;
    // Board positions(not wolrd Positions) 
    int matrixX;
    int matrixY;
    // false: movement, true: attacking
    public bool attack = false;
    private bool isSelected = false;
    public bool isOD = false;

    enum ECardState { Moving, UsingCard }

    public void Start()
    {
        AttackChess();
    }
    private void AttackChess()
    {
        // °ø°ÝÇÏ´Â ÇÔ¼ö
        if (attack)
        {
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
                yield return 0;
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

        reference.isMoving = true;
        TurnManager.Instance.ButtonColor();
    }

    private IEnumerator OnMouseUpEvent()
    {
        //== set card state
        SetECardState();

        //== card attack 
        if (attack)
        {
            Chessman cp = GameManager.Inst.GetPosition(matrixX, matrixY);
            Skill athen = SkillManager.Inst.GetSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false));

            if (SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(true)) || SkillManager.Inst.CheckSkillList("ÀüÀï±¤", GetCurrentPlayer(false)))
            {
                Skill sk = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(true));
                if (sk == null)
                {
                    sk = SkillManager.Inst.GetSkillList("ÀüÀï±¤", GetCurrentPlayer(false));
                }
                if (sk.cnt != 0)
                {
                    if (cp.name == "black_king" || cp.name == "white_king")
                        yield break;
                }
            }
            if (SkillManager.Inst.CheckSkillList("´Þºû", GetCurrentPlayer(true)))
            {
                if (cp.name == "black_king" || cp.name == "white_king")
                    yield break;
            }

            if (CheckSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(false)) && cp == athen.GetSelectPiece())
            {
                athen.IsAttack(true);
                athen.CheckAS();
                cp.DestroyMovePlates();
                TurnManager.Instance.ButtonColor();
                yield break;
            }

            if (CheckSkillList("Ãâ»ê", GetCurrentPlayer(false)))
            {
                Chessman cm;
                Skill sk = SkillManager.Inst.GetSkillList("Ãâ»ê", GetCurrentPlayer(false));
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

                reference.SetCoords();
                GameManager.Inst.SetPosition(reference);
                reference.DestroyMovePlates();
                reference.SetIsMoved(true);
                Destroy(cp.gameObject);
                sk.SelectPieceNull();
                SkillManager.Inst.DeleteSkillList(sk);
                //sk.
                Destroy(sk.gameObject);
                GameManager.Inst.UpdateArr(cp);
                GameManager.Inst.AddArr(cm);
                TurnManager.Instance.ButtonColor();

                yield break;
            }

            if (CheckSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(false)))
            {
                Skill sk = SkillManager.Inst.GetSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(false));
                if (sk == null)
                {
                    yield break;
                }
                if (cp != sk.GetSelectPieceTo())
                {
                    yield return 0;    
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
                    reference.DestroyMovePlates();
                    TurnManager.Instance.ButtonColor();
                    yield break;
                }
            }

            if (CheckSkillList("±æµ¿¹«", GetCurrentPlayer(false)))
            {
                Skill sk = SkillManager.Inst.GetSkillList("±æµ¿¹«", GetCurrentPlayer(false));
                if (sk == null) yield break;

                if (reference.name == "black_king" || reference.name == "white_king")
                {
                    Debug.Log("¿Õ¿Õ¿Õ");
                }

                else
                {
                    reference.DestroyMovePlates();
                    Destroy(reference.gameObject);
                    Destroy(cp.gameObject);
                    GameManager.Inst.SetPositionEmpty(reference.GetXBoard(), reference.GetYBoard());
                    GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
                    GameManager.Inst.UpdateArr(reference);
                    GameManager.Inst.UpdateArr(cp);
                    yield break;
                }

                TurnManager.Instance.ButtonColor();
                sk.ReloadStreetFriend();
                sk.SetIsUsingCard(false);
            }
            if (GetCurrentPlayer(false) == cp.player)
            {
                PilSalGi.Inst.attackCntPlus();
            }

            Destroy(cp.gameObject);
            GameManager.Inst.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
            GameManager.Inst.UpdateArr(cp);
            
            

            if (reference.isAttacking)
            {
                GameManager.Inst.attackings.Remove(reference);
                reference.attackCount = 0;
            }

            GameManager.Inst.attackings.Add(reference);
            GameManager.Inst.CheckDeadSkillPiece(cp);
            reference.isAttacking = true;
        }
        //== card move
        // Repeat the game controller to see if this is an attack
        // Reset the position so that the position is empty
        if (eCardState == ECardState.Moving)
        {
            yield return MovingCard();
        }


        else if (eCardState == ECardState.UsingCard)
        {
            if (CheckSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(true)))
            {
                Skill sk = SkillManager.Inst.GetSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(true));
                if (sk == null) yield break;

                sk.SetSelectPieceTo(GameManager.Inst.GetPosition(reference.GetXBoard(), reference.GetYBoard()));
                reference.DestroyMovePlates();
                sk.StartLOE_Effect();
                CardManager.Inst.ChangeIsUse(true);

                //TurnManager.Inst.EndTurn();
                SkillManager.Inst.SetIsUsingCard(false);
            }

            if (CheckSkillList("ÆÄµµ", GetCurrentPlayer(true)))
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
                reference.DestroyMovePlates();
                TurnManager.Instance.SetIsActive(true);
                CardManager.Inst.ChangeIsUse(true);
            }
            if (CheckSkillList("¼ö¸é", GetCurrentPlayer(true)))
            {
                Debug.Log("³È");
                Skill sk = SkillManager.Inst.GetSkillList("¼ö¸é", GetCurrentPlayer(true));
                if (sk == null) yield break;
                sk.SetSelectPieceTo(GameManager.Inst.GetPosition(reference.GetXBoard(), reference.GetYBoard()));
                sk.GetSelectPiece().SetIsMoved(false);
                sk.GetSelectPieceTo().SetIsMoved(false);
                reference.DestroyMovePlates();
                sk.StartSP_SkillEffect();
                //TurnManager.Inst.EndTurn();
                CardManager.Inst.ChangeIsUse(true);
                SkillManager.Inst.SetIsUsingCard(false);
            }
        }

        yield return null;
    }

    public void OnMouseUp()
    {
        if (TurnManager.Instance.isLoading) return;
        if (isSelected) return;
        if(PilSalGi.Inst.GetisUsePilSalGi())
        {
            PilSalGi.Inst.SetselectPiece(GameManager.Inst.GetPosition(matrixX, matrixY));
            return;
        }
        Debug.Log("¾ßÈ£");
        // change to coroutine for moving animation
        StartCoroutine(OnMouseUpEvent());
    }

    void SetECardState()
    {
        if (SkillManager.Inst.UsingCard())
            eCardState = ECardState.UsingCard;

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
