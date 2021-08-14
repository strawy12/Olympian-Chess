using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBase : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public string player;

    private List<SkillBase> chosenSkill = new List<SkillBase>();
    protected int moveCnt = 0;
    public int attackCount = 0;
    protected int xBoard = -1;
    protected int yBoard = -1;

    public bool isMoving = false;
    public bool isAttacking = false;
    protected bool noneAttack = false;
    protected bool isSelecting = false;
    protected bool attackSelecting = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public virtual void MovePlate() { }

    public virtual void MovePosition() { }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    public void PlusMoveCnt()
    {
        moveCnt++;
    }

    public int GetMoveCnt()
    {
        return moveCnt;
    }

    public bool CheckClickChessPiece()
    {
        if (TurnManager.Instance.GetIsActive()) return true;
        if (SkillManager.Inst.CheckDontClickPiece(this)) return true;
        if (GameManager.Inst.IsGameOver()) return true;
        if (!TurnManager.Instance.GetCurrentPlayerTF()) return true;
        if (NetworkManager.Inst.GetPlayer() != player) return true;

        return false;
    }
    public void OnMouseUp()
    {
        if (CheckClickChessPiece()) return;

        SkillManager.Inst.CheckSkillCancel("¿¡·Î½ºÀÇ »ç¶û,¼ö¸é,Á×À½ÀÇ ¶¥,ÆÄµµ");
        GameManager.Inst.DestroyMovePlates();

        if (SkillManager.Inst.MoveControl(this))
        {
            return;
        }

        MovePlate(); // Instatiate

    }
    public SkillBase CheckSkillList(string name)
    {
        for (int i = 0; i < chosenSkill.Count; i++)
        {
            if (chosenSkill[i].gameObject.name == name)
            {
                return chosenSkill[i];
            }
        }
        return null;
    }

    public List<SkillBase> GetSkillList(string name)
    {
        List<SkillBase> _skillList = new List<SkillBase>();
        SkillBase skill;
        string[] names = name.Split(',');

        for (int i = 0; i < names.Length; i++)
        {
            skill = CheckSkillList(names[i]);
            if (skill != null)
            {
                _skillList.Add(skill);
            }
        }

        return _skillList;
    }

    public bool IsAttackSpawn(int x, int y)
    {
        if (noneAttack && ChessManager.Inst.GetPosition(x, y).name.Contains("king")) return true;
        else return false;
    }
    public void AddChosenSkill(SkillBase skill)
    {
        chosenSkill.Add(skill);
    }

    public void RemoveChosenSkill(SkillBase skill)
    {
        chosenSkill.Remove(skill);
    }

    public bool GetAttackSelecting()
    {
        return attackSelecting;
    }
    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    public void SetAttackSelecting(bool attackSelecting)
    {
        this.attackSelecting = attackSelecting;
    }

    public void SetNoneAttack(bool noneAttack)
    {
        this.noneAttack = noneAttack;
    }

    public void SetIsSelecting(bool _isHidden)
    {
        isSelecting = _isHidden;
    }
    public bool CheckIsMine()
    {
        if (player == TurnManager.Instance.GetCurrentPlayer())
            return true;
        else
            return false;
    }
}
