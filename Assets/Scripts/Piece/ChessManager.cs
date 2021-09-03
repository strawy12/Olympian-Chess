using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;


public class ChessManager : MonoBehaviourPunCallbacks
{
    #region SingleTon

    private static ChessManager inst;
    public static ChessManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<ChessManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<ChessManager>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }

    #endregion

    int matrixX, matrixY;

    int pawn = 0, bishop = 1, knight = 2, rook = 3, queen = 4, king = 5;

    int IDs = 0;
    ChessBase cpp;

    [SerializeField] private GameObject[] white; // { white_Pawn, white_Bishop, white_Knight, white_Rook, white_Queen  ,white_King }
    [SerializeField] private GameObject[] black; // black_Bishop, black_King, black_Knight, black_Pawn, black_Queen, black_Rook;

    [SerializeField] private ChessData[,] position = new ChessData[8, 8];

    [SerializeField] private ChessBase[] playerBlack = new ChessBase[16];
    [SerializeField] private ChessBase[] playerWhite = new ChessBase[16];

    [SerializeField] private GameObject cccccp;
    [SerializeField] private GameObject promotionUI;


    [SerializeField] private WaitForSeconds delay = new WaitForSeconds(0.5f);

    private bool isLoading;
    private bool isMoving;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (position[i, j] == null)
                        continue;
                    Debug.Log(i + "," + j + " " + position[i, j].chessPiece);
                }
            }
        }
    }


    public void SettingGame()
    {
        if (GameManager.Inst.GetPlayer() == "white")
        {
            //StartCoroutine(SpawnWhiteChessPiece());

            SpawnWhiteChessPiece();
        }
        else
        {
            //StartCoroutine(SpawnBlackChessPiece());
            SpawnBlackChessPiece();
        }

        StartCoroutine(SetPlayerArr());
        //SetPlayerArr(jsonData);
        
    }

    private void SpawnWhiteChessPiece()
    {
        playerWhite = new ChessBase[]
        {
           Creat(white[pawn], 1,1), Creat(white[pawn], 2,1), Creat(white[pawn], 3,1),
           Creat(white[pawn], 4,1), Creat(white[pawn], 5,1), Creat(white[pawn], 6,1),
           Creat(white[pawn], 7,1), Creat(white[pawn], 0,1), Creat(white[rook], 0,0),
           Creat(white[knight], 1,0), Creat(white[bishop], 2,0), Creat(white[queen], 3,0),
           Creat(white[king], 4,0), Creat(white[bishop], 5,0), Creat(white[knight], 6,0),
           Creat(white[rook], 7,0)
        };
    }
    private void SpawnBlackChessPiece()
    {
        playerBlack = new ChessBase[]
        {
           Creat(black[pawn], 0,6), Creat(black[pawn], 1,6), Creat(black[pawn], 2,6),
           Creat(black[pawn], 3,6), Creat(black[pawn], 4,6), Creat(black[pawn], 5,6),
           Creat(black[pawn], 6,6), Creat(black[pawn], 7,6), Creat(black[rook], 0,7),
           Creat(black[knight], 1,7), Creat(black[bishop], 2,7), Creat(black[queen], 3,7),
           Creat(black[king], 4,7), Creat(black[bishop], 5,7), Creat(black[rook], 7,7),
           Creat(black[knight], 6,7)
        };
    }

    IEnumerator SetPlayerArr()
    {
        yield return new WaitForSeconds(2f);
        ChessBase[] cbs = FindObjectsOfType<ChessBase>();
        string player = GameManager.Inst.GetPlayer() == "white" ? "black" : "white";
        bool isBlack = false;
        if (player == "white")
        {
            isBlack = true;
        }
        for (int i = 0; i < cbs.Length; i++)
        {

            if (isBlack)
            {
                cbs[i].transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }

            if (cbs[i].GetChessData().player == player)
            {
                AddArr(cbs[i]);
            }
        }
        yield return new WaitForSeconds(1f);
        if (!isBlack)
        {
            for (int i = 0; i < playerWhite.Length; i++)
            {
                SetPosition(playerWhite[i]);
            }
        }
        else
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {
                SetPosition(playerBlack[i]);
            }
        }
        Debug.Log(position);
    }

    public ChessBase Creat(GameObject chessPiece, int x, int y)
    {
        GameObject obj = NetworkManager.Inst.SpawnObject(chessPiece);
        ChessBase cb = obj.GetComponent<ChessBase>();
        obj.name = chessPiece.name;
        obj.transform.SetParent(cccccp.transform);

        cb.SetXBoard(x);
        cb.SetYBoard(y);
        //SetCoords(obj, x, y);
        cb.SetCoords();
        SetIds(cb);
        return cb;
    }

    public void SetIds(ChessBase cp)
    {
        if (cp.GetChessData().player == "white")
        {
            cp.SetID(IDs + 100);
        }
        else if (cp.GetChessData().player == "black")
        {
            cp.SetID(IDs + 200);
        }

        IDs++;
    }
    public void PointMovePlate(int x, int y, ChessBase cp)
    {
        if (PositionOnBoard(x, y))
        {
            ChessBase cb = GetPosition(x, y);

            if (cb == null)
            {
                GameManager.Inst.MovePlateSpawn(x, y, cp);
            }

            else if (cb.GetChessData().player != cp.GetChessData().player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x, y, cp);
            }

        }
    }
    public void FalseIsMoving()
    {
        for (int i = 0; i < playerWhite.Length; i++)
        {
            if (playerWhite[i] == null)
                continue;
            playerWhite[i].SetIsMoving(false);
            if (playerBlack[i] == null)
                continue;
            playerBlack[i].SetIsMoving(false);
        }
    }

    public void UpdateArr(ChessBase chessPiece)
    {
        Debug.Log(cpp);
        ChessData chessData = chessPiece.GetChessData();
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, false);
        photonView.RPC("UpdateArr_Pun", RpcTarget.All, jsonData);
    }

    [PunRPC]
    public void UpdateArr_Pun(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        ChessBase chessPiece = GetChessPiece(chessData);
        if (chessPiece == null) return;

        for (int i = 0; i < playerWhite.Length; i++)
        {
            if (playerBlack[i] == null) continue;
            if (playerWhite[i] == chessPiece)
                playerWhite[i] = null;
        }

        for (int i = 0; i < playerBlack.Length; i++)
        {
            if (playerBlack[i] == null) continue;
            if (playerBlack[i] == chessPiece)
                playerBlack[i] = null;
        }
    }


    public void AddArr(ChessBase chessPiece)
    {
        if (chessPiece.GetChessData().player == "white")
        {
            for (int i = 0; i < playerWhite.Length; i++)
            {

                if (playerWhite[i] == null)
                {
                    playerWhite[i] = chessPiece;
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {

                if (playerBlack[i] == null)
                {
                    playerBlack[i] = chessPiece;
                    return;
                }
            }
        }
        //RotationBoard.playerWhite = playerWhite;
        //RotationBoard.playerBlack = playerBlack;
    }
    public bool CheckArr(bool isWhite, string name)
    {
        if (isWhite)
        {
            for (int i = 0; i < playerWhite.Length; i++)
            {
                if (playerWhite[i] == null)
                {
                    continue;
                }

                if (playerWhite[i].GetChessData().chessPiece.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }

        else
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {
                if (playerBlack[i] == null)
                {
                    continue;
                }

                if (playerBlack[i].GetChessData().chessPiece.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }
    }

    #region Position

    [PunRPC]
    public void SetPositionEmpty(int x, int y, bool isSend = true)
    {

        if (isSend)
        {
            photonView.RPC("SetPositionEmpty", RpcTarget.OthersBuffered, x, y, false);
        }

        position[x, y] = null;
    }


    public ChessData CheckPosition(int x, int y)
    {
        return position[x, y];
    }

    public ChessBase GetChessPiece(ChessData chessData)
    {
        if (chessData == null) return null;
        var targetPlayers = chessData.ID < 200 ? playerWhite : playerBlack;

        for (int i = 0; i < 16; i++)
        {

            if (targetPlayers[i] == null)
            {
                continue;
            }

            if (chessData.ID == targetPlayers[i].GetID())
            {
                return targetPlayers[i];
            }
        }
        return null;
    }

    //return positions
    public ChessBase GetPosition(int x, int y)
    {
        if (position[x, y] == null) return null;
        var targetPlayers = position[x, y].ID < 200 ? playerWhite : playerBlack;

        for (int i = 0; i < 16; i++)
        {

            if (targetPlayers[i] == null)
            {
                continue;
            }

            if (position[x, y].ID == targetPlayers[i].GetID())
            {
                return targetPlayers[i];
            }


        }
        return null;
    }

    // Function checking if any chesspiece exists on parameters' value on board
    // exist => true
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= position.GetLength(0) || y >= position.GetLength(1)) return false;
        return true;
    }
    public void SetChessPiecePosition(int x, int y, ChessBase obj)
    {

        position[x, y] = obj.GetChessData();
    }

    public void SetPosition(ChessBase cp)
    {
        if (cp == null) return;
        ChessData chessData = cp.GetChessData();
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, false);

        if(PhotonNetwork.IsMasterClient)
        {
            int x = chessData.xBoard;
            int y = chessData.yBoard;
            position[x, y] = chessData;


            photonView.RPC("SetPosition", RpcTarget.OthersBuffered, jsonData);
        }
        else
        {
            photonView.RPC("SetPosition", RpcTarget.MasterClient, jsonData);
        }



    }

    [PunRPC]
    private void SetPosition(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        if(PhotonNetwork.IsMasterClient)
        {
            int x = chessData.xBoard;
            int y = chessData.yBoard;
            position[x, y] = chessData;
            photonView.RPC("SetPosition", RpcTarget.OthersBuffered, jsonData);
        }
        else
        {
            int x = chessData.xBoard;
            int y = chessData.yBoard;
            position[x, y] = chessData;
        }
        
    }
    #endregion

    public void AttackChessPiece(MovePlate mp)
    {
        ChessBase cp = GetPosition(mp.GetPosX(), mp.GetPosY());

        if (cp.GetAttackSelecting())
        {
            SkillManager.Inst.AttackUsingSkill(mp);
        }

        if (GameManager.Inst.GetIsStop())
        {
            return;
        }

        EnPassant(mp.Getreference(), mp);
        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        DestroyChessPiece(cp.GetChessData());
        if (mp.Getreference().GetIsAttacking())
        {
            GameManager.Inst.RemoveAttackings(mp.Getreference());
        }

        GameManager.Inst.AddAttackings(mp.Getreference());
        SuperSkillManager.Inst.CheckSuperSkill();
    }

    [PunRPC]
    void DestroyRPC(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        ChessBase cp = GetChessPiece(chessData);
        UpdateArr(cp);
        if (SkillManager.Inst.CheckDontClickPiece(cp))
        {
            SkillManager.Inst.RemoveDontClickPiece(cp);
        }
        cp.DestroyChessPiece();
    }

    public void DestroyChessPiece(ChessData chessData)
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, false);
        photonView.RPC("DestroyRPC", RpcTarget.AllBuffered, jsonData);
    }
    public void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        Debug.Log(matrixX + ", " + matrixY);
        SoundManager.Instance.MoveChessSound();

        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        SetPosition(cp);
        cp.SetIsMoving(true);
        cp.SetCoordsAnimation();
        TurnManager.Instance.ButtonActive();
        GameManager.Inst.DestroyMovePlates();
        isMoving = true;
        CastlingKing(cp);
        Promotion(cp);
    }

    public ChessBase[] GetPlayerBlack()
    {
        return playerBlack;
    }

    public ChessBase[] GetPlayerWhite()
    {
        return playerWhite;
    }

    public GameObject[] GetWhiteObject()
    {
        return white;
    }

    public GameObject[] GetBlackObject()
    {
        return black;
    }

    public bool CheckMate(string player)
    {
        List<MovePlate> movePlates;
        string king = player == "black" ? "black_king" : "white_king";
        ChessBase[] cps = player == "black" ? playerWhite : playerBlack;

        for (int i = 0; i < 16; i++)
        {
            if (cps[i] == null) continue;
            if (cps[i].name.Contains(king)) continue;

            cps[i].MovePlate();
            movePlates = GameManager.Inst.GetMovePlates();

            for (int j = 0; j < movePlates.Count; j++)
            {
                if (movePlates[j].GetChessPiece() == null)
                {
                    GameManager.Inst.DestroyMovePlates();
                    continue;
                }

                if (movePlates[j].GetChessPiece().name.Contains(king))
                {
                    GameManager.Inst.DestroyMovePlates();
                    return true;
                }
            }

            GameManager.Inst.DestroyMovePlates();
        }

        GameManager.Inst.DestroyMovePlates();
        return false;
    }

    public bool KingAndRook(string player, bool isKing)
    {
        if (player == "white" && isKing)
        {
            if (GetPosition(5, 0) == null && GetPosition(6, 0) == null)
                return true;
        }

        else if (player == "white" && !isKing)
        {
            if (GetPosition(1, 0) == null && GetPosition(2, 0) == null && GetPosition(3, 0) == null)
                return true;
        }

        else if (player == "black" && isKing)
        {
            if (GetPosition(5, 7) == null && GetPosition(6, 7) == null)
                return true;
        }

        else if (player == "black" && !isKing)
        {
            if (GetPosition(1, 7) == null && GetPosition(2, 7) == null && GetPosition(3, 7) == null)
                return true;
        }

        return false;
    }

    public bool CheckMovePlate(string player, int x, int y)
    {
        List<MovePlate> movePlates;
        ChessBase[] chessPieces = player == "black" ? playerWhite : playerBlack;
        string king = player == "black" ? "black_king" : "white_king";

        for (int i = 0; i < chessPieces.Length; i++)
        {
            if (chessPieces[i] == null) continue;
            if (chessPieces[i].name.Contains(king)) continue;

            chessPieces[i].MovePlate();
            movePlates = GameManager.Inst.GetMovePlates();

            if(Array.Exists(movePlates.ToArray(), z => (z.GetPosX() == x && z.GetPosY() == y)))
            {
                GameManager.Inst.DestroyMovePlates();
                return true;
            }

            GameManager.Inst.DestroyMovePlates();
        }

        GameManager.Inst.DestroyMovePlates();
        return false;
    }

    public bool Castling(string player, int moveCnt, bool isKing)
    {
        if (player == "white" && moveCnt == 0)
        {
            if (isKing && KingAndRook(player, true))
            {
                if (!CheckMate(player))
                    return true;
            }

            else if (!isKing && KingAndRook(player, false))
            {
                if (!CheckMate(player))
                    return true;
            }
        }

        else if (player == "black" && moveCnt == 0)
        {
            if (isKing && KingAndRook(player, true))
            {
                if (!CheckMate(player))
                    return true;
            }

            else if (!isKing && KingAndRook(player, false))
            {
                if (!CheckMate(player))
                    return true;
            }
        }
        return false;
    }

    public void RookCastling(string player, bool isKing)
    {
        Debug.Log("·è ¿Å±â±â");

        ChessBase rook;

        if (player == "white")
        {
            if (isKing)
            {
                rook = GetPosition(7, 0);
                MoveChessPiece(rook, 5, 0);
            }

            else
            {
                rook = GetPosition(0, 0);
                MoveChessPiece(rook, 3, 0);
            }
        }

        else
        {
            if (isKing)
            {
                rook = GetPosition(7, 7);
                MoveChessPiece(rook, 5, 7);
            }
            else
            {
                rook = GetPosition(0, 7);
                MoveChessPiece(rook, 3, 7);
            }
        }
    }

    private void CastlingKing(ChessBase cp)
    {
        if (cp.name == "white_king")
        {
            if (cp.GetMoveCnt() == 1)
            {
                if (cp.GetXBoard() == 6 && cp.GetYBoard() == 0)
                    RookCastling("white", true);
                else if (cp.GetXBoard() == 2 && cp.GetYBoard() == 0)
                    RookCastling("white", false);
            }
        }

        else if (cp.name == "black_king")
        {
            if (cp.GetXBoard() == 6 && cp.GetYBoard() == 7)
                RookCastling("black", true);
            else if (cp.GetXBoard() == 2 && cp.GetYBoard() == 7)
                RookCastling("black", false);
        }
    }

    private void EnPassant(ChessBase cp, MovePlate mp)
    {
        int x, y;
        int x2;

        if (cp.name.Contains("pawn") && cp.GetMoveCnt() == 2 && mp.GetChessPiece().name.Contains("pawn"))
        {
            if (mp.GetPosX() == cp.GetXBoard() + 1 || mp.GetPosX() == cp.GetXBoard() - 1)
            {
                if (mp.GetPosY() != cp.GetYBoard()) return;

                x = mp.GetPosX();
                y = cp.name.Contains("white") ? mp.GetPosY() + 1 : mp.GetPosY() - 1;
                x2 = x == cp.GetXBoard() + 1 ? x - 2 : x + 2;

                if (PositionOnBoard(x, y) && GetPosition(x, y) == null)
                    mp.SetCoords(x, y);

                else if (PositionOnBoard(x2, y) && GetPosition(x2, y) == null)
                    mp.SetCoords(x2, y);

                else if (PositionOnBoard(cp.GetXBoard(), y) && GetPosition(cp.GetXBoard(), y) == null)
                    mp.SetCoords(cp.GetXBoard(), y);

                else
                    mp.SetCoords(cp.GetXBoard(), cp.GetYBoard());
            }
        }
    }

    private void Promotion(ChessBase cp)
    {
        if (cp.name.Contains("white_pawn"))
        {
            if (cp.GetYBoard() == 7)
            {
                promotionUI.SetActive(true);
                promotionUI.transform.GetChild(1).gameObject.SetActive(true);
                promotionUI.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        else if (cp.name.Contains("black_pawn"))
        {
            if (cp.GetYBoard() == 0)
            {
                promotionUI.SetActive(true);
                promotionUI.transform.GetChild(0).gameObject.SetActive(true);
                promotionUI.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        cpp = cp;
    }

    public void ExecutePromotion(int cp)
    {
        ChessBase promotion;

        if (cpp.GetPlayer() == "white")
        {
            promotion = Creat(white[cp], cpp.GetXBoard(), cpp.GetYBoard());
        }

        else
        {
            promotion = Creat(black[cp], cpp.GetXBoard(), cpp.GetYBoard());
        }

        DestroyChessPiece(cpp.GetChessData());
        photonView.RPC("ChangePiece", RpcTarget.AllBuffered, promotion.gameObject.GetPhotonView().ViewID);
    }

    [PunRPC]
    private void ChangePiece(int num)
    {
        GameObject obj = PhotonView.Find(num).gameObject;
        AddArr(obj.GetComponent<ChessBase>());
        if (GameManager.Inst.GetPlayer() == "white") return;
        obj.transform.Rotate(0f, 0f, 180f);
    }

    public void SetIsMoving(bool isTrue)
    {
        isMoving = isTrue;
    }
    public bool GetIsMoving()
    {
        return isMoving;
    }
}