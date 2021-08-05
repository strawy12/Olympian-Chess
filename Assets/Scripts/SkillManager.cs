using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Inst { get; private set; }
    Card card;
    [SerializeField] private GameObject skillPrefab;

    private bool isUsingCard = false;

    public List<Chessman> dontClickPiece = new List<Chessman>();
    [SerializeField]
    private List<Skill> skillList = new List<Skill>();

    public int turnTime { get; private set; } = 0;
    void Awake() => Inst = this;

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
    public void SetTurnTime()
    {

        turnTime++;
        CheckSkillTime();
    }
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

    public void CheckSkillCancel()
    {
        Skill sk;
        if (CardManager.Inst.CheckCard("���ν��� ���", true))
        {
            sk = GetSkillList("���ν��� ���", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            Destroy(sk.gameObject);
            RemoveSkillList(sk);
        }

        if (CardManager.Inst.CheckCard("����", true))
        {
            sk = GetSkillList("����", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            Destroy(sk.gameObject);
            RemoveSkillList(sk);
        }
    }

    public bool UsingCard()
    {
        return isUsingCard;
    }
    public void SetIsUsingCard(bool isUsingCard)
    {
        this.isUsingCard = isUsingCard;
    }

    public void SetDontClickPiece(Chessman cp)
    {
        dontClickPiece.Add(cp);
    }
    public void RemoveDontClickPiece(Chessman cp)
    {
        dontClickPiece.Remove(cp);
    }
    public bool CheckDontClickPiece(Chessman cp)
    {
        for (int i = 0; i < dontClickPiece.Count; i++)
        {
            if (dontClickPiece[i] == cp)
                return true;
        }
        return false;
    }
    public void SetSkillList(Skill sk)
    {
        skillList.Add(sk);
    }
    public void DeleteSkillList(Skill sk)
    {
        skillList.Remove(sk);
    }
    public void RemoveSkillList(Skill sk)
    {
        skillList.Remove(sk);
    }
    public Skill SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        Skill sk = Instantiate(skillPrefab, transform).GetComponent<Skill>();
        sk.transform.SetParent(null);
        SetSkillList(sk);
        sk.SetPalyer(GameManager.Inst.GetCurrentPlayer());

        sk.UseSkill(card, chessPiece);

        return sk;
    }
    public bool CheckSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
                return true;
        }
        return false;
    }
    public Skill GetSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
                return skillList[i];
        }
        return null;
    }
    public bool CheckTurnTime(int turn)
    {
        if (turnTime > turn)
            return true;
        else
            return false;
    }
}
