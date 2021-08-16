using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ChessData
{
    public int moveCnt;
    public int attackCount;
    public int xBoard;
    public int yBoard;

    public bool isMoving;
    public bool isAttacking;
    public bool noneAttack;
    public bool isSelecting;
    public bool attackSelecting;

    public ChessData(int moveCnt, int attackCount, int xBoard, int yBoard, bool isMoving, bool isAttacking, bool noneAttack, bool isSelecting, bool attackSelecting)
    {
        this.moveCnt = moveCnt;
        this.attackCount = attackCount;
        this.xBoard = xBoard;
        this.yBoard = yBoard;
        this.isMoving = isMoving;
        this.isAttacking = isAttacking;
        this.noneAttack = noneAttack;
        this.isSelecting = isSelecting;
        this.attackSelecting = attackSelecting;
    }
}


public class ChessBase : MonoBehaviourPunCallbacks//, IPunObservable
{
    public SpriteRenderer spriteRenderer { get; private set; }

    public string player;

    protected List<SkillBase> chosenSkill = new List<SkillBase>();

    public ChessData chessData { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        chessData = new ChessData(0, 0, 0, 0, false, false, false, false, false);
    }
    private void Start()
    {
        
    }


    public virtual void MovePlate() { }

    public virtual void MovePosition() { }

    public int GetXBoard()
    {
        return chessData.xBoard;
    }
    public int GetYBoard()
    {
        return chessData.yBoard;
    }
    public void SetXBoard(int x)
    {
        chessData.xBoard = x;
    }

    public void SetYBoard(int y)
    {
        chessData.yBoard = y;
    }

    public void PlusMoveCnt()
    {
        chessData.moveCnt++;
    }

    public int GetMoveCnt()
    {
        return chessData.moveCnt;
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
        if (chessData.noneAttack && ChessManager.Inst.GetPosition(x, y).name.Contains("king")) return true;
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
        return chessData.attackSelecting;
    }
    public void SetIsMoving(bool isMoving)
    {
        chessData.isMoving = isMoving;
    }

    public void SetAttackSelecting(bool attackSelecting)
    {
        chessData.attackSelecting = attackSelecting;
    }

    public void SetNoneAttack(bool noneAttack)
    {
        chessData.noneAttack = noneAttack;
    }

    public void SetIsSelecting(bool _isHidden)
    {
        chessData.isSelecting = _isHidden;
    }
    public bool CheckIsMine()
    {
        if (player == TurnManager.Instance.GetCurrentPlayer())
            return true;
        else
            return false;
    }
    public void SetCoords()
    {
        float x = chessData.xBoard;
        float y = chessData.yBoard;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        // Aligns according the board
        transform.position = new Vector3(x, y, -1.0f);
    }
    public void SetCoordsAnimation()
    {
        StartCoroutine(SetCoordsAnimationCo());
    }
    public IEnumerator SetCoordsAnimationCo()
    {
        Vector3 startPos = transform.position;

        float x = chessData.xBoard;
        float y = chessData.yBoard;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        // end position
        Vector3 endPos = new Vector3(x, y, -1.0f);
        // calculate distance for move speed
        float distance = (endPos - startPos).magnitude;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / distance * 10f;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(chessData);
    //    }
    //    else
    //    {
    //        chessData = (ChessData)stream.ReceiveNext();
    //    }
    //}
}
