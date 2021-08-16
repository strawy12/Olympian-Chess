//using System;
//using System.Collections;
//using UnityEngine;
//using Random = UnityEngine.Random;

//public class Skill : MonoBehaviour
//{
//    #region SerializeField Var
//    [SerializeField] new Animation animation;
//    #endregion

//    #region Var List
//    private Chessman selectPiece;
//    private Chessman selectPieceTo;

//    private bool isUsingCard = false;
//    private bool isML_moved = false;
//    private int posX;
//    private int posY;
//    private bool isAttack;
//    public int turn { get; private set; } = 0;
//    private string player = "white";
//    public bool isBreak { get; private set; } = true;
//    public int cnt { get; private set; } = 0;
//    private GameObject god_mp;
//    #endregion

//    #region System Check
//    Functions checking if the parameter is equal to current player
//    private bool CheckPlayer(string player)
//    {
//        if (GameManager.Inst.GetCurrentPlayer() == player)
//            return true;
//        else
//            return false;
//    }

//    Function that returns whether parameter(turn) is greater than turntime.
//     if(turn<turnTime) => return true => this skill is over
//    private bool CheckTurnTime(int turn)
//    {
//        return SkillManager.Inst.CheckTurnTime(turn);
//    }

//    Function to check if the chess piece of sleep have moved


//    function checking 2D position(x and y)
//     if(parameter value(x, y) == location(posX, posY)) => true
//    public bool CheckPos(int x, int y)
//    {
//        if (x == posX && y == posY)
//            return true;

//        else
//            return false;
//    }

//    function checking whether selected piece of Moonlight is visible
//    public void CheckML()
//    {
//        if (selectPiece == null) return;

//        if (selectPiece.player != GameManager.Inst.GetCurrentPlayer())
//        {
//            selectPiece.spriteRenderer.enabled = true;
//        }
//        else
//        {
//            selectPiece.spriteRenderer.enabled = false;
//        }
//    }

//    Function to determine whether Athena's shield is maintained or not
//    public void CheckAS()
//    {
//        if (!CheckPos(selectPiece.GetXBoard(), selectPiece.GetYBoard()) || isAttack)
//        {
//            posX = 0;
//            posY = 0;
//            selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
//            selectPiece = null;
//            1();
//        }
//    }

//    #endregion

//    #region External Access
//    Function getting turnTime of SkillManager
//    private int GetTurnTime()
//    {
//        return SkillManager.Inst.turnTime;
//    }

//    Function Setting selected piece to null
//    public void SelectPieceNull()
//    {
//        selectPiece = null;
//    }

//    function starting LOE_SkillEffect Coroutine
//    public void StartLOE_Effect()
//    {
//        StartCoroutine(LOE_SkillEffect());
//    }

//    function starting Ground of Death Skill Effect
//    public void StartGOD_SkillEffect()
//    {
//        if (!CheckTurnTime(turn)) return;
//        StartCoroutine(GOD_SkillEffect());
//    }

//    function starting Ground of Death Skill Effect
//    public void StartSP_SkillEffect()
//    {
//        StartCoroutine(SP_SkillEffect());
//    }
//    #endregion

//    #region Script Access 
//    Function setting isUsingCard to parameter value
//    public void SetIsUsingCard(bool isUsingCard)
//    {
//        this.isUsingCard = isUsingCard;
//    }

//    function returning selectPiece
//    public Chessman GetSelectPiece()
//    {
//        return selectPiece;
//    }

//    function setting selectPieceto to parameter value
//    public void SetSelectPieceTo(Chessman cp)
//    {
//        selectPieceTo = cp;
//    }

//    function returning player
//    public string GetPalyer()
//    {
//        return player;
//    }

//    function setting player to parameter
//    public void SetPalyer(string player)
//    {
//        this.player = player;
//    }

//    function returning selectPieceTo
//    public Chessman GetSelectPieceTo()
//    {
//        return selectPieceTo;
//    }

//    Function setting isAttack value based on parameter value
//    public void IsAttack(bool isTrue)
//    {
//        if (isTrue)
//            isAttack = true;
//        else
//            isAttack = false;
//    }

//    Function setting isML_moved value based on parameter value
//    public void SetIsMLMoved(bool isTrue)
//    {
//        if (isTrue)
//            isML_moved = true;
//        else
//            isML_moved = false;
//    }

//    Function returning isML_moved
//    public bool isMLMoved()
//    {
//        return isML_moved;
//    }
//    #endregion

//    #region Skill System
//    Function using card based on parameter values
//    public void UseSkill(Card card, Chessman chessPiece)
//{
//    if (card == null) return;
//    gameObject.name = card.carditem.name;
//    switch (card.carditem.name)
//    {
//        case "천벌":
//            HeavenlyPunishment(chessPiece);
//            break;
//        case "에로스의 사랑":
//            LoveOfEros(chessPiece);
//            break;

//        case "음악":
//            Music(chessPiece);
//            break;
//        case "돌진":
//            Rush(chessPiece);
//            break;
//        case "여행자":
//            Traveler(chessPiece);
//            break;
//        case "길동무":
//            StreetFriend(chessPiece);
//            break;
//        case "바카스":
//            Bacchrs();
//            break;
//        case "시간왜곡":
//            TimeWarp();
//            break;
//        case "제물":
//            Offering(chessPiece);
//            break;
//        case "정의구현":
//            Justice();
//            break;
//        case "출산":
//            GiveBirth(chessPiece);
//            break;
//        case "아테나의 방패":
//            AthenaShield(chessPiece);
//            break;
//        case "달빛":
//            MoonLight(chessPiece);
//            break;
//        case "파도":
//            Wave(chessPiece);
//            break;
//        case "서풍":
//            WestWind(chessPiece);
//            break;
//        case "수중감옥":
//            OceanJail(chessPiece);
//            break;
//        case "질서":
//            Order(chessPiece);
//            break;
//        case "죽음의 땅":
//            GroundOfDeath(chessPiece);
//            break;
//        case "전쟁광":
//            WarBuff(chessPiece);
//            break;
//    }
//}

//function adding cnt
//    public void PlusCnt()
//{
//    cnt++;
//}

//function resetting cnt to zero
//    public void ResetCnt()
//{
//    cnt = 0;
//}

//Functions setting true or false randomly
//    private bool RandomPercent(int turnTime, int k)
//{
//    int percent = Random.Range(1, 11);
//    if (turnTime == k + 2)
//    {
//        if (percent % 2 == 0)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    else if (turnTime == k + 3)
//    {
//        if (percent > 7)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    else
//        return false;
//}

//function removing this from the skill list
//    Use it when the skill's ability ends
//    public void DestroyObject()
//{
//    SkillManager.Inst.RemoveSkillList(this);
//    Destroy(gameObject);
//}

//Function Removing skill from skill list
//    public void DeleteSkill()
//{
//    SkillManager.Inst.DeleteSkillList(this);
//    Destroy(gameObject);
//}

//Function spawning move plates of Wave
//    private void WV_MovePlate(Chessman chessPiece, int x, int y)
//{
//    if (GameManager.Inst.CheckNull(true, true, y) > 0 && x != 7) //right
//        chessPiece.WV_MovePlateSpawn(x + 1, y);

//    if (GameManager.Inst.CheckNull(true, false, y) > 0 && x != 0) //left
//        chessPiece.WV_MovePlateSpawn(x - 1, y);

//    if (GameManager.Inst.CheckNull(false, true, x) > 0 && y != 7) //up
//        chessPiece.WV_MovePlateSpawn(x, y + 1);

//    if (GameManager.Inst.CheckNull(false, false, x) > 0 && y != 0) //down
//        chessPiece.WV_MovePlateSpawn(x, y - 1);
//}

//#endregion

//#region Skill Functions
//Love of Eros function
//    private void LoveOfEros(Chessman chessPiece)
//{
//    selectPiece = chessPiece;
//    selectPiece.SetMySkill(true);
//    selectPiece.SetMovePlateColor(new Color32(29, 219, 22, 255));
//    selectPiece.MovePlateSpawn(selectPiece.GetXBoard(), selectPiece.GetYBoard());
//    selectPiece.SetMovePlateColor(new Color32(255, 255, 36, 255));
//    selectPiece.SetMySkill(false);
//    GameManager.Inst.AllMovePlateSpawn(selectPiece, true);
//    isUsingCard = true;
//    SkillManager.Inst.SetIsUsingCard(true);
//}

//HeavenlyPunishment function
//    private void HeavenlyPunishment(Chessman chessPiece)
//{
//    Preventing King from being the target of HeavenlyPunishment
//        if (chessPiece.name == "black_king" || chessPiece.name == "white_king")
//    {
//        CardManager.Inst.SetisBreak(true);
//        return;
//    }

//    if the opposing team has a rook or rooks,
//        Preventing Queen from being the target of HeavenlyPunishment
//        if (chessPiece.name == "black_queen" || chessPiece.name == "white_queen")
//    {
//        if (CheckPlayer("white"))
//            isBreak = GameManager.Inst.CheckArr(false, "black_rook");
//        else
//            isBreak = GameManager.Inst.CheckArr(true, "white_rook");
//        if (isBreak)
//        {
//            CardManager.Inst.SetisBreak(true);
//            return;
//        }

//    }
//    selectPiece = chessPiece;
//    StartCoroutine(HP_SkillEffect());
//    CardManager.Inst.SetisBreak(false);
//    isUsingCard = false;
//    SkillManager.Inst.SetDontClickPiece(selectPiece);
//}

//Sleep function
//    private void Sleep(Chessman chessPiece)
//{

//}

//West wind function
//    private void WestWind(Chessman chessPiece)
//{
//    selectPiece = chessPiece;
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(215, 199, 176, 144));
//    posX = selectPiece.GetXBoard();
//    posY = selectPiece.GetYBoard();
//    selectPiece.spriteRenderer.sortingOrder = -2;
//    selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
//    GameManager.Inst.SetPositionEmpty(posX, posY);
//    CardManager.Inst.SetisBreak(false);
//    isUsingCard = false;
//    turn = GetTurnTime() + 1;
//    isBreak = false;

//}

//Rush function
//    private void Rush(Chessman chessPiece)
//{
//    isUsingCard = false;
//    selectPiece = chessPiece;

//    int posX_rush;
//    int posY_rush;

//    posX_rush = selectPiece.GetXBoard();
//    posY_rush = selectPiece.GetYBoard();

//    if color of selected piece is white,
//        selected piece moves up one space
//        if (selectPiece.player == "white")
//    {
//        if (GameManager.Inst.GetPosition(posX_rush, posY_rush + 1) == null)
//        {
//            GameManager.Inst.SetPositionEmpty(posX_rush, posY_rush);

//            selectPiece.SetchessData.xBoard(posX_rush);
//            selectPiece.SetYBoard(posY_rush + 1);
//            selectPiece.SetCoords();

//            GameManager.Inst.SetPosition(selectPiece);
//            CardManager.Inst.ChangeIsUse(true);
//            CardManager.Inst.SetisBreak(false);

//        }

//        if the space to go is not empty, Use of the card is canceled.
//            else
//        {
//            CardManager.Inst.SetisBreak(true);
//        }
//    }
//    else
//    {
//        if color of selected piece is black,
//            selected piece moves down one space
//            if (GameManager.Inst.GetPosition(posX_rush, posY_rush - 1) == null)
//        {
//            GameManager.Inst.SetPositionEmpty(posX_rush, posY_rush);

//            selectPiece.SetchessData.xBoard(posX_rush);
//            selectPiece.SetYBoard(posY_rush - 1);
//            selectPiece.SetCoords();

//            CardManager.Inst.ChangeIsUse(true);
//            CardManager.Inst.SetisBreak(false);
//            GameManager.Inst.SetPosition(selectPiece);
//        }
//        if the space to go is not empty, Use of the card is canceled.
//            else
//        {
//            CardManager.Inst.SetisBreak(true);
//        }

//    }
//    Because of the immediate use of card, it is removed from the skill list.
//    DeleteSkill();
//}

//Music function
//    private void Music(Chessman chessPiece)
//{
//    Preventing King from being the target of Music
//        if (chessPiece.name == "black_king" || chessPiece.name == "white_king")
//    {
//        CardManager.Inst.SetisBreak(true);
//        return;
//    }

//    selectPiece = chessPiece;
//    StartCoroutine(MC_SkillEffect());
//    CardManager.Inst.SetisBreak(false);
//    isUsingCard = false;
//    SkillManager.Inst.SetDontClickPiece(selectPiece);
//}

//OceanJail function
//    private void OceanJail(Chessman chessPiece)
//{
//    selectPiece = chessPiece;
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 255, 144));
//    posX = selectPiece.GetXBoard();
//    posY = selectPiece.GetYBoard();
//    selectPiece.spriteRenderer.sortingOrder = -2;
//    selectPiece.gameObject.GetComponent<Collider2D>().enabled = false;
//    GameManager.Inst.SetPositionEmpty(posX, posY);
//    CardManager.Inst.SetisBreak(false);
//    isUsingCard = false;
//    turn = GetTurnTime() + 2;
//    isBreak = false;

//}

//Order function
//    private void Order(Chessman chessPiece)
//{
//    turn = GetTurnTime() + 2;
//    isUsingCard = false;
//    selectPiece = chessPiece;
//    CardManager.Inst.SetisBreak(false);
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 144));
//    isBreak = false;
//}

//Ground of death function
//    private void GroundOfDeath(Chessman chesspiece)
//{
//    Only pawns are the target of Ground of death
//        if (chesspiece.name == "white_pawn" || chesspiece.name == "black_pawn")
//    {
//        isUsingCard = false;

//        posX = chesspiece.GetXBoard();
//        posY = chesspiece.GetYBoard();
//        god_mp = chesspiece.GOD_MovePlateSpawn(posX, posY);

//        turn = GetTurnTime() + 1;
//        isBreak = false;
//    }
//    if the pieces are not pawns, use of card is canceled
//        else
//        CardManager.Inst.SetisBreak(true);
//}


//War buff Function
//    private void WarBuff(Chessman chessPiece)
//{

//}
//}

//Traveler Function
//    private void Traveler(Chessman chessPiece)
//{
//    Only pawns are the target of Traveler
//        if (chessPiece.name == "white_pawn" || chessPiece.name == "black_pawn")
//    {
//        isUsingCard = false;
//        selectPiece = chessPiece;

//        int randomX, randomY;

//        randomly set a location(x, y)
//            do
//        {
//            randomX = Random.Range(0, 8);
//            randomY = Random.Range(0, 8);
//        } while (GameManager.Inst.GetPosition(randomX, randomY) != null);

//        selectPiece.SetchessData.xBoard(randomX);
//        selectPiece.SetYBoard(randomY);
//        selectPiece.SetCoords();

//        GameManager.Inst.SetPosition(selectPiece);
//    }

//    if the pieces are not pawns, use of card is canceled
//        else
//    {
//        CardManager.Inst.SetisBreak(true);
//        return;
//    }
//    DeleteSkill();
//    SkillManager.Inst.DeleteSkillList(this);
//}

//Street Friend Function
//    private void StreetFriend(Chessman chessPiece)
//{
//    isUsingCard = false;
//    selectPiece = chessPiece;

//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(129, 0, 1, 0));
//}

//Bacchrs Function
//    private void Bacchrs()
//{
//    isUsingCard = false;
//    turn = GetTurnTime() + 1;
//}

//Time warp Function
//    private void TimeWarp()
//{
//    if number of used card is zero, 
//         use of this card is canceled
//        if (CardManager.Inst.GetUsedCards().Count == 0)
//    {
//        CardManager.Inst.SetisBreak(true);
//        Debug.Log("사용한 카드가 0개입니다.");
//        return;
//    }

//    isUsingCard = false;

//    int random;

//    random = Random.Range(0, CardManager.Inst.GetUsedCards().Count);
//    CardManager.Inst.AddUsedCard(random);
//    DeleteSkill();
//}

//Offering function
//    private void Offering(Chessman chessPiece)
//{
//    Preventing Pawns from being the target of Offering
//        if (chessPiece.name == "black_pawn" || chessPiece.name == "white_pawn")
//    {
//        CardManager.Inst.SetisBreak(true);
//        return;
//    }

//    isUsingCard = false;
//    selectPiece = chessPiece;

//    Destroy(selectPiece.gameObject);
//}

//Justice function
//    private void Justice()
//{
//    isUsingCard = false;

//    turn = GetTurnTime() + 2;
//}

//GiveBirth function
//    private void GiveBirth(Chessman chessPiece)
//{
//    isUsingCard = false;
//    selectPiece = chessPiece;
//}

//Shield of Athena function
//    private void AthenaShield(Chessman chessPiece)
//{
//    Preventing Kings from being the target of Shield of Athena
//        if (chessPiece.name == "black_king" || chessPiece.name == "white_king")
//    {
//        CardManager.Inst.SetisBreak(true);
//        return;
//    }

//    isUsingCard = false;
//    selectPiece = chessPiece;
//    posX = selectPiece.GetXBoard();
//    posY = selectPiece.GetYBoard();
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 1, 0));
//}

//Moon light function
//    private void MoonLight(Chessman chessPiece)
//{
//    isUsingCard = false;
//    selectPiece = chessPiece;
//    selectPiece.spriteRenderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0f);

//    turn = GetTurnTime() + 3;
//}

//Wave function
//    private void Wave(Chessman chessPiece)
//{
//    selectPiece = chessPiece;
//    WV_MovePlate(selectPiece, selectPiece.GetXBoard(), selectPiece.GetYBoard());
//    SkillManager.Inst.SetIsUsingCard(true);
//    TurnManager.Instance.ButtonInactive();
//}
//#endregion

//#region Skill ReLoad
//A function that removes this and returns what has changed by Western Wind
//     when Western Wind is over,
//    public void ReLoadWWChessPiece()
//{
//    if (!CheckTurnTime(turn)) return;
//    selectPiece.spriteRenderer.sortingOrder = 0;
//    selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
//    if (GameManager.Inst.GetPosition(posX, posY) != null && GameManager.Inst.GetPosition(posX, posY).player != selectPiece.player)
//    {
//        Destroy(GameManager.Inst.GetPosition(posX, posY).gameObject);
//        GameManager.Inst.SetPositionEmpty(posX, posY);
//    }
//    GameManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
//    selectPiece = null;
//    turn = 0;
//    SkillManager.Inst.RemoveSkillList(this);
//    Destroy(gameObject);
//}

//Function that removes this and returns what has changed by Ocean Jail
//     when Ocean Jail is over,
//    public void ReLoadOJChessPiece()
//{
//    if (!CheckTurnTime(turn)) return;
//    selectPiece.spriteRenderer.sortingOrder = 0;
//    selectPiece.gameObject.GetComponent<Collider2D>().enabled = true;
//    if (GameManager.Inst.GetPosition(posX, posY) != null)
//    {
//        Destroy(GameManager.Inst.GetPosition(posX, posY).gameObject);
//        GameManager.Inst.SetPositionEmpty(posX, posY);
//    }
//    GameManager.Inst.SetChessPiecePosition(posX, posY, selectPiece);
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
//    selectPiece = null;
//    turn = 0;
//    SkillManager.Inst.RemoveSkillList(this);
//    Destroy(gameObject);
//}


//Function that removes this and returns what has changed by Order
//     when Order is over,
//    public void ReLoadODChessPiece()
//{
//    if (!CheckTurnTime(turn)) return;
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
//    selectPiece = null;
//    SkillManager.Inst.RemoveSkillList(this);
//    Destroy(gameObject);
//}

//Function Removing and Resetting Wave from skill list
//    public void ReSetWave()
//{
//    selectPiece = null;
//    DeleteSkill();
//}

//Function Removing and Resetting Moonlight from skill list
//    public void ResetML()
//{
//    if (selectPiece == null) return;
//    selectPiece.spriteRenderer.enabled = true;
//    selectPiece.spriteRenderer.material.color = new Color(0, 0, 0, 0);
//    selectPiece = null;
//    turn = 0;
//    DeleteSkill();
//}

//Function Removing and Resetting Street Friend from skill list
//    public void ReloadStreetFriend()
//{
//    if (selectPiece == null) return;
//    selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
//    selectPiece = null;
//    turn = 0;
//    DeleteSkill();
//}

//Function Removing and Resetting Bacchrs from skill list
//    public void ReloadBacchrs()
//{
//    turn = 0;
//    DeleteSkill();
//}

//Function Removing and Resetting Justice from skill list
//    public void ReLoadJustice()

//{
//    turn = 0;
//    DeleteSkill();
//}

//#endregion

//#region SkillEffect Coroutine

//Coroutine of effect of Love of Eros
//    public IEnumerator LOE_SkillEffect()
//{
//    int k = GetTurnTime() + 4;
//    sparkling effect(pink)
//        while (GetTurnTime() < k && isUsingCard)
//    {
//        if (selectPiece == null) yield break;
//        selectPieceTo.spriteRenderer.material.color = new Color32(255, 0, 127, 0);
//        yield return new WaitForSeconds(0.5f);
//        selectPieceTo.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
//        yield return new WaitForSeconds(0.5f);
//    }

//    When card time is over, destroy gameObject
//        Destroy(gameObject);
//    SkillManager.Inst.RemoveDontClickPiece(selectPiece);
//    SkillManager.Inst.RemoveDontClickPiece(selectPieceTo);
//    SkillManager.Inst.RemoveSkillList(this);
//    selectPiece = null;
//    selectPieceTo = null;
//    isUsingCard = false;
//}
//Coroutine of effect of Ground of Death

//    private IEnumerator GOD_SkillEffect()
//{
//    Chessman cp;
//    if the ground of death is empty
//    move plate of ground of death is invisible
//        if (GameManager.Inst.GetPosition(posX, posY) == null)
//    {
//        god_mp.SetActive(false);
//        yield return 0;
//    }

//    if the ground of death is not empty,
//         explosion effect is towards chess piece on ground of death
//         and destroy the chess piece and move plate
//        else if (GameManager.Inst.GetPosition(posX, posY) != null)
//    {
//        god_mp.SetActive(true);
//        cp = GameManager.Inst.GetPosition(posX, posY);
//        sparkling effect(red)
//            for (int i = 0; i < 5; i++)
//        {
//            cp.spriteRenderer.material.SetColor("_Color", new Color(1, 0, 0, 0));
//            yield return new WaitForSeconds(0.05f);
//            cp.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
//            yield return new WaitForSeconds(0.05f);
//        }
//        Destroy(cp.gameObject);
//        GameManager.Inst.SetPositionEmpty(posX, posY);
//        god_mp.SetActive(false);
//    }
//}

//Coroutine of effect of Sleep


//Coroutine of effect of HeavenlyPunishment
//    private IEnumerator HP_SkillEffect()
//{
//    int k = GetTurnTime() + 2;
//    sparkling effect(yellow)
//        while (GetTurnTime() < k)
//    {
//        selectPiece.spriteRenderer.material.color = new Color32(255, 228, 0, 0);
//        yield return new WaitForSeconds(0.2f);
//        selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
//        yield return new WaitForSeconds(0.2f);
//    }
//    When card time is over, selected pieces turn to original color
//        selectPiece = null;
//    SkillManager.Inst.RemoveSkillList(this);
//    SkillManager.Inst.RemoveDontClickPiece(selectPiece);
//    Destroy(gameObject);
//}

//Music effect function
//    private IEnumerator MC_SkillEffect()
//{
//    int k = GetTurnTime();
//    int cnt = 1;
//    Randomly determines whether or not there is a sparkling effect
//        while (GetTurnTime() < k + 4)
//    {
//        if (GetTurnTime() > k + 1)
//        {
//            if (GetTurnTime() == k + 2 && cnt == 1)
//            {
//                cnt++;
//                if (RandomPercent(GetTurnTime(), k))
//                    break;
//            }
//            else if (GetTurnTime() == k + 3 && cnt == 2)
//            {
//                cnt++;
//                if (RandomPercent(GetTurnTime(), k))
//                    break;
//            }
//        }

//        selectPiece.spriteRenderer.material.color = new Color32(237, 175, 140, 0);
//        yield return new WaitForSeconds(0.2f);
//        selectPiece.spriteRenderer.material.color = new Color32(0, 0, 0, 0);
//        yield return new WaitForSeconds(0.2f);
//    }

//    if use of this card is over,
//         remove this from skill list and destroy this game object
//        SkillManager.Inst.RemoveSkillList(this);
//    SkillManager.Inst.RemoveDontClickPiece(selectPiece);
//    selectPiece = null;
//    Destroy(gameObject);
//}

//    #endregion


//}