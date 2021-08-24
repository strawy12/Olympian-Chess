using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class ChessData
{
    public string chessPiece;
    public string player;
    public int moveCnt;
    public int attackCount;
    public int xBoard;
    public int yBoard;
    public int ID;

    public bool isAttacking;
    public bool isMoving;
    public bool noneAttack;
    public bool isSelecting;
    public bool attackSelecting;

    public List<SkillData> chosenSkill;
    public ChessData(string player,int ID, string chessPiece, int moveCnt, int attackCount, int xBoard, int yBoard,bool isMoving, bool isAttacking, bool noneAttack, bool isSelecting, bool attackSelecting, List<SkillData> chosenSkill)
    {
        this.chessPiece = chessPiece;
        this.player = player;
        this.ID = ID;
        this.moveCnt = moveCnt;
        this.attackCount = attackCount;
        this.xBoard = xBoard;
        this.yBoard = yBoard;
        this.isAttacking = isAttacking;
        this.isMoving = isMoving;
        this.noneAttack = noneAttack;
        this.isSelecting = isSelecting;
        this.attackSelecting = attackSelecting;
        this.chosenSkill = chosenSkill;
    }
}

[System.Serializable]
public class ChessBase : MonoBehaviourPunCallbacks
{
    public SpriteRenderer spriteRenderer { get; private set; }
    [SerializeField] private string player;
    protected ChessData chessData;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        chessData = new ChessData(player, 0, gameObject.name, 0, 0, 0, 0, false, false, false, false, false, new List<SkillData>());
    }

    public virtual void MovePlate() { }

    public virtual void MovePosition() { }



    private void SendChessData()
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, true);
        photonView.RPC("SetChessData", RpcTarget.OthersBuffered, jsonData);
    }

    [PunRPC]
    public void SetChessData(string jsonData)
    {
        ChessData cd = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        chessData = cd;
    }

    public bool CheckClickChessPiece()
    {
        if (TurnManager.Instance.GetIsActive()) return true;
        if (TurnManager.Instance.isLoading) return true;
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
    public SkillData CheckSkillList(string name)
    {
        for (int i = 0; i < chessData.chosenSkill.Count; i++)
        {
            if (chessData.chosenSkill[i].name == name)
            {
                return chessData.chosenSkill[i];
            }
        }
        return null;
    }

    public List<SkillBase> GetSkillList(string name)
    {
        List<SkillBase> _skillList = new List<SkillBase>();
        SkillData skill;
        string[] names = name.Split(',');

        for (int i = 0; i < names.Length; i++)
        {
            skill = CheckSkillList(names[i]);
            if (skill != null)
            {
                _skillList.Add(SkillManager.Inst.GetSkill(skill));
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
        chessData.chosenSkill.Add(skill.GetSkillData());
        SendChessData();
    }

    public void RemoveChosenSkill(SkillBase skill)
    {
        chessData.chosenSkill.Remove(skill.GetSkillData());
        SendChessData();
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
        photonView.RPC("SettingCoords", RpcTarget.AllBuffered);
        SettingCoords();
    }
    [PunRPC]
    public void SettingCoords()
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
        photonView.RPC("SetCoordsAnimationCo", RpcTarget.AllBuffered);
    }

    [PunRPC]
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

    public void DestroyChessPiece()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    #region Get / Set from ChessData
    public void SetXBoard(int x)
    {
        chessData.xBoard = x;
        SendChessData();
    }

    public void SetYBoard(int y)
    {
        chessData.yBoard = y;
        SendChessData();
    }
    public void SetID(int ID)
    {
        chessData.ID = ID;
        SendChessData();
    }

    public void PlusMoveCnt()
    {
        chessData.moveCnt++;
        SendChessData();
    }

    public void PlusAttackCnt()
    {
        chessData.attackCount++;
        SendChessData();
    }

    public void ClearAttackCnt()
    {
        chessData.attackCount = 0;
        SendChessData();
    }

    public void ClearMoveCnt()
    {
        chessData.moveCnt = 0;
        SendChessData();
    }


    public void SetNoneAttack(bool noneAttack)
    {
        chessData.noneAttack = noneAttack;;
        SendChessData();
    }
    public void SetAttackSelecting(bool attackSelecting)
    {
        chessData.attackSelecting = attackSelecting;
        SendChessData();
    }

    public void SetIsSelecting(bool isSelecting)
    {
        chessData.isSelecting = isSelecting;
        SendChessData();
    }

    public void SetIsAttacking(bool isAttacking)
    {
        chessData.isAttacking = isAttacking;
        SendChessData();
    }

    public void SetIsMoving(bool isMoving)
    {
        chessData.isMoving = isMoving;
        SendChessData();
    }

    public int GetXBoard()
    {
        return chessData.xBoard;
    }
    public int GetYBoard()
    {
        return chessData.yBoard;
    }

    public int GetMoveCnt()
    {
        return chessData.moveCnt;
    }

    public int GetAttackCnt()
    {
        return chessData.attackCount;
    }

    public bool GetAttackSelecting()
    {
        return chessData.attackSelecting;
    }

    public bool GetIsAttacking()
    {
        return chessData.isAttacking;
    }

    public bool GetIsSelecting()
    {
        return chessData.isSelecting;
    }

    public bool GetNoneAttack()
    {
        return chessData.noneAttack;
    }    
    public bool GetIsMoving()
    {
        return chessData.isMoving;
    }

    public int GetID()
    {
        return chessData.ID;
    }

    public ChessData GetChessData()
    {
        return chessData;
    }
    #endregion
}
