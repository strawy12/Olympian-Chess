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
    [SerializeField] private List<Skill> skillList = new List<Skill>();
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
    private void CheckSkillTime()
    {
        Skill sk;
        Skill sk1;
        Skill sk2;

        if (CheckSkillList("길동무", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("길동무", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPiece() == null)
            {
                sk.ReloadStreetFriend();
            }
        }

        if (CheckSkillList("바카스", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("바카스", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
            {
                sk.ReloadBacchrs();
            }
        }

        if (CheckSkillList("정의구현", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("정의구현", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
            {
                sk.ReLoadJustice();
            }
        }

        if (CheckSkillList("아테나의 방패", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("아테나의 방패", GetCurrentPlayer(true));
            if (sk == null) return;
            sk.CheckAS();
        }

        if (CheckSkillList("달빛", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("달빛", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
                sk.ResetML();
            else
                sk.CheckML();
        }

        if (CheckSkillList("달빛", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("달빛", GetCurrentPlayer(false));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
                sk.ResetML();
            else
                sk.CheckML();
        }
        if (CheckSkillList("수면", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("수면", GetCurrentPlayer(false));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.CheckParticle();
        }
        if (CheckSkillList("서풍", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("서풍", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadWWChessPiece();
        }
        if (CheckSkillList("수중감옥", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("수중감옥", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadOJChessPiece();
        }
        if (CheckSkillList("질서", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("질서", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadODChessPiece();
        }
        if (CheckSkillList("죽음의 땅", GetCurrentPlayer(true)) && CheckSkillList("죽음의 땅", GetCurrentPlayer(false)))
        {
            sk1 = GetSkillList("죽음의 땅", GetCurrentPlayer(true));
            sk2 = GetSkillList("죽음의 땅", GetCurrentPlayer(false));
            if (sk1 == null || sk2 == null) return;
            if (sk1.isBreak || sk2.isBreak) return;
            sk1.StartGOD_SkillEffect();
            sk2.StartGOD_SkillEffect();
        }

        if (CheckSkillList("죽음의 땅", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("죽음의 땅", GetCurrentPlayer(true));

            if (sk.isBreak) return;
            sk.StartGOD_SkillEffect();
        }

        if (CheckSkillList("죽음의 땅", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("죽음의 땅", GetCurrentPlayer(false));

            if (sk.isBreak) return;
            sk.StartGOD_SkillEffect();
        }
    }

    // Function to stop using the canceled cards (Eros love, sleep)
    public void CheckSkillCancel()
    {
        Skill sk;

        if (CardManager.Inst.CheckCard("에로스의 사랑"))
        {
            sk = GetSkillList("에로스의 사랑", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            // 에로스의 사랑을 사용하지 않은 상태가 아니고
            // 스킬 사용을 위해 선택한 체스피스가 없다면 스킬 제거
            Destroy(sk.gameObject);
            RemoveSkillList(sk);
        }

        if (CardManager.Inst.CheckCard("수면"))
        {
            sk = GetSkillList("수면", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            // 수면을 사용하지 않은 상태가 아니고
            // 스킬 사용을 위해 선택한 체스피스가 없다면 스킬 제거
            Destroy(sk.gameObject);
            RemoveSkillList(sk);
        }
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
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
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
    public Skill GetSkillList(string name, string player)
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

    //A function increasing the turntime and checking the skill time
    public void SetTurnTime()
    {
        turnTime++;
        CheckSkillTime();
    }

    // Function adding sk to skillList

    public void SetSkillList(Skill sk)
    {
        skillList.Add(sk);
    }

    // Function removing sk to skillList
    public void DeleteSkillList(Skill sk)
    {
        skillList.Remove(sk);
    }

    // to be removed later**********************************
    public void RemoveSkillList(Skill sk)
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
    public Skill SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        Skill sk = Instantiate(skillPrefab, transform).GetComponent<Skill>();
        sk.transform.SetParent(null);
        SetSkillList(sk);
        sk.SetPalyer(GameManager.Inst.GetCurrentPlayer());

        sk.UseSkill(card, chessPiece);

        return sk;
    }
    #endregion

}
