using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public class SkillData
{
    public ChessData selectPieceDT;
    public ChessData selectPieceToDT;
    public int posX;
    public int posY;
    public int turnCnt;
    public string player;
    public string name;
    public int ID;

    public SkillData(ChessData selectPieceDT, ChessData selectPieceToDT, int posX, int posY, int turnCnt, string player, int ID, string name)
    {
        this.selectPieceDT = selectPieceDT;
        this.selectPieceToDT = selectPieceToDT;
        this.posX = posX;
        this.posY = posY;
        this.ID = ID;
        this.name = name;
        this.turnCnt = turnCnt;
        this.player = player;
    }
}

public class SkillBase : MonoBehaviourPunCallbacks
{
    protected SkillData skillData;
    protected MovePlate movePlate;
    protected ChessBase selectPiece;
    protected ChessBase selectPieceTo;

    protected void Awake()
    {
        skillData = new SkillData(null, null, 0, 0, 0, null, 0, null);
    }

    public virtual void UsingSkill() { }

    public virtual void StandardSkill() { }

    public virtual void ResetSkill() { }

    public void SendSkillData()
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(skillData, true);
        photonView.RPC("SetSkillData", RpcTarget.OthersBuffered, jsonData);
    }


    [PunRPC]
    public void SetSkillData(string jsonData)
    {
        SkillData sd = NetworkManager.Inst.LoadDataFromJson<SkillData>(jsonData);
        skillData = sd;
    }


    public void SetSelectPiece(ChessBase cp)
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(cp.GetChessData(), false);
        photonView.RPC("SetSelectPiece", RpcTarget.AllBuffered, jsonData);
    }

    [PunRPC]
    public void SetSelectPiece(string jsonData)
    {
        ChessData cp = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        skillData.selectPieceDT = cp;
        selectPiece = ChessManager.Inst.GetChessPiece(skillData.selectPieceDT);
    }

    public void SetSelectPieceTo(ChessBase cp)
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(cp.GetChessData(), false);
        photonView.RPC("SetSelectPieceTo", RpcTarget.AllBuffered, jsonData);
    }

    [PunRPC]
    public void SetSelectPieceTo(string jsonData)
    {
        ChessData cp = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        skillData.selectPieceToDT = cp;
        selectPieceTo = ChessManager.Inst.GetChessPiece(skillData.selectPieceToDT);
    }

    public ChessBase GetSelectPieceTo()
    {
        return selectPieceTo;
    }

    public ChessBase GetSelectPiece()
    {
        return selectPiece;
    }

    public string GetPlayer()
    {
        return skillData.player;

    }
    public void SetPalyer(string player)
    {
        skillData.player = player;
        SendSkillData();
    }
    public int GetPosX()
    {
        return skillData.posX;
    }

    public int GetPosY()
    {
        return skillData.posY;
    }
    public int GetID()
    {
        return skillData.ID;
    }
    public SkillData GetSkillData()
    {
        return skillData;
    }
    public string GetName()
    {
        return skillData.name;
    }

    public void SetPosX(int posX)
    {
        skillData.posX = posX;
        SendSkillData();
    }
    public void SetName(string name)
    {
        skillData.name = name;
        SendSkillData();
    }

    public void SetPosY(int posY)
    {
        skillData.posY = posY;
        SendSkillData();
    }

    public void TurnCntPlus()
    {
        skillData.turnCnt++;
        Debug.Log(123);
        SendSkillData();
    }

    public void SetID(int ID)
    {
        skillData.ID = ID;
        
        SendSkillData();
    }

    public void SetMovePlate(MovePlate movePlate)
    {
        this.movePlate = movePlate;
    }

    public void RPC_DestroySkill()
    {
        photonView.RPC("DestroySkill", RpcTarget.AllBuffered);
    }

    [PunRPC]
    protected void DestroySkill()
    {
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
}