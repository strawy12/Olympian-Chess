using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
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
    public List<Chessman> dontClickPiece = new List<Chessman>();
    private SkillBase selectingSkill;
    private bool isUsingCard = false;
    

    #endregion

    #region System Check

    // Function returning whether the cp is in the dontClickPiece list.
    // if there is return true

    public bool CheckDontClickPiece(Chessman cp)
    {
        for (int i = 0; i < dontClickPiece.Count; i++)
        {
            if (dontClickPiece[i] == cp)
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
    public List<SkillBase> GetSkillList(string name, string player)
    {
        List<SkillBase> _skillList = new List<SkillBase>();

        string[] names = name.Split(',');
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPlayer() == player)
                _skillList.Add(skillList[i]);
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
    public void AddDontClickPiece(Chessman cp)
    {
        dontClickPiece.Add(cp);
    }

    // Function removing cp from dontClickPiece list
    public void RemoveDontClickPiece(Chessman cp)
    {
        dontClickPiece.Remove(cp);
    }

    public void UsingSkill(MovePlate mp)
    {
        SkillBase sb = skillList[skillList.Count - 1];
        sb.SetPosX(mp.GetPosX());
        sb.SetPosY(mp.GetPosY());
        sb.StandardSkill();
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
    }
    public void AttackUsingSkill(MovePlate mp)
    {
        string player = GameManager.Inst.GetCurrentPlayer() == "white" ? "black" : "white";
        List<SkillBase> _skillList = GetSkillList("출산,아테나의 방패,에로스의 사랑,길동무", player);
        for (int i = 0; i < _skillList.Count; i++)
        {
            _skillList[i].SetPosX(mp.GetPosX());
            _skillList[i].SetPosY(mp.GetPosY());
            _skillList[i].StandardSkill();
        }
        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
    }

    // Function spawning skill prefab
    public SkillBase SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        SkillBase sb = CheckSkill(card).GetComponent<SkillBase>();
        if (sb == null) return null;
        AddSkillList(sb);
        sb.SetPalyer(GameManager.Inst.GetCurrentPlayer());
        sb.SetSelectPiece(chessPiece);
        chessPiece.AddChosenSkill(sb);
        selectingSkill = sb;
        sb.UsingSkill();

        return sb;
    }


    private GameObject CheckSkill(Card card)
    {
        GameObject obj = null;
        obj = Instantiate(skillPrefab, transform);
        obj.name = card.carditem.name;
        obj.transform.SetParent(null);
        switch (card.carditem.name)
        {
            case "천벌":
                obj.AddComponent<HeavenlyPunishment>();
                break;
            //case "에로스의 사랑":
            //    LoveOfEros(chessPiece);
            //    break;
            case "수면":
                obj.AddComponent<Sleep>();
                break;
            //case "음악":
            //    Music(chessPiece);
            //    break;
            //case "돌진":
            //    Rush(chessPiece);
            //    break;
            case "여행자":
                obj.AddComponent<Traveler>();
                break;
            //case "길동무":
            //    StreetFriend(chessPiece);
            //    break;
            //case "바카스":
            //    Bacchrs();
            //    break;
            //case "시간왜곡":
            //    TimeWarp();
            //    break;
            //case "제물":
            //    Offering(chessPiece);
            //    break;
            //case "정의구현":
            //    Justice();
            //    break;
            //case "출산":
            //    GiveBirth(chessPiece);
            //    break;
            case "아테나의 방패":
                obj.AddComponent<ShieldOfAthena>();
                break;
            //case "달빛":
            //    MoonLight(chessPiece);
            //    break;
            case "파도":
                obj.AddComponent<Wave>();
                break;
            //case "서풍":
            //    WestWind(chessPiece);
            //    break;
            case "수중감옥":
                obj.AddComponent<OceanJail>();
                break;
            //case "질서":
            //    Order(chessPiece);
            //    break;
            case "죽음의 땅":
                obj.AddComponent<GroundOfDeath>();
                break;
            case "전쟁광":
                obj.AddComponent<WarBuff>();
                break;
        }
        return obj;
    }

    #endregion

}
