using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //SkillManager 싱글톤
    public static SkillManager Inst { get; private set; }
    Card card;
    [SerializeField] private GameObject skillPrefab;

    private bool isUsingCard = false;

    //클릭 허용하지 않는 체스피스 리스트
    public List<Chessman> dontClickPiece = new List<Chessman>();
    //현재 사용 중인 스킬 리스트
    [SerializeField]
    private List<Skill> skillList = new List<Skill>();

    public int turnTime { get; private set; } = 0;
    void Awake() => Inst = this;

    //현재 턴이 있는 플레이어의 색(black or white)을 반환하는 함수
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
    //턴이 끝날 때 턴타임을 1씩 증가시키고 스킬 지속 시간을 확인하는 함수를 호출하는 함수
    public void SetTurnTime()
    {
        turnTime++;
        CheckSkillTime();
    }
    //스킬 지속 시간을 확인하는 함수
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
    // 사용하다 취소한 카드들(에로스의 사랑, 수면)의 사용을 중지하는 함수
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

    // isUsingCard의 bool값을 반환하는 함수
    public bool UsingCard()
    {
        return isUsingCard;
    }
    // isUsingCard의 bool값을 설정하는 함수
    public void SetIsUsingCard(bool isUsingCard)
    {
        this.isUsingCard = isUsingCard;
    }
    // dontClickPiece 리스트에 인자값(매개변수) 체스피스를 추가하는 함수
    public void SetDontClickPiece(Chessman cp)
    {
        dontClickPiece.Add(cp);
    }
    // dontClickPiece 리스트에서 인자값(매개변수) 체스피스를 제거하는 함수
    public void RemoveDontClickPiece(Chessman cp)
    {
        dontClickPiece.Remove(cp);
    }
    // 인자값(매개변수) 체스피스가 dontClickPiece 리스트에 있는지의 여부를 판별하는 함수
    // 있다면 true
    public bool CheckDontClickPiece(Chessman cp)
    {
        for (int i = 0; i < dontClickPiece.Count; i++)
        {
            if (dontClickPiece[i] == cp)
                return true;
        }
        return false;
    }
    // 인자 값 스킬을 현재 사용중인 스킬 리스트에 추가하는 함수
    public void SetSkillList(Skill sk)
    {
        skillList.Add(sk);
    }
    // 인자 값 스킬을 현재 사용중인 스킬 리스트에서 제거하는 함수
    public void DeleteSkillList(Skill sk)
    {
        skillList.Remove(sk);
    }
    // 나중에 제거할 함수
    public void RemoveSkillList(Skill sk)
    {
        skillList.Remove(sk);
    }
    // 스킬 프리팹을 생성하는 함수
    public Skill SpawnSkillPrefab(Card card, Chessman chessPiece)
    {
        Skill sk = Instantiate(skillPrefab, transform).GetComponent<Skill>();
        sk.transform.SetParent(null);
        SetSkillList(sk);
        sk.SetPalyer(GameManager.Inst.GetCurrentPlayer());

        sk.UseSkill(card, chessPiece);

        return sk;
    }
    // 인자값에 해당하는 스킬이 있는지 확인하는 함수
    // (현재 사용 중인 스킬 리스트에 name 있고 player가 그 스킬을 사용했던 플레이어이면 true)
    public bool CheckSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
                return true;
        }
        return false;
    }
    // 인자값에 해당하는 스킬을 반환하는 함수
    // (현재 사용 중인 스킬 리스트에 name 있고 player가 그 스킬을 사용했던 플레이어이면 스킬 반환)
    public Skill GetSkillList(string name, string player)
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            if (skillList[i].gameObject.name == name && skillList[i].GetPalyer() == player)
                return skillList[i];
        }
        return null;
    }
    // 스킬 턴 타임을 체크하는 함수
    // 반환 값이 true이면 스킬 턴이 끝남
    public bool CheckTurnTime(int turn)
    {
        if (turnTime > turn)
            return true;
        else
            return false;
    }
}
