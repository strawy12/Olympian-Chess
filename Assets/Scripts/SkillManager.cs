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
    Card card;
    public int turnTime { get; private set; } = 0;

    #endregion

    #region System Check
    //Function checking every skill's times
    // Function to stop using the canceled cards (Eros love, sleep)
    public void CheckSkillCancel()
    {
        SkillController sk;

            sk = GetSkillList("에로스의 사랑", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            // 에로스의 사랑을 사용하지 않은 상태가 아니고
            // 스킬 사용을 위해 선택한 체스피스가 없다면 스킬 제거
            Destroy(sk.gameObject);
            RemoveSkillList(sk);
    }

    // Function checking skill's turn time
    // if the returned value is true, the skill turn ends
    public bool CheckTurnTime(int turn)
    {
        if (turnTime > turn)
            return true;
        else
            return false;
    }

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
    //Function returning the color(black or white) of the player who is current turn.
    public string GetCurrentPlayer(bool reverse)
    {
        if (!reverse)
        {
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                return "black";
            }
            else if (GameManager.Inst.GetCurrentPlayer() == "black")
            {
                return "white";
            }
        }
        else
        {
            return GameManager.Inst.GetCurrentPlayer();
        }
        return GameManager.Inst.GetCurrentPlayer();

    }

    // Function returning isUsingCard value
    public bool UsingCard()
    {
        return isUsingCard;
    }

    // Function setting isUsingCard value
    public void SetIsUsingCard(bool isUsingCard)
    {
        this.isUsingCard = isUsingCard;
    }

    // Function returning skill if there is a skill from skillList that is the same as name
    public SkillController GetSkillList(string name, string player)
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

    //A function increasing the turntime and checking the skill time
    public void SetTurnTime()
    {
        turnTime++;
        //CheckSkillTime();
    }

    // Function adding sk to skillList

    public void SetSkillList(SkillController sk)
    {
        skillList.Add(sk);
    }

    // Function removing sk to skillList
    public void DeleteSkillList(SkillController sk)
    {
        skillList.Remove(sk);
    }

    // to be removed later**********************************
    public void RemoveSkillList(SkillController sk)
    {
        skillList.Remove(sk);
    }

    // Function adding the cp to dontClickPiece list
    public void SetDontClickPiece(Chessman cp)
    {
        dontClickPiece.Add(cp);
    }

    // Function removing cp from dontClickPiece list
    public void RemoveDontClickPiece(Chessman cp)
    {
        dontClickPiece.Remove(cp);
    }

    // Function spawning skill prefab
    public GameObject SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        string str = card.carditem.className;
        Type T = Type.GetType(str);
        GameObject obj = Instantiate(skillPrefab);
        obj.AddComponent(T);
        obj.transform.SetParent(null);
        obj.name = card.carditem.name;

        //SetSkillList(sk);
        //sk.SetPalyer(GameManager.Inst.GetCurrentPlayer());

        //sk.UseSkill(card, chessPiece);

        return obj;
    }
    #endregion

}
