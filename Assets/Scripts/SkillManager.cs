using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //SkillManager �̱���
    public static SkillManager Inst { get; private set; }
    Card card;
    [SerializeField] private GameObject skillPrefab;

    private bool isUsingCard = false;

    //Ŭ�� ������� �ʴ� ü���ǽ� ����Ʈ
    public List<Chessman> dontClickPiece = new List<Chessman>();
    //���� ��� ���� ��ų ����Ʈ
    [SerializeField]
    private List<Skill> skillList = new List<Skill>();

    public int turnTime { get; private set; } = 0;
    void Awake() => Inst = this;

    //���� ���� �ִ� �÷��̾��� ��(black or white)�� ��ȯ�ϴ� �Լ�
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
    //���� ���� �� ��Ÿ���� 1�� ������Ű�� ��ų ���� �ð��� Ȯ���ϴ� �Լ��� ȣ���ϴ� �Լ�
    public void SetTurnTime()
    {
        turnTime++;
        CheckSkillTime();
    }
    //��ų ���� �ð��� Ȯ���ϴ� �Լ�
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
    // ����ϴ� ����� ī���(���ν��� ���, ����)�� ����� �����ϴ� �Լ�
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

    // isUsingCard�� bool���� ��ȯ�ϴ� �Լ�
    public bool UsingCard()
    {
        return isUsingCard;
    }
    // isUsingCard�� bool���� �����ϴ� �Լ�
    public void SetIsUsingCard(bool isUsingCard)
    {
        this.isUsingCard = isUsingCard;
    }
    // dontClickPiece ����Ʈ�� ���ڰ�(�Ű�����) ü���ǽ��� �߰��ϴ� �Լ�
    public void SetDontClickPiece(Chessman cp)
    {
        dontClickPiece.Add(cp);
    }
    // dontClickPiece ����Ʈ���� ���ڰ�(�Ű�����) ü���ǽ��� �����ϴ� �Լ�
    public void RemoveDontClickPiece(Chessman cp)
    {
        dontClickPiece.Remove(cp);
    }
    // ���ڰ�(�Ű�����) ü���ǽ��� dontClickPiece ����Ʈ�� �ִ����� ���θ� �Ǻ��ϴ� �Լ�
    // �ִٸ� true
    public bool CheckDontClickPiece(Chessman cp)
    {
        for (int i = 0; i < dontClickPiece.Count; i++)
        {
            if (dontClickPiece[i] == cp)
                return true;
        }
        return false;
    }
    // ���� �� ��ų�� ���� ������� ��ų ����Ʈ�� �߰��ϴ� �Լ�
    public void SetSkillList(Skill sk)
    {
        skillList.Add(sk);
    }
    // ���� �� ��ų�� ���� ������� ��ų ����Ʈ���� �����ϴ� �Լ�
    public void DeleteSkillList(Skill sk)
    {
        skillList.Remove(sk);
    }
    // ���߿� ������ �Լ�
    public void RemoveSkillList(Skill sk)
    {
        skillList.Remove(sk);
    }
    // ��ų �������� �����ϴ� �Լ�
    public Skill SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        Skill sk = Instantiate(skillPrefab, transform).GetComponent<Skill>();
        sk.transform.SetParent(null);
        SetSkillList(sk);
        sk.SetPalyer(GameManager.Inst.GetCurrentPlayer());

        sk.UseSkill(card, chessPiece);

        return sk;
    }
    // ���ڰ��� �ش��ϴ� ��ų�� �ִ��� Ȯ���ϴ� �Լ�
    // (���� ��� ���� ��ų ����Ʈ�� name �ְ� player�� �� ��ų�� ����ߴ� �÷��̾��̸� true)
    public bool CheckSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
                return true;
        }
        return false;
    }
    // ���ڰ��� �ش��ϴ� ��ų�� ��ȯ�ϴ� �Լ�
    // (���� ��� ���� ��ų ����Ʈ�� name �ְ� player�� �� ��ų�� ����ߴ� �÷��̾��̸� ��ų ��ȯ)
    public Skill GetSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
                return skillList[i];
        }
        return null;
    }
    // ��ų �� Ÿ���� üũ�ϴ� �Լ�
    // ��ȯ ���� true�̸� ��ų ���� ����
    public bool CheckTurnTime(int turn)
    {
        if (turnTime > turn)
            return true;
        else
            return false;
    }
}
