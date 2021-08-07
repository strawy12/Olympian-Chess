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
    public bool CheckSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPlayer() == player)
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
    public SkillBase GetSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPlayer() == player)
                return skillList[i];
        }
        return null;
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
        isUsingCard = false;
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
            //case "���׳��� ����":
            //    AthenaShield(chessPiece);
            //    break;
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
                //case "���ﱤ":
                //    WarBuff(chessPiece);
                //    break;
        }
        return obj;
    }

    #endregion

}
