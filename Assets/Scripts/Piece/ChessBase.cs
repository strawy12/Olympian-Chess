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
    public void OnMouseUp()
    {
        Vector3 P = new Vector3(transform.position.x, transform.position.y, 0f);
        ParticleManager.Instance.AddParticle(ParticleManager.ParticleType.W_ChessPieceClick, P);
        //ParticleManager.Instance.AddParticle(ParticleManager.ParticleType.B_ChessPieceClick, P);
        //ParticleManager.Instance.AddParticle(ParticleManager.ParticleType.S, P);

        Debug.Log("마우스 올림");
        if (TurnManager.Instance.GetIsActive()) return;
        if (!GameManager.Inst.IsGameOver() && GameManager.Inst.GetCurrentPlayer() == player)
        {
            SkillManager.Inst.CheckSkillCancel("에로스의 사랑,수면,죽음의 땅,파도");
            GameManager.Inst.DestroyMovePlates();

            if (SkillManager.Inst.MoveControl(this))
            {
                return;
            }

            MovePlate(); // Instatiate
        }

        
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
        if (player == GameManager.Inst.GetCurrentPlayer())
            return true;
        else
            return false;
    }
}
