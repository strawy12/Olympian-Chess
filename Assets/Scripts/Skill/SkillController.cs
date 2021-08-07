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
            case "천벌":
                gameObject.AddComponent<HeavenlyPunishment>();
                skillBase = GetComponent<HeavenlyPunishment>();
                break;
            //case "에로스의 사랑":
            //    LoveOfEros(chessPiece);
            //    break;
            //case "수면":
            //    Sleep(chessPiece);
            //    break;
            //case "음악":
            //    Music(chessPiece);
            //    break;
            //case "돌진":
            //    Rush(chessPiece);
            //    break;
            //case "여행자":
            //    Traveler(chessPiece);
            //    break;
            //case "길동무":
            //    StreetFriend(chessPiece);
            //    break;
            //case "바카스":
            //    Bacchrs();
            //    break;
            //case "시간왜곡":
            //    TimeWarp();
            //    break;
            //case "제물":
            //    Offering(chessPiece);
            //    break;
            //case "정의구현":
            //    Justice();
            //    break;
            //case "출산":
            //    GiveBirth(chessPiece);
            //    break;
            //case "아테나의 방패":
            //    AthenaShield(chessPiece);
            //    break;
            //case "달빛":
            //    MoonLight(chessPiece);
            //    break;
            //case "파도":
            //    Wave(chessPiece);
            //    break;
            //case "서풍":
            //    WestWind(chessPiece);
            //    break;
            //case "수중감옥":
            //    OceanJail(chessPiece);
            //    break;
            //case "질서":
            //    Order(chessPiece);
            //    break;
            //case "죽음의 땅":
            //    GroundOfDeath(chessPiece);
            //    break;
            //case "전쟁광":
            //    WarBuff(chessPiece);
            //    break;
        }
    }
}
