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
        List<SkillBase> _skillList = GetSkillList("���,���׳��� ����,���ν��� ���,�浿��", player);
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
            case "õ��":
                obj.AddComponent<HeavenlyPunishment>();
                break;
            //case "���ν��� ���":
            //    LoveOfEros(chessPiece);
            //    break;
            case "����":
                obj.AddComponent<Sleep>();
                break;
            //case "����":
            //    Music(chessPiece);
            //    break;
            //case "����":
            //    Rush(chessPiece);
            //    break;
            case "������":
                obj.AddComponent<Traveler>();
                break;
            //case "�浿��":
            //    StreetFriend(chessPiece);
            //    break;
            //case "��ī��":
            //    Bacchrs();
            //    break;
            //case "�ð��ְ�":
            //    TimeWarp();
            //    break;
            //case "����":
            //    Offering(chessPiece);
            //    break;
            //case "���Ǳ���":
            //    Justice();
            //    break;
            //case "���":
            //    GiveBirth(chessPiece);
            //    break;
            case "���׳��� ����":
                obj.AddComponent<ShieldOfAthena>();
                break;
            //case "�޺�":
            //    MoonLight(chessPiece);
            //    break;
            case "�ĵ�":
                obj.AddComponent<Wave>();
                break;
            //case "��ǳ":
            //    WestWind(chessPiece);
            //    break;
            case "���߰���":
                obj.AddComponent<OceanJail>();
                break;
            //case "����":
            //    Order(chessPiece);
            //    break;
            case "������ ��":
                obj.AddComponent<GroundOfDeath>();
                break;
            case "���ﱤ":
                obj.AddComponent<WarBuff>();
                break;
        }
        return obj;
    }

    #endregion

}
