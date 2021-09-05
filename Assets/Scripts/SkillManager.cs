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
    [SerializeField] private GameObject skillEffectPrefab;
    #endregion

    #region Var List
    // List of non-clickable chess pieces
    public List<ChessData> dontClickPiece = new List<ChessData>();
    public List<ChessData> godPieces = new List<ChessData>();
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
    }

    // Function adding the cp to dontClickPiece list
    public void AddDontClickPiece(ChessBase cp, bool isSend = false)
    {
        if(isSend)
        {
            ChessData chessData = cp.GetChessData();
            string jsonData = DataManager.Inst.SaveDataToJson(chessData, false);
            photonView.RPC("AddDontClickPiece", RpcTarget.AllBuffered, jsonData);
        }
        else
        {
            if(Array.Exists(dontClickPiece.ToArray(), x => x.ID == cp.GetChessData().ID))
            {
                return;
            }

            dontClickPiece.Add(cp.GetChessData());
        }
    }

    [PunRPC]
    private void AddDontClickPiece(string jsonData)
    {
        ChessData chessData = DataManager.Inst.LoadDataFromJson<ChessData>(jsonData);

        if (Array.Exists(dontClickPiece.ToArray(), x => x.ID == chessData.ID))
        {
            return;
        }

        dontClickPiece.Add(chessData);
    }

    // Function removing cp from dontClickPiece list
    public void RemoveDontClickPiece(ChessBase cp, bool isSend = false)
    {
        if (isSend)
        {
            ChessData chessData = cp.GetChessData();
            string jsonData = DataManager.Inst.SaveDataToJson(chessData, false);
            photonView.RPC("RemoveDontClickPiece", RpcTarget.AllBuffered, jsonData);
        }
        else
        {
            ChessData cd = dontClickPiece.Find(x => x.ID == cp.GetID());
            dontClickPiece.Remove(cd);
        }
    }

    [PunRPC]
    public void RemoveDontClickPiece(string jsonData)
    {
        ChessData chessData = DataManager.Inst.LoadDataFromJson<ChessData>(jsonData);

        ChessData cd = dontClickPiece.Find(x => x.ID == chessData.ID);
        dontClickPiece.Remove(cd);
    }

    public void AddGodPieces(ChessBase cp)
    {
        ChessData chessData = cp.GetChessData();
        string jsonData = DataManager.Inst.SaveDataToJson(chessData, false);
        photonView.RPC("AddGodPieces", RpcTarget.AllBuffered, jsonData);

    }

    [PunRPC]
    private void AddGodPieces(string jsonData)
    {
        ChessData chessData = DataManager.Inst.LoadDataFromJson<ChessData>(jsonData);

        if (Array.Exists(godPieces.ToArray(), x => x.ID == chessData.ID))
        {
            return;
        }

        godPieces.Add(chessData);
    }

    public void RemoveGodPieces(ChessData chessData)
    {
        string jsonData = DataManager.Inst.SaveDataToJson(chessData, false);
        photonView.RPC("RemoveGodPieces", RpcTarget.AllBuffered, jsonData);
    }

    [PunRPC]
    public void RemoveGodPieces(string jsonData)
    {
        ChessData chessData = DataManager.Inst.LoadDataFromJson<ChessData>(jsonData);

        ChessData cd = godPieces.Find(x => x.ID == chessData.ID);
        dontClickPiece.Remove(cd);
    }

    public List<ChessData> GetGodPieces()
    {
        return godPieces;
    }

    public void CheckSkillCancel(string name)
    {
        if (CardManager.Inst.GetSelectCard() == null) return;

        var skillList = GetSkillList(name);


        for (int i = 0; i < skillList.Count; i++)
        {
            if (CardManager.Inst.GetSelectCard().name == skillList[i].GetName())
            {
                CardManager.Inst.GetSelectCard().SetActive(true);
                CardManager.Inst.SetSelectCardNull();
                skillList[i].RPC_DestroySkill();
                GameManager.Inst.SetUsingSkill(false);
                GameManager.Inst.SetMoving(true);
            }
        }
        CardManager.Inst.SetSelectCardNull();
    }
    public SkillBase GetSkill(SkillData skillData)
    {
        if (skillData == null) return null;

        return skillList.Find(x => x.GetID() == skillData.ID);
    }

    public bool CheckReturnMovePlate(int x, int y, string name)
    {
        List<SkillBase> skillList = GetSkillList(name);

        return Array.Exists(skillList.ToArray(), z => z.GetPosX() == x && z.GetPosY() == y);
    }

    public bool MoveControl(ChessBase cp)
    {
        List<SkillBase> _skillList = cp.GetSkillList("����,��ī��");
        int i;
        for (i = 0; i < _skillList.Count; i++)
        {
            if (GameManager.Inst.isBacchrs && _skillList[i].GetName() == "��ī��")
            {
                _skillList[i].SetSelectPiece(cp);
            }

            _skillList[i].StandardSkill();
        }

        if (i != 0)
            return true;

        else
            return false;
    }

    public void AttackUsingSkill(MovePlate mp)
    {
        ChessBase cp = ChessManager.Inst.GetPosition(mp.GetPosX(), mp.GetPosY());
        List<SkillBase> _skillList = cp.GetSkillList("���,���׳��� ����,���ν��� ���,���;���");
        for (int i = 0; i < _skillList.Count; i++)
        {
            _skillList[i].SetPosX(mp.Getreference().GetXBoard());
            _skillList[i].SetPosY(mp.Getreference().GetYBoard());
            _skillList[i].SetMovePlate(mp);

            if (_skillList[i].GetName() == "���ν��� ���")
            {
                _skillList[i].SetPosX(mp.GetChessPiece().GetXBoard());
                _skillList[i].SetPosY(mp.GetChessPiece().GetYBoard());
            }

            _skillList[i].StandardSkill();
        }
    }

    public GameObject SkillEffectSpawn()
    {
        return Instantiate(skillEffectPrefab, transform);
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
            sk.SetID(IDs + 100);

        else if (sk.GetSkillData().player == "black")
            sk.SetID(IDs + 200);

        IDs++;
    }

    // Function spawning skill prefab
    public SkillBase SpawnSkillPrefab(Card card, ChessBase chessPiece)
    {
        card.SetActive(false);
        SkillBase sb = CheckSkill(card).GetComponent<SkillBase>();
        if (sb == null) return null;

        sb.SetPalyer(GameManager.Inst.GetPlayer());
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
        SkillBase sb;
        switch (name)
        {
            case "õ��":
                obj.AddComponent<HeavenlyPunishment>();
                break;

            case "���ν��� ���":
                obj.AddComponent<LoveOfEros>();
                break;

            case "����":
                obj.AddComponent<Sleep>();
                break;

            case "���׳��� ����":
                obj.AddComponent<ShieldOfAthena>();
                break;

            case "�ĵ�":
                obj.AddComponent<Wave>();
                break;

            case "��ǳ":
                obj.AddComponent<WestWind>();
                break;

            case "���߰���":
                obj.AddComponent<OceanJail>();
                break;

            case "����":
                obj.AddComponent<Law>();
                break;

            case "������ ��":
                obj.AddComponent<GroundOfDeath>();
                break;

            case "���ﱤ":
                obj.AddComponent<WarBuff>();
                break;

            case "����":
                obj.AddComponent<Music>();
                break;

            case "�ż�":
                obj.AddComponent<Rush>();
                break;

            case "������":
                obj.AddComponent<Traveler>();
                break;

            case "���;���":
                obj.AddComponent<StreetFriend>();
                break;

            case "��ī��":
                obj.AddComponent<Bacchrs>();
                break;

            case "�ð��ְ�":
                obj.AddComponent<TimeWarp>();
                break;

            case "����":
                obj.AddComponent<Offering>();
                break;

            case "���Ǳ���":
                obj.AddComponent<Justice>();
                break;

            case "���":
                obj.AddComponent<GiveBirth>();
                break;

            case "�޺�":
                obj.AddComponent<MoonLight>();
                break;

            case "����":
                obj.AddComponent<Back>();
                break;

            case "����":
                obj.AddComponent<Ghost>();
                break;

            case "�ð�����":
                obj.AddComponent<TimeStop>();
                break;

            case "����":
                obj.AddComponent<Drunkenness>();
                break;
        }
        sb = obj.GetComponent<SkillBase>();
        skillList.Add(sb);
        
    }

    private GameObject CheckSkill(Card card)
    {
        GameObject obj = null;
        int skillID;
        obj = NetworkManager.Inst.SpawnObject(skillPrefab);
        obj.name = card.carditem.name;
        obj.transform.SetParent(null);
        skillID = obj.GetPhotonView().ViewID;
        //AddSkill(obj, card.carditem.name)
        photonView.RPC("AddSkill", RpcTarget.AllBuffered, skillID, card.carditem.name);
        return obj;
    }

    #endregion
}
