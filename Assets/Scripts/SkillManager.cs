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
        List<SkillBase> _skillList = cp.GetSkillList("���,���׳��� ����,���ν��� ���,�浿��");
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
        List<SkillBase> _skillList = cp.GetSkillList("����,��ī��");
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
            case "õ��":
                obj.AddComponent<HeavenlyPunishment>();
                break;
            //case "���ν��� ���":
            //    LoveOfEros(chessPiece);
            //    break;
            //case "����":
            //    Sleep(chessPiece);
            //    break;
            case "����":
                obj.AddComponent<Music>();
                break;
            case "����":
                obj.AddComponent<Rush>();
                break;
            case "������":
                obj.AddComponent<Traveler>();
                break;
            case "�浿��":
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
            //case "���׳��� ����":
            //    AthenaShield(chessPiece);
            //    break;
            case "�޺�":
                obj.AddComponent<MoonLight>();
                break;
                //case "�ĵ�":
                //    Wave(chessPiece);
                //    break;
                //case "��ǳ":
                //    WestWind(chessPiece);
                //    break;
                //case "���߰���":
                //    OceanJail(chessPiece);
                //    break;
                //case "����":
                //    Order(chessPiece);
                //    break;
                //case "������ ��":
                //    GroundOfDeath(chessPiece);
                //    break;
                //case "���ﱤ":
                //    WarBuff(chessPiece);
                //    break;
        }
        return obj;
    }

    #endregion

}
