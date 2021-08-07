using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    string player;
    public SkillBase skillBase { get; private set; }

    public void SettingSkill(Chessman chessPiece)
    {
        CheckSkill();
        if (skillBase == null) return;
        skillBase.SetSelectPiece(chessPiece);
        skillBase.UsingSkill();
    }

    public string GetPalyer()
    {
        return player;
    }

    public void SetPalyer(string player)
    {
        this.player = player;
    }

    private void CheckSkill()
    {
        switch (gameObject.name)
        { 
            case "õ��":
                gameObject.AddComponent<HeavenlyPunishment>();
                skillBase = GetComponent<HeavenlyPunishment>();
                break;
            //case "���ν��� ���":
            //    LoveOfEros(chessPiece);
            //    break;
            //case "����":
            //    Sleep(chessPiece);
            //    break;
            //case "����":
            //    Music(chessPiece);
            //    break;
            //case "����":
            //    Rush(chessPiece);
            //    break;
            //case "������":
            //    Traveler(chessPiece);
            //    break;
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
            //case "�ĵ�":
            //    Wave(chessPiece);
            //    break;
            //case "��ǳ":
            //    WestWind(chessPiece);
            //    break;
            //case "���߰���":
            //    OceanJail(chessPiece);
            //    break;
            //case "����":
            //    Order(chessPiece);
            //    break;
            //case "������ ��":
            //    GroundOfDeath(chessPiece);
            //    break;
            //case "���ﱤ":
            //    WarBuff(chessPiece);
            //    break;
        }
    }
}
