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
        if (CheckSkillList("±æµ¿¹«", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("±æµ¿¹«", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPiece() == null)
            {
                sk.ReloadStreetFriend();
            }
        }

        if (CheckSkillList("¹ÙÄ«½º", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("¹ÙÄ«½º", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
            {
                sk.ReloadBacchrs();
            }
        }

        if (CheckSkillList("Á¤ÀÇ±¸Çö", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("Á¤ÀÇ±¸Çö", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
            {
                sk.ReLoadJustice();
            }
        }

        if (CheckSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("¾ÆÅ×³ªÀÇ ¹æÆÐ", GetCurrentPlayer(true));
            if (sk == null) return;
            sk.CheckAS();
        }

        if (CheckSkillList("´Þºû", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("´Þºû", GetCurrentPlayer(true));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
                sk.ResetML();
            else
                sk.CheckML();
        }

        if (CheckSkillList("´Þºû", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("´Þºû", GetCurrentPlayer(false));
            if (sk == null) return;
            if (CheckTurnTime(sk.turn))
                sk.ResetML();
            else
                sk.CheckML();
        }
        if (CheckSkillList("¼ö¸é", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("¼ö¸é", GetCurrentPlayer(false));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.CheckParticle();
        }
        if (CheckSkillList("¼­Ç³", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("¼­Ç³", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadWWChessPiece();
        }
        if (CheckSkillList("¼öÁß°¨¿Á", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("¼öÁß°¨¿Á", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadOJChessPiece();
        }
        if (CheckSkillList("Áú¼­", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("Áú¼­", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.isBreak) return;
            sk.ReLoadODChessPiece();
        }
        if (CheckSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(true)) && CheckSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(false)))
        {
            sk1 = GetSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(true));
            sk2 = GetSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(false));
            if (sk1 == null || sk2 == null) return;
            if (sk1.isBreak || sk2.isBreak) return;
            sk1.StartGOD_SkillEffect();
            sk2.StartGOD_SkillEffect();
        }

        if (CheckSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(true)))
        {
            sk = GetSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(true));

            if (sk.isBreak) return;
            sk.StartGOD_SkillEffect();
        }

        if (CheckSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(false)))
        {
            sk = GetSkillList("Á×À½ÀÇ ¶¥", GetCurrentPlayer(false));

            if (sk.isBreak) return;
            sk.StartGOD_SkillEffect();
        }
    }

    public void CheckSkillCancel()
    {
        Skill sk;
        if (CardManager.Inst.CheckCard("¿¡·Î½ºÀÇ »ç¶û", true))
        {
            sk = GetSkillList("¿¡·Î½ºÀÇ »ç¶û", GetCurrentPlayer(true));
            if (sk == null) return;
            if (sk.GetSelectPieceTo() != null) return;
            Destroy(sk.gameObject);
            RemoveSkillList(sk);
        }

        if (CardManager.Inst.CheckCard("¼ö¸é", true))
        {
            sk = GetSkillList("¼ö¸é", GetCurrentPlayer(true));
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
