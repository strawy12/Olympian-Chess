using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;


public class ChessManager : MonoBehaviourPunCallbacks
{
    //black_pawn
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

    [SerializeField] private GameObject[] white; // { white_Pawn, white_Bishop, white_Knight, white_Rook, white_Queen  ,white_King }
    [SerializeField] private GameObject[] black; // black_Bishop, black_King, black_Knight, black_Pawn, black_Queen, black_Rook;

    [SerializeField] private ChessData[,] position = new ChessData[8, 8];

    [SerializeField] private ChessBase[] playerBlack = new ChessBase[16];
    [SerializeField] private ChessBase[] playerWhite = new ChessBase[16];

    [SerializeField] private GameObject cccccp;

    [SerializeField] private WaitForSeconds delay = new WaitForSeconds(0.5f);

    private PositionData positionData;
    private bool isLoading;




    void Start()
    {
        positionData = new PositionData(new ChessData[,] { });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
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
        if (NetworkManager.Inst.GetPlayer() == "white")
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
        string player = NetworkManager.Inst.GetPlayer() == "white" ? "black" : "white";
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
    public bool CheckArr(bool isPlayer, string name)
    {
        if (isPlayer)
        {
            for (int i = 0; i < playerWhite.Length; i++)
            {
                if (playerWhite[i] == null)
                    continue;
                if (playerWhite[i].gameObject.name == name)
                    return true;

            }
            return false;
        }
        else
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {
                if (playerBlack[i] == null)
                    continue;
                if (playerBlack[i].gameObject.name == name)
                    return true;
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
        bool isWhite = false;
        if (chessData == null) return null;
        Debug.Log(chessData.ID);
        if (chessData.ID < 200)
        {
            isWhite = true;
        }
        for (int i = 0; i < 16; i++)
        {
            if (isWhite)
            {
                if (playerWhite[i] == null)
                {
                    continue;
                }

                if (chessData.ID == playerWhite[i].GetID())
                {
                    return playerWhite[i];
                }
            }
            else
            {
                
                if (playerBlack[i] == null)
                {
                    continue;
                }

                if (chessData.ID == playerBlack[i].GetID())
                {
                    
                    return playerBlack[i];
                }
            }
        }
        return null;
    }
    //return positions
    public ChessBase GetPosition(int x, int y)
    {
        if (position[x, y] == null) return null;
        bool isWhite = false;
        

        if(position[x, y].ID < 200)
        {
            isWhite = true;
        }
        for (int i = 0; i < 16; i++)
        {
            if(isWhite)
            {
                if (playerWhite[i] == null)
                {
                    continue;
                }

                if (position[x, y].ID == playerWhite[i].GetID())
                {
                    return playerWhite[i];
                }
            }
            else
            {

                if (playerBlack[i] == null)
                {
                    continue;
                }

                if (position[x, y].ID == playerBlack[i].GetID())
                {
                    return playerBlack[i];
                }
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
        int x = chessData.xBoard;
        int y = chessData.yBoard;
        position[x, y] = chessData;
        //Debug.Log(x + "," + y + position[x, y].chessPiece);

        SendPositionData(chessData);
    }

    private void SendPositionData(ChessData chessData)
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, true);
        photonView.RPC("SetPositionData", RpcTarget.OthersBuffered, jsonData);
    }

    [PunRPC]
    private void SetPositionData(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        int x = chessData.xBoard;
        int y = chessData.yBoard;
        position[x, y] = chessData;
        //Debug.Log(x + "," + y + position[x, y].chessPiece);
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

        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        DestroyChessPiece(cp.GetChessData());

        if (mp.Getreference().GetIsAttacking())
        {
            GameManager.Inst.RemoveAttackings(mp.Getreference());
        }

        GameManager.Inst.AddAttackings(mp.Getreference());
    }

    [PunRPC]
    void DestroyRPC(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        ChessBase cp = GetChessPiece(chessData);
        UpdateArr(cp);
        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        if(SkillManager.Inst.CheckDontClickPiece(cp))
        {
            SkillManager.Inst.RemoveDontClickPiece(cp);
        }
        cp.DestroyChessPiece();
    }

    public void DestroyChessPiece(ChessData chessData)
    {
        Debug.Log(chessData.chessPiece);
        string jsonData = NetworkManager.Inst.SaveDataToJson(chessData, false);
        photonView.RPC("DestroyRPC", RpcTarget.AllBuffered, jsonData);
    }
    public void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        SetPosition(cp);
        cp.SetIsMoving(true);
        cp.SetCoordsAnimation();
        //StartCoroutine(SetCoordsAnimation());
        TurnManager.Instance.ButtonActive();
        GameManager.Inst.DestroyMovePlates();
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


    //public void SetCoords(GameObject obj, int xBoard, int yBoard)
    //{
    //    Debug.Log(obj.name);
    //    float x = xBoard;
    //    float y = yBoard;

    //    x *= 0.684f;
    //    y *= 0.684f;

    //    x += -2.4f;
    //    y += -2.4f;

    //    // Aligns according the board
    //    obj.transform.position = new Vector3(x, y, -1.0f);
    //}
    //public IEnumerator SetCoordsAnimation(ChessBase cp)
    //{
    //    Vector3 startPos = cp.transform.position;

    //    float x = cp.GetXBoard();
    //    float y = cp.GetYBoard();

    //    x *= 0.684f;
    //    y *= 0.684f;

    //    x += -2.4f;
    //    y += -2.4f;

    //    // end position
    //    Vector3 endPos = new Vector3(x, y, -1.0f);
    //    // calculate distance for move speed
    //    float distance = (endPos - startPos).magnitude;

    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        if (cp == null) yield break;
    //        t += Time.deltaTime / distance * 10f;
    //        cp.transform.position = Vector3.Lerp(startPos, endPos, t);
    //        yield return null;
    //    }
    //}
}
