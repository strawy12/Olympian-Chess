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
    public bool CheckSkillList(string skill, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].name == skill && skillList[i].GetPlayer() == player)
                return true;
        }
        return false;
    }


    #endregion

    #region Script Access 
    public void AttackUsingSkill(MovePlate mp)
    {
        Chessman cp = GameManager.Inst.GetPosition(mp.GetPosX(), mp.GetPosY());
        List<SkillBase> _skillList = cp.GetSkillList("출산,아테나의 방패,에로스의 사랑,길동무");
        Debug.Log(_skillList.Count);
        for (int i = 0; i < _skillList.Count; i++)
        {
            Debug.Log(_skillList[i].name);
            _skillList[i].SetPosX(mp.Getreference().GetXBoard());
            _skillList[i].SetPosY(mp.Getreference().GetYBoard());
            _skillList[i].SetMovePlate(mp);
            _skillList[i].StandardSkill();
        }
    }


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
    public SkillBase GetSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPlayer() == player)
                return skillList[i];
        }
        return null;
    }

    public bool MoveControl(Chessman cp)
    {
        List<SkillBase> _skillList = cp.GetSkillList("질서,바카스");
        int i = 0;
           
        for (i = 0; i < _skillList.Count; i++)
        {
            if(GameManager.Inst.isBacchrs)
            {
                skillList[i].SetSelectPiece(cp);
            }
            _skillList[i].StandardSkill();
        }
        if(i != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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

    public void SkillListStandard(Chessman cm)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].selectPiece == cm)
            {
                skillList[i].StandardSkill();
            }
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
    }

    // Function spawning skill prefab
    public SkillBase SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        SkillBase sb = CheckSkill(card).GetComponent<SkillBase>();
        if (sb == null) return null;
        AddSkillList(sb);
        sb.SetPalyer(GameManager.Inst.GetCurrentPlayer());
        sb.SetSelectPiece(chessPiece);
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
            //case "수면":
            //    Sleep(chessPiece);
            //    break;
            case "음악":
                obj.AddComponent<Music>();
                break;
            case "돌진":
                obj.AddComponent<Rush>();
                break;
            case "여행자":
                obj.AddComponent<Traveler>();
                break;
            case "길동무":
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
            //case "아테나의 방패":
            //    AthenaShield(chessPiece);
            //    break;
            case "달빛":
                obj.AddComponent<MoonLight>();
                break;
                //case "파도":
                //    Wave(chessPiece);
                //    break;
                //case "서풍":
                //    WestWind(chessPiece);
                //    break;
                //case "수중감옥":
                //    OceanJail(chessPiece);
                //    break;
                //case "질서":
                //    Order(chessPiece);
                //    break;
                //case "죽음의 땅":
                //    GroundOfDeath(chessPiece);
                //    break;
                //case "전쟁광":
                //    WarBuff(chessPiece);
                //    break;
        }
        return obj;
    }

    #endregion

}
