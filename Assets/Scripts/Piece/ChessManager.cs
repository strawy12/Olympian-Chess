using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessManager : MonoBehaviour
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

    [SerializeField] private GameObject[] white; // { white_Pawn, white_Bishop, white_Knight, white_Rook, white_Queen  ,white_King }
    [SerializeField] private GameObject[] black; // black_Bishop, black_King, black_Knight, black_Pawn, black_Queen, black_Rook;

    [SerializeField] private ChessBase[,] position = new ChessBase[8, 8];

    [SerializeField] private ChessBase[] playerBlack = new ChessBase[16];
    [SerializeField] private ChessBase[] playerWhite = new ChessBase[16];

    [SerializeField] private GameObject cccccp;

    void Start()
    {
        SettingGame();
    }

    private void SettingGame()
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

        playerBlack = new ChessBase[]
        {
           Creat(black[pawn], 0,6), Creat(black[pawn], 1,6), Creat(black[pawn], 2,6),
           Creat(black[pawn], 3,6), Creat(black[pawn], 4,6), Creat(black[pawn], 5,6),
           Creat(black[pawn], 6,6), Creat(black[pawn], 7,6), Creat(black[rook], 0,7),
           Creat(black[knight], 1,7), Creat(black[bishop], 2,7), Creat(black[queen], 3,7),
           Creat(black[king], 4,7), Creat(black[bishop], 5,7), Creat(black[rook], 7,7),
           Creat(black[knight], 6,7)
        };

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }
    public ChessBase Creat(GameObject chessPiece, int x, int y)
    {
        GameObject obj = Instantiate(chessPiece, new Vector3(0, 0, -1), Quaternion.identity);
        obj.name = chessPiece.name;

        obj.transform.SetParent(cccccp.transform);

        ChessBase cb = obj.GetComponent<ChessBase>();

        cb.SetXBoard(x);
        cb.SetYBoard(y);
        SetCoords(obj, x, y);

        return cb;
    }

    public void SetCoords(GameObject obj, int xBoard, int yBoard)
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        // Aligns according the board
        obj.transform.position = new Vector3(x, y, -1.0f);
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

            else if (cb.player != cp.player)
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
            playerWhite[i].isMoving = false;
            if (playerBlack[i] == null)
                continue;
            playerBlack[i].isMoving = false;
        }
    }

    public void UpdateArr(ChessBase chessPiece)
    {
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
        if (chessPiece.player == "white")
        {
            for (int i = 0; i < playerWhite.Length; i++)
            {
                if (playerWhite[i] == null)
                    playerWhite[i] = chessPiece;
            }
        }
        else
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {
                if (playerBlack[i] == null)
                    playerBlack[i] = chessPiece;
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
    public void SetPositionEmpty(int x, int y)
    {
        position[x, y] = null;
    }
    //return positions
    public ChessBase GetPosition(int x, int y)
    {
        return position[x, y];
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
        position[x, y] = obj;
    }

    public void SetPosition(ChessBase obj)
    {
        if (obj == null) return;
        position[obj.GetXBoard(), obj.GetYBoard()] = obj;
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
        UpdateArr(cp);
        GameManager.Inst.RemoveAttackings(cp);
        Destroy(cp.gameObject);

        if (mp.Getreference().isAttacking)
        {
            GameManager.Inst.RemoveAttackings(mp.Getreference());
        }

        GameManager.Inst.AddAttackings(mp.Getreference());
        SuperSkillManager.Inst.CheckSuperSkill();
    }

    public void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        SetPosition(cp);
        cp.isMoving = true;
        StartCoroutine(SetCoordsAnimation(cp));
        TurnManager.Instance.ButtonColor();
        GameManager.Inst.DestroyMovePlates();
    }
    public IEnumerator SetCoordsAnimation(ChessBase cp)
    {
        Vector3 startPos = cp.transform.position;

        float x = cp.GetXBoard();
        float y = cp.GetYBoard();

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        // end position
        Vector3 endPos = new Vector3(x, y, -1.0f);
        // calculate distance for move speed
        float distance = (endPos - startPos).magnitude;

        float t = 0f;

        while (t < 1f)
        {
            if (cp == null) yield break;
            t += Time.deltaTime / distance * 10f;
            cp.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
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
        List<GameObject> movePlates = new List<GameObject>();

        if (player == "black")
        {
            for (int i = 0; i < playerWhite.Length; i++)
            {
                if (playerWhite[i] == null) continue;

                playerWhite[i].MovePlate();

                movePlates = GameManager.Inst.GetMovePlates();

                for (int j = 0; j < movePlates.Count; j++)
                {
                    if (movePlates[j].GetComponent<MovePlate>().GetChessPiece() == null) continue;

                    if (movePlates[j].GetComponent<MovePlate>().GetChessPiece().name == "black_king")
                        return true;
                }

                GameManager.Inst.DestroyMovePlates();
            }
        }

        else
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {
                if (playerBlack[i] == null) continue;

                playerBlack[i].MovePlate();

                movePlates = GameManager.Inst.GetMovePlates();

                for (int j = 0; j < movePlates.Count; j++)
                {
                    if (movePlates[j].GetComponent<MovePlate>().GetChessPiece() == null) continue;

                    if (movePlates[j].GetComponent<MovePlate>().GetChessPiece().name == "white_king")
                        return true;
                }

                GameManager.Inst.DestroyMovePlates();
            }
        }

        GameManager.Inst.DestroyMovePlates();
        return false;
    }

    public bool KingAndRook(string player, bool isKing)
    {
        List<ChessBase> cblist = new List<ChessBase>();

        if (player == "white" && isKing)
        {
            if (PositionOnBoard(5, 0) && PositionOnBoard(6, 0))
                return true;
        }

        else if (player == "white" && !isKing)
        {
            if (PositionOnBoard(1, 0) && PositionOnBoard(2, 0) && PositionOnBoard(3, 0))
                return true;
        }

        else if (player == "black" && isKing)
        {
            if (PositionOnBoard(5, 7) && PositionOnBoard(6, 7))
                return true;
        }

        else if (player == "black" && !isKing)
        {
            if (PositionOnBoard(1, 7) && PositionOnBoard(2, 7) && PositionOnBoard(3, 7))
                return true;
        }

        return false;
    }

    public bool CheckMovePlate(string player, int x, int y)
    {
        List<GameObject> movePlates = new List<GameObject>();

        //if (player == "black")
        //{
        //    for (int i = 0; i < playerWhite.Length; i++)
        //    {
        //        if (playerWhite[i] == null) continue;

        //        playerWhite[i].MovePlate();

        //        movePlates = GameManager.Inst.GetMovePlates();

        //        for (int j = 0; j < movePlates.Count; j++)
        //        {
        //            if (movePlates[j].GetComponent<MovePlate>().GetChessPiece() == null) continue;

        //            if (movePlates[j].GetComponent<MovePlate>().GetPosX() == x && movePlates[j].GetComponent<MovePlate>().GetPosY() == y)
        //                return true;
        //        }

        //        GameManager.Inst.DestroyMovePlates();
        //    }
        //}

        if(player == "white")
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {
                if (playerBlack[i] == null) continue;

                playerBlack[i].MovePlate();

                movePlates = GameManager.Inst.GetMovePlates();

                for (int j = 0; j < movePlates.Count; j++)
                {
                    if (movePlates[j].GetComponent<MovePlate>().GetChessPiece() == null) continue;

                    if (movePlates[j].GetComponent<MovePlate>().GetPosX() == x && movePlates[j].GetComponent<MovePlate>().GetPosY() == y)
                        return true;
                }

            }

            GameManager.Inst.DestroyMovePlates();

        }

        GameManager.Inst.DestroyMovePlates();
        return false;

    }

    public bool Castling(string player, int moveCnt)
    {
        if (player == "white" && moveCnt == 0)
        {
            Debug.Log("플레이어 화이트고 움직이지 않았음");

            if (KingAndRook("white", true) || KingAndRook("white", false))
            {
                Debug.Log("킹과 룩 사이에 장애물 없음");

                if (!CheckMate("white"))
                {
                    Debug.Log("왕이 체크메이트가 아님");

                    return true;
                }
            }

            else if (player == "black" && moveCnt == 0)
            {
                if (KingAndRook("black", true) || KingAndRook("black", true))
                {
                    if (!CheckMate("white"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}