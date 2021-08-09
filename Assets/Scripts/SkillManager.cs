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
    public List<SkillBase> GetSkillList(string name)
    {
        List<SkillBase> _skillList = new List<SkillBase>();

        string[] names = name.Split(',');
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name)
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

    public void CheckSkillCancel(string name)
    {
        if (CardManager.Inst.GetSelectCard() == null) return;
        string[] names = name.Split(',');

        for (int i = 0; i < names.Length; i++)
        {
            if (CardManager.Inst.GetSelectCard().name == names[i])
            {
                SkillBase sb = GetSkillList(names[i])[0];
                skillList.Remove(sb);
                Destroy(sb.gameObject);

            }
        }
    }

    public bool CheckReturnMovePlate(int x, int y, string name)
    {
        List<SkillBase> skillList = GetSkillList(name);

        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i] != null)
            {
                if (skillList[i].GetPosX() == x && skillList[i].GetPosY() == y)
                {
                    return true;
                }   

            }
        }
        return false;
    }

    public bool MoveControl(Chessman cp)
    {
        List<SkillBase> _skillList = cp.GetSkillList("����,��ī��");
        int i = 0;
           
        for (i = 0; i < _skillList.Count; i++)
        {
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
            _skillList[i].StandardSkill();
        }
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

            case "���ν��� ���":
                obj.AddComponent<LoveOfEros>();
                break;

            case "����":
                obj.AddComponent<Sleep>();
                break;

            case "���׳��� ����":
                obj.AddComponent<ShieldOfAthena>();
                break;

            case "�ĵ�":
                obj.AddComponent<Wave>();
                break;

            case "��ǳ":
                obj.AddComponent<WestWind>();
                break;

            case "���߰���":
                obj.AddComponent<OceanJail>();
                break;

            case "����":
                obj.AddComponent<Law>();
                break;

            case "������ ��":
                obj.AddComponent<GroundOfDeath>();
                break;

            case "���ﱤ":
                obj.AddComponent<WarBuff>();
                break;

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

            case "�޺�":
                obj.AddComponent<MoonLight>();
                break;
        }
        return obj;
    }

    #endregion

}
