using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private Chessman selectPiece;
    private Chessman selectPieceTo;
    [SerializeField] new Animation animation;
    private bool isUsingCard = false;
    private bool isML_moved = false;
    private int posX;
    private int posY;
    private bool isAttack;
    public int turn { get; private set; } = 0;
    private string player = "white";
    public bool isBreak { get; private set; } = true;
    public int cnt { get; private set; } = 0;
    private GameObject god_mp;

    private bool CheckPlayer(string player)
    {
        if (GameManager.Inst.GetCurrentPlayer() == player)
            return true;
        else
            return false;
    }
    private bool CheckTurnTime(int turn)
    {
        return SkillManager.Inst.CheckTurnTime(turn);
    }
    private int GetTurnTime()
    {
        return SkillManager.Inst.turnTime;
    }

    public void SetIsUsingCard(bool isUsingCard)
    {
        this.isUsingCard = isUsingCard;
    }

    public void UseSkill(Card card, Chessman chessPiece)
    {
        if (card == null) return;
        gameObject.name = card.carditem.name;
        switch (card.carditem.name)
        {
            case "천벌":
                HeavenlyPunishment(chessPiece);
                break;
            case "에로스의 사랑":
                LoveOfEros(chessPiece);
                break;
            case "수면":
                Sleep(chessPiece);
                break;
            case "음악":
                Music(chessPiece);
                break;
            case "돌진":
                Rush(chessPiece);
                break;
            case "여행자":
                Traveler(chessPiece);
                break;
            case "길동무":
                StreetFriend(chessPiece);
                break;
            case "바카스":
                Bacchrs();
                break;
            case "시간왜곡":
                TimeWarp();
                break;
            case "제물":
                Offering(chessPiece);
                break;
            case "정의구현":
                Justice();
                break;
            case "출산":
                GiveBirth(chessPiece);
                break;
            case "아테나의 방패":
                AthenaShield(chessPiece);
                break;
            case "달빛":
                MoonLight(chessPiece);
                break;
            case "파도":
                Wave(chessPiece);
                break;
            case "서풍":
                WestWind(chessPiece);
                break;
            case "수중감옥":
                OceanJail(chessPiece);
                break;
            case "질서":
                Order(chessPiece);
                break;
            case "죽음의 땅":
                GroundOfDeath(chessPiece);
                break;
            case "전쟁광":
                WarBuff(chessPiece);
                break;
        }
    }

    private void LoveOfEros(Chessman chessPiece)
    {
        selectPiece = chessPiece;
        selectPiece.SetMySkill(true);
        selectPiece.SetMovePlateColor(new Color32(29, 219, 22, 255));
        selectPiece.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard());
        selectPiece.SetMovePlateColor(new Color32(255, 255, 36, 255));
        selectPiece.SetMySkill(false);
        GameManager.Inst.AllMovePlateSpawn(selectPiece, true);
        isUsingCard = true;
        SkillManager.Inst.SetIsUsingCard(true);
    }
    private void HeavenlyPunishment(Chessman chessPiece)
    {
        if (chessPiece.name == "black_king" || chessPiece.name == "white_king")
        {

            CardManager.Inst.SetisBreak(true);
            return;
        }

        if (chessPiece.name == "black_queen" || chessPiece.name == "white_queen")
        {
            if (CheckPlayer("white"))
                isBreak = GameManager.Inst.CheckArr(false, "black_rook");
            else
                isBreak = GameManager.Inst.CheckArr(true, "white_rook");
            if (isBreak)
            {

                CardManager.Inst.SetisBreak(true);
                return;
            }

        }
        selectPiece = chessPiece;
        StartCoroutine(HP_SkillEffect());
        CardManager.Inst.SetisBreak(false);
        isUsingCard = false;
        SkillManager.Inst.SetDontClickPiece(selectPiece);
    }
    private void Sleep(Chessman chessPiece)
    {
        selectPiece = chessPiece;
        chessPiece.SetMySkill(true);
        chessPiece.SetMovePlateColor(new Color32(95, 0, 255, 255));
        chessPiece.MovePlateSpawn(chessPiece.GetXBoard(), chessPiece.GetYBoard());
        chessPiece.SetMySkill(false);
        GameManager.Inst.AllMovePlateSpawn(chessPiece, false);
        chessPiece.SetMovePlateColor(new Color32(255, 255, 36, 255));
        SkillManager.Inst.SetIsUsingCard(true);
    }
    private void WestWind(Chessman chessPiece)
    {
        selectPiece = chessPiece;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(215, 199, 176, 144));
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();
        selectPiece.spriteRenderer.sortingOrder = -2;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
        GameManager.Inst.SetPositionEmpty(posX, posY);
        CardManager.Inst.SetisBreak(false);
        isUsingCard = false;
        turn = GetTurnTime() + 1;
        isBreak = false;

    }
    private void Rush(Chessman chessPiece)
    {
        isUsingCard = false;
        selectPiece = chessPiece;

        int posX_rush;
        int posY_rush;

        posX_rush = selectPiece.GetXBoard();
        posY_rush = selectPiece.GetYBoard();

        if (selectPiece.player == "white")
        {
            if (GameManager.Inst.GetPosition(posX_rush, posY_rush + 1) == null)
            {
                GameManager.Inst.SetPositionEmpty(posX_rush, posY_rush);

                selectPiece.SetXBoard(posX_rush);
                selectPiece.SetYBoard(posY_rush + 1);
                selectPiece.SetCoords();

                GameManager.Inst.SetPosition(selectPiece);
                CardManager.Inst.ChangeIsUse(true);
                CardManager.Inst.SetisBreak(false);

            }
            else
            {
                CardManager.Inst.SetisBreak(true);
            }
        }
        else
        {
            if (GameManager.Inst.GetPosition(posX_rush, posY_rush - 1) == null)
            {
                GameManager.Inst.SetPositionEmpty(posX_rush, posY_rush);

                selectPiece.SetXBoard(posX_rush);
                selectPiece.SetYBoard(posY_rush - 1);
                selectPiece.SetCoords();

                CardManager.Inst.ChangeIsUse(true);
                CardManager.Inst.SetisBreak(false);
                GameManager.Inst.SetPosition(selectPiece);
            }
            else
            {
                CardManager.Inst.SetisBreak(true);
            }

        }

        DeleteSkill();
    }
    private void Music(Chessman chessPiece)
    {
        if (chessPiece.name == "black_king" || chessPiece.name == "white_king")
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }

        selectPiece = chessPiece;
        StartCoroutine(MC_SkillEffect());
        CardManager.Inst.SetisBreak(false);
        isUsingCard = false;
        SkillManager.Inst.SetDontClickPiece(selectPiece);
    }
    private void OceanJail(Chessman chessPiece)
    {
        selectPiece = chessPiece;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 255, 144));
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();
        selectPiece.spriteRenderer.sortingOrder = -2;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
        GameManager.Inst.SetPositionEmpty(posX, posY);
        CardManager.Inst.SetisBreak(false);
        isUsingCard = false;
        turn = GetTurnTime() + 2;
        isBreak = false;

    }
    private void Order(Chessman chessPiece)
    {
        turn = GetTurnTime() + 2;
        isUsingCard = false;
        selectPiece = chessPiece;
        CardManager.Inst.SetisBreak(false);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 144));
        isBreak = false;
    }
    private void GroundOfDeath(Chessman chesspiece)
    {
        if (chesspiece.name == "white_pawn" || chesspiece.name == "black_pawn")
        {
            isUsingCard = false;

            posX = chesspiece.GetXBoard();
            posY = chesspiece.GetYBoard();
            god_mp = chesspiece.GOD_MovePlateSpawn(posX, posY);

            turn = GetTurnTime() + 1;
            isBreak = false;
        }
        else
            CardManager.Inst.SetisBreak(true);
    }
    public void checkSPChessPiece()
    {
        turn = GetTurnTime() + 1;
        if (!selectPiece.isMoved && !selectPieceTo.isMoved)
        {
            selectPiece.SleepParticle(true, selectPiece.player);
            selectPieceTo.SleepParticle(true, selectPiece.player);
            SkillManager.Inst.SetDontClickPiece(selectPiece);
            SkillManager.Inst.SetDontClickPiece(selectPieceTo);
            return;
        }
        if (selectPieceTo.isMoved && !selectPiece.isMoved)
        {
            selectPieceTo = null;
            selectPiece.SleepParticle(true, selectPiece.player);
            SkillManager.Inst.SetDontClickPiece(selectPiece);
            return;
        }

        if (selectPiece.isMoved && !selectPieceTo.isMoved)
        {
            selectPiece = null;
            selectPieceTo.SleepParticle(true, selectPieceTo.player);
            SkillManager.Inst.SetDontClickPiece(selectPieceTo);
            return;
        }
    }
    public void CheckParticle()
    {
        if (selectPiece != null && selectPieceTo != null && CheckTurnTime(turn))
        {
            selectPiece.SleepParticle(false, selectPiece.player);
            selectPieceTo.SleepParticle(false, selectPieceTo.player);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);
            selectPiece = null;
            selectPieceTo = null;

        }
        else if (selectPiece != null && CheckTurnTime(turn))
        {
            selectPiece.SleepParticle(false, selectPiece.player);
            SkillManager.Inst.RemoveDontClickPiece(selectPiece);
            selectPiece = null;

        }

        else if (selectPieceTo != null && CheckTurnTime(turn))
        {
            selectPieceTo.SleepParticle(false, selectPieceTo.player);
            SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);
            selectPieceTo = null;

        }
        else
        {
            return;
        }
        //SkillManager.Inst.RemoveSkillList(this);
        turn = 0;
        //Destroy(gameObject);
    }
    private void WarBuff(Chessman chessPiece)
    {
        isUsingCard = false;
        if (chessPiece.player != GameManager.Inst.GetCurrentPlayer())
        {
            selectPiece = chessPiece;
            if (selectPiece.isMoving)
            {
                TurnManager.Instance.SetIsActive(false);
            }
            return;
        }
        selectPiece = chessPiece;
        if (selectPiece.isMoving)
        {
            TurnManager.Instance.SetIsActive(false);
        }
    }
    public void ReLoadWWChessPiece()
    {
        if (!CheckTurnTime(turn)) return;
        selectPiece.spriteRenderer.sortingOrder = 0;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
        if (GameManager.Inst.GetPosition(posX, posY) != null && GameManager.Inst.GetPosition(posX, posY).player != selectPiece.player)
        {
            Destroy(GameManager.Inst.GetPosition(posX, posY).gameObject);
            GameManager.Inst.SetPositionEmpty(posX, posY);
        }
        GameManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        selectPiece = null;
        turn = 0;
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
    public void ReLoadOJChessPiece()
    {
        if (!CheckTurnTime(turn)) return;
        selectPiece.spriteRenderer.sortingOrder = 0;
        selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
        if (GameManager.Inst.GetPosition(posX, posY) != null)
        {
            Destroy(GameManager.Inst.GetPosition(posX, posY).gameObject);
            GameManager.Inst.SetPositionEmpty(posX, posY);
        }
        GameManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        selectPiece = null;
        turn = 0;
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
    public void ReLoadODChessPiece()
    {
        if (!CheckTurnTime(turn)) return;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        selectPiece = null;
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
    private bool RandomPercent(int turnTime, int k)
    {
        int percent = Random.Range(1, 11);
        if (turnTime == k + 2)
        {
            if (percent % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (turnTime == k + 3)
        {
            if (percent > 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }

    private void Traveler(Chessman chessPiece)
    {
        if (chessPiece.name == "white_pawn" || chessPiece.name == "black_pawn")
        {
            isUsingCard = false;
            selectPiece = chessPiece;

            int randomX, randomY;

            do
            {
                randomX = Random.Range(0, 8);
                randomY = Random.Range(0, 8);
            } while (GameManager.Inst.GetPosition(randomX, randomY) != null);

            selectPiece.SetXBoard(randomX);
            selectPiece.SetYBoard(randomY);
            selectPiece.SetCoords();

            GameManager.Inst.SetPosition(selectPiece);
        }

        else
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }
        DeleteSkill();
        SkillManager.Inst.DeleteSkillList(this);
    }
    private void StreetFriend(Chessman chessPiece)
    {
        isUsingCard = false;
        selectPiece = chessPiece;

        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(129, 0, 1, 0));
    }
    private void Bacchrs()
    {
        isUsingCard = false;
        turn = GetTurnTime() + 1;
    }
    private void TimeWarp()
    {
        if (CardManager.Inst.usedCards.Count == 0)
        {
            CardManager.Inst.SetisBreak(true);
            Debug.Log("사용한 카드가 0개입니다.");
            return;
        }

        isUsingCard = false;

        int random;

        random = Random.Range(0, CardManager.Inst.usedCards.Count);
        CardManager.Inst.AddUsedCard(random);
        DeleteSkill();
    }
    private void Offering(Chessman chessPiece)
    {
        if (chessPiece.name == "black_pawn" || chessPiece.name == "white_pawn")
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }

        isUsingCard = false;
        selectPiece = chessPiece;

        Destroy(selectPiece.gameObject);
    }
    private void Justice()
    {
        isUsingCard = false;

        turn = GetTurnTime() + 2;
    }
    private void GiveBirth(Chessman chessPiece)
    {
        isUsingCard = false;
        selectPiece = chessPiece;
    }
    private void AthenaShield(Chessman chessPiece)
    {
        if (chessPiece.name == "black_king" || chessPiece.name == "white_king")
        {
            CardManager.Inst.SetisBreak(true);
            return;
        }

        isUsingCard = false;
        selectPiece = chessPiece;
        posX = selectPiece.GetXBoard();
        posY = selectPiece.GetYBoard();
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 1, 0));
    }
    private void MoonLight(Chessman chessPiece)
    {
        isUsingCard = false;
        selectPiece = chessPiece;
        selectPiece.spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);

        turn = GetTurnTime() + 3;
    }
    public void DestroyObject()
    {
        SkillManager.Inst.RemoveSkillList(this);
        Destroy(gameObject);
    }
    public void PlusCnt()
    {
        cnt++;
    }
    public void ResetCnt()
    {
        cnt = 0;
    }
    public Chessman GetSelectPiece()
    {
        return selectPiece;
    }
    public void SetSelectPieceTo(Chessman cp)
    {
        selectPieceTo = cp;
    }
    public string GetPalyer()
    {
        return player;
    }
    public void SetPalyer(string player)
    {
        this.player = player;
    }

    public bool CheckPos(int x, int y)
    {
        if (x == posX && y == posY)
            return true;

        else
            return false;
    }

    public Chessman GetSelectPieceTo()
    {
        return selectPieceTo;
    }
    public void CheckML()
    {
        if (selectPiece == null) return;

        if (selectPiece.player != GameManager.Inst.GetCurrentPlayer())
        {
            selectPiece.spriteRenderer.enabled = true;
        }
        else
        {
            selectPiece.spriteRenderer.enabled = false;
        }
    }
    private void Wave(Chessman chessPiece)
    {
        selectPiece = chessPiece;
        WV_MovePlate(selectPiece, selectPiece.GetXBoard(), selectPiece.GetYBoard());
        SkillManager.Inst.SetIsUsingCard(true);
        TurnManager.Instance.ButtonInactive();
    }
    public void StartLOE_Effect()
    {
        StartCoroutine(LOE_SkillEffect());
    }
    public void StartGOD_SkillEffect()
    {
        Debug.Log(turn);
        Debug.Log(GetTurnTime());

        if (!CheckTurnTime(turn)) return;
        StartCoroutine(GOD_SkillEffect());
    }
    public void StartSP_SkillEffect()
    {
        StartCoroutine(SP_SkillEffect());
    }
    public IEnumerator LOE_SkillEffect()
    {
        int k = GetTurnTime() + 4;
        while (GetTurnTime() < k && isUsingCard)
        {
            if (selectPiece == null) yield break;
            selectPieceTo.spriteRenderer.material.color = new Color32(255, 0, 127, 0);
            yield return new WaitForSeconds(0.5f);
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.5f);
        }

        Destroy(gameObject);
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);
        SkillManager.Inst.RemoveSkillList(this);
        selectPiece = null;
        selectPieceTo = null;
        isUsingCard = false;
    }
    private IEnumerator GOD_SkillEffect()
    {
        Chessman cp;
        if (GameManager.Inst.GetPosition(posX, posY) == null)
        {
            god_mp.SetActive(false);
            yield return 0;
        }

        else if (GameManager.Inst.GetPosition(posX, posY) != null)
        {
            god_mp.SetActive(true);
            cp = GameManager.Inst.GetPosition(posX, posY);
            for (int i = 0; i < 5; i++)
            {
                cp.spriteRenderer.material.SetColor("_Color", new Color(1, 0, 0, 0));
                yield return new WaitForSeconds(0.05f);
                cp.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
                yield return new WaitForSeconds(0.05f);
            }
            Destroy(cp.gameObject);
            GameManager.Inst.SetPositionEmpty(posX, posY);
            god_mp.SetActive(false);
        }
    }
    public IEnumerator SP_SkillEffect()
    {
        int k = GetTurnTime() + 2;
        isBreak = true;
        while (GetTurnTime() < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(0, 216, 255, 0);
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 216, 255, 0);
            yield return new WaitForSeconds(0.5f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.5f);
        }
        selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
        selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
        checkSPChessPiece();
        isBreak = false;
    }
    private IEnumerator HP_SkillEffect()
    {
        int k = GetTurnTime() + 2;
        while (GetTurnTime() < k)
        {
            selectPiece.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }
        selectPiece = null;
        SkillManager.Inst.RemoveSkillList(this);
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        Destroy(gameObject);
    }
    private void WV_MovePlate(Chessman chessPiece, int x, int y)
    {
        if (GameManager.Inst.CheckNull(true, true, y) > 0 && x != 7) //우
            chessPiece.WV_MovePlateSpawn(x + 1, y);

        if (GameManager.Inst.CheckNull(true, false, y) > 0 && x != 0) //좌
            chessPiece.WV_MovePlateSpawn(x - 1, y);

        if (GameManager.Inst.CheckNull(false, true, x) > 0 && y != 7) //상
            chessPiece.WV_MovePlateSpawn(x, y + 1);

        if (GameManager.Inst.CheckNull(false, false, x) > 0 && y != 0) //하
            chessPiece.WV_MovePlateSpawn(x, y - 1);
    }
    public void CheckAS()
    {
        if (!CheckPos(selectPiece.GetXBoard(), selectPiece.GetYBoard()) || isAttack)
        {
            posX = 0;
            posY = 0;
            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
            selectPiece = null;
            DeleteSkill();
        }
    }
    public void ReSetWave()
    {
        selectPiece = null;
        DeleteSkill();
    }
    public void ResetML()
    {
        if (selectPiece == null) return;
        selectPiece.spriteRenderer.enabled = true;
        selectPiece.spriteRenderer.material.color = new Color(0, 0, 0, 0);
        selectPiece = null;
        turn = 0;
        DeleteSkill();
    }
    public void ReloadStreetFriend()
    {
        if (selectPiece == null) return;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        selectPiece = null;
        turn = 0;
        DeleteSkill();
    }
    public void ReloadBacchrs()
    {
        turn = 0;
        DeleteSkill();
    }
    public void ReLoadJustice()
    {
        turn = 0;
        DeleteSkill();
    }
    public void DeleteSkill()
    {
        SkillManager.Inst.DeleteSkillList(this);
        Destroy(gameObject);
    }
    public void SelectPieceNull()
    {
        selectPiece = null;
    }
    public void IsAttack(bool isTrue)
    {
        if (isTrue)
            isAttack = true;
        else
            isAttack = false;
    }
    public void SetIsMLMoved(bool isTrue)
    {
        if (isTrue)
            isML_moved = true;
        else
            isML_moved = false;
    }
    public bool isMLMoved()
    {
        return isML_moved;
    }

    private IEnumerator MC_SkillEffect()
    {
        int k = GetTurnTime();
        int cnt = 1;
        while (GetTurnTime() < k + 4)
        {
            if (GetTurnTime() > k + 1)
            {
                if (GetTurnTime() == k + 2 && cnt == 1)
                {
                    cnt++;
                    if (RandomPercent(GetTurnTime(), k))
                        break;
                }
                else if (GetTurnTime() == k + 3 && cnt == 2)
                {
                    cnt++;
                    if (RandomPercent(GetTurnTime(), k))
                        break;
                }

            }

            selectPiece.spriteRenderer.material.color = new Color32(237, 175, 140, 0);
            yield return new WaitForSeconds(0.2f);
            selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.2f);
        }


        SkillManager.Inst.RemoveSkillList(this);
        SkillManager.Inst.RemoveDontClickPiece(selectPiece);
        selectPiece = null;
        Destroy(gameObject);
    }
}