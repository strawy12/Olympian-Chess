using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviourPunCallbacks
{
    #region SingleTon
    //SkillManager singleton
    public static SkillManager Inst { get; private set; }
    void Awake() => Inst = this;
    #endregion

    #region SerializeField Var
    //List of skills currently in use
    [SerializeField] private List<SkillBase> skillList = new List<SkillBase>();
    [SerializeField] private GameObject skillPrefab;
    #endregion

    #region Var List
    // List of non-clickable chess pieces
    public List<ChessData> dontClickPiece = new List<ChessData>();
    private SkillBase selectingSkill;
    private bool isUsingCard = false;
    private int IDs = 0;
    #endregion

    #region System Check

    // Function returning whether the cp is in the dontClickPiece list.
    // if there is return true

    public bool CheckDontClickPiece(ChessBase cp)
    {
        ChessData chessData = cp.GetChessData();
        for (int i = 0; i < dontClickPiece.Count; i++)
        {
            Debug.Log(dontClickPiece[i].ID);
            if (dontClickPiece[i].ID == chessData.ID)
                return true;
        }
        return false;
    }

    // Function checking if there is a skill from skillList that is the same as name
    public bool CheckSkillList(SkillBase skill, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i] == skill && skillList[i].GetPlayer() == player)
                return true;
        }
        return false;
    }


    #endregion

    #region Script Access 


    // Function returning isUsingCard value
    public bool GetUsingCard()
    {
        return isUsingCard;
    }

    // Function setting isUsingCard value
    public void SetUsingCard(bool isUsingCard)
    {
        this.isUsingCard = isUsingCard;
    }

    // Function returning skill if there is a skill from skillList that is the same as name
    public List<SkillBase> GetSkillList(string name)
    {
        List<SkillBase> _skillList = new List<SkillBase>();

        string[] names = name.Split(',');
        for (int i = 0; i < skillList.Count; i++)
        {
            for(int j =0; j< names.Length; j++)
            {
                if (skillList[i].GetName() == names[j])
                    _skillList.Add(skillList[i]);
            }
        }
        return _skillList;
    }
    #endregion

    #region System

    public void SkillListCntPlus()
    {
        for (int i = 0; i < skillList.Count; i++)
        {

            skillList[i].TurnCntPlus();

            skillList[i].ResetSkill();
        }
    }

    // Function adding sk to skillList
    public void AddSkillList(SkillBase sb)
    {
        skillList.Add(sb);
    }

    // to be removed later
    public void RemoveSkillList(SkillBase sb)
    {
        skillList.Remove(sb);
        Destroy(sb.gameObject);
    }



    // Function adding the cp to dontClickPiece list
    public void AddDontClickPiece(ChessBase cp)
    {
        ChessData chessData = cp.GetChessData();
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, false);
        photonView.RPC("AddDontClickPiece", RpcTarget.AllBuffered, jsonData);

    }

    [PunRPC]
    private void AddDontClickPiece(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        for (int i = 0; i < dontClickPiece.Count; i++)
        {
            if (dontClickPiece[i].ID == chessData.ID)
            {
                return;
            }
        }
        dontClickPiece.Add(chessData);
    }

    // Function removing cp from dontClickPiece list
    public void RemoveDontClickPiece(ChessBase cp)
    {
        ChessData chessData = cp.GetChessData();
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, false);
        photonView.RPC("RemoveDontClickPiece", RpcTarget.AllBuffered, jsonData);
    }

    [PunRPC]
    public void RemoveDontClickPiece(string jsonData)
    {

        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);

        for (int i = 0; i < dontClickPiece.Count; i++)
        {
            if (dontClickPiece[i].ID == chessData.ID)
            {
                dontClickPiece.RemoveAt(i);

            }
        }
    }

    public void CheckSkillCancel(string name)
    {
        if (CardManager.Inst.GetSelectCard() == null) return;
        string[] names = name.Split(',');

        for (int i = 0; i < names.Length; i++)
        {
            if (CardManager.Inst.GetSelectCard().name == names[i])
            {
                CardManager.Inst.GetSelectCard().cardPrame.enabled = true;
                CardManager.Inst.GetSelectCard().card.enabled = true;
                if (GetSkillList(names[i]).Count == 0) return;
                SkillBase sb = GetSkillList(names[i])[0];

                CardManager.Inst.SetSelectCard(null);
                sb.RPC_DestroySkill();
                GameManager.Inst.SetUsingSkill(false);
                GameManager.Inst.SetMoving(true);
                return;
            }
        }
    }
    public SkillBase GetSkill(SkillData skillData)
    {
        if (skillData == null) return null;

        for (int i = 0; i < skillList.Count; i++)
        {

            if (skillData.ID == skillList[i].GetID())
            {
                return skillList[i];
            }

        }
        return null;
    }

    public bool CheckReturnMovePlate(int x, int y, string name)
    {
        List<SkillBase> skillList = GetSkillList(name);

        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i] != null)
            {
                if (skillList[i].GetPosX() == x && skillList[i].GetPosY() == y)
                {

                    return true;
                }

            }
        }
        return false;
    }

    public bool MoveControl(ChessBase cp)
    {
        List<SkillBase> _skillList = cp.GetSkillList("질서,바카스");
        int i;
        for (i = 0; i < _skillList.Count; i++)
        {
            if (GameManager.Inst.isBacchrs && _skillList[i].GetName() == "바카스")
            {
                _skillList[i].SetSelectPiece(cp);
            }

            _skillList[i].StandardSkill();
        }
        if (i != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AttackUsingSkill(MovePlate mp)
    {
        ChessBase cp = ChessManager.Inst.GetPosition(mp.GetPosX(), mp.GetPosY());
        List<SkillBase> _skillList = cp.GetSkillList("출산,아테나의 방패,에로스의 사랑,동귀어진");
        for (int i = 0; i < _skillList.Count; i++)
        {
            _skillList[i].SetPosX(mp.Getreference().GetXBoard());
            _skillList[i].SetPosY(mp.Getreference().GetYBoard());
            _skillList[i].SetMovePlate(mp);
            if (_skillList[i].GetName() == "에로스의 사랑")
            {
                _skillList[i].SetPosX(mp.GetChessPiece().GetXBoard());
                _skillList[i].SetPosY(mp.GetChessPiece().GetYBoard());
            }
            _skillList[i].StandardSkill();
        }
    }

    public void UsingSkill(MovePlate mp)
    {
        if (skillList.Count < 1) return;

        SkillBase sb = skillList[skillList.Count - 1];
        sb.SetPosX(mp.GetPosX());
        sb.SetPosY(mp.GetPosY());
        sb.SetSelectPiece(mp.Getreference());
        sb.StandardSkill();

        CardManager.Inst.SetSelectCard(null);
    }

    public void SetIds(SkillBase sk)
    {
        if (sk.GetSkillData().player == "white")
        {
            sk.SetID(IDs + 100);
        }
        else if (sk.GetSkillData().player == "black")
        {
            sk.SetID(IDs + 200);
        }

        IDs++;
    }

    // Function spawning skill prefab
    public SkillBase SpawnSkillPrefab(Card card, ChessBase chessPiece)
    {
        SkillBase sb = CheckSkill(card).GetComponent<SkillBase>();
        if (sb == null) return null;

        sb.SetPalyer(NetworkManager.Inst.GetPlayer());
        sb.SetName(card.carditem.name);
        SetIds(sb);
        selectingSkill = sb;
        if (chessPiece != null)
        {
            chessPiece.AddChosenSkill(sb);
            sb.SetSelectPiece(chessPiece);
        }
        sb.UsingSkill();

        return sb;
    }

    [PunRPC]
    private void AddSkill(int skillID, string name)
    {
        GameObject obj = PhotonView.Find(skillID).gameObject;
        switch (name)
        {
            case "천벌":
                obj.AddComponent<HeavenlyPunishment>();
                break;

            case "에로스의 사랑":
                obj.AddComponent<LoveOfEros>();
                break;

            case "수면":
                obj.AddComponent<Sleep>();
                break;

            case "아테나의 방패":
                obj.AddComponent<ShieldOfAthena>();
                break;

            case "파도":
                obj.AddComponent<Wave>();
                break;

            case "서풍":
                obj.AddComponent<WestWind>();
                break;

            case "수중감옥":
                obj.AddComponent<OceanJail>();
                break;

            case "순보":
                obj.AddComponent<Law>();
                break;

            case "죽음의 땅":
                obj.AddComponent<GroundOfDeath>();
                break;

            case "전쟁광":
                obj.AddComponent<WarBuff>();
                break;

            case "음악":
                obj.AddComponent<Music>();
                break;

            case "신속":
                obj.AddComponent<Rush>();
                break;

            case "여행자":
                obj.AddComponent<Traveler>();
                break;

            case "동귀어진":
                obj.AddComponent<StreetFriend>();
                break;

            case "바카스":
                obj.AddComponent<Bacchrs>();
                break;

            case "시간왜곡":
                obj.AddComponent<TimeWarp>();
                break;

            case "제물":
                obj.AddComponent<Offering>();
                break;

            case "정의구현":
                obj.AddComponent<Justice>();
                break;

            case "출산":
                obj.AddComponent<GiveBirth>();
                break;

            case "달빛":
                obj.AddComponent<MoonLight>();
                break;

            case "후진":
                obj.AddComponent<Back>();
                break;

            case "부활":
                obj.AddComponent<Ghost>();
                break;

            case "시간정지":
                obj.AddComponent<TimeStop>();
                break;

            case "만취":
                obj.AddComponent<Drunkenness>();
                break;
        }

        skillList.Add(obj.GetComponent<SkillBase>());

    }

    private GameObject CheckSkill(Card card)
    {
        GameObject obj = null;
        int skillID;
        obj = NetworkManager.Inst.SpawnObject(skillPrefab);
        obj.name = card.carditem.name;
        obj.transform.SetParent(null);
        skillID = obj.GetPhotonView().ViewID;
        //AddSkill(obj, card.carditem.name);

        photonView.RPC("AddSkill", RpcTarget.AllBuffered, skillID, card.carditem.name);
        return obj;
    }

    #endregion

}
