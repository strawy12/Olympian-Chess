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
    [SerializeField] private List<SkillController> skillList = new List<SkillController>();
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
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
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
    public SkillController GetSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
                return skillList[i];
        }
        return null;
    }

    #endregion

    #region System

    // Function adding sk to skillList

    public void AddSkillList(SkillController sc)
    {
        skillList.Add(sc);
    }

    // to be removed later
    public void RemoveSkillList(SkillController sc)
    {
        skillList.Remove(sc);
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

    // Function spawning skill prefab
    public SkillController SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        SkillController sc = Instantiate(skillPrefab, transform).GetComponent<SkillController>();
        sc.transform.SetParent(null);
        AddSkillList(sc);
        sc.SetPalyer(GameManager.Inst.GetCurrentPlayer());
        sc.gameObject.name = card.carditem.name;

        sc.SettingSkill(chessPiece);

        return sc;
    }
    #endregion

}
