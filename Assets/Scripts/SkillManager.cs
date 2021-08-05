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

        if (CheckSkillList("�浿��", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("�浿��", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPiece() == null)
            {
                sk.ReloadStreetFriend();
            }
        }

        if (CheckSkillList("��ī��", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("��ī��", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
            {
                sk.ReloadBacchrs();
            }
        }

        if (CheckSkillList("���Ǳ���", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("���Ǳ���", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
            {
                sk.ReLoadJustice();
            }
        }

        if (CheckSkillList("���׳��� ����", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("���׳��� ����", GetCurrentPlayer(true));
            if (sk == null) return;
            sk.CheckAS();
        }

        if (CheckSkillList("�޺�", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("�޺�", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
                sk.ResetML();
            else
                sk.CheckML();
        }

        if (CheckSkillList("�޺�", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("�޺�", GetCurrentPlayer(false));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
                sk.ResetML();
            else
                sk.CheckML();
        }
        if (CheckSkillList("����", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("����", GetCurrentPlayer(false));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.CheckParticle();
        }
        if (CheckSkillList("��ǳ", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("��ǳ", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadWWChessPiece();
        }
        if (CheckSkillList("���߰���", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("���߰���", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadOJChessPiece();
        }
        if (CheckSkillList("����", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("����", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadODChessPiece();
        }
        if (CheckSkillList("������ ��", GetCurrentPlayer(true)) && CheckSkillList("������ ��", GetCurrentPlayer(false)))
        {
            sk1 = GetSkillList("������ ��", GetCurrentPlayer(true));
            sk2 = GetSkillList("������ ��", GetCurrentPlayer(false));
            if (sk1 == null || sk2 == null) return;
            if (sk1.isBreak || sk2.isBreak) return;
            sk1.StartGOD_SkillEffect();
            sk2.StartGOD_SkillEffect();
        }

        if (CheckSkillList("������ ��", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("������ ��", GetCurrentPlayer(true));

            if (sk.isBreak) return;
            sk.StartGOD_SkillEffect();
        }

        if (CheckSkillList("������ ��", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("������ ��", GetCurrentPlayer(false));

            if (sk.isBreak) return;
            sk.StartGOD_SkillEffect();
        }
    }

    // Function to stop using the canceled cards (Eros love, sleep)
    public void CheckSkillCancel()
    {
        Skill sk;

        if (CardManager.Inst.CheckCard("���ν��� ���"))
        {
            sk = GetSkillList("���ν��� ���", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            // ���ν��� ����� ������� ���� ���°� �ƴϰ�
            // ��ų ����� ���� ������ ü���ǽ��� ���ٸ� ��ų ����
            Destroy(sk.gameObject);
            RemoveSkillList(sk);
        }

        if (CardManager.Inst.CheckCard("����"))
        {
            sk = GetSkillList("����", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            // ������ ������� ���� ���°� �ƴϰ�
            // ��ų ����� ���� ������ ü���ǽ��� ���ٸ� ��ų ����
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
