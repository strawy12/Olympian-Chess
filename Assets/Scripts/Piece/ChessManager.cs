using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ChessManager : MonoBehaviour
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

    [SerializeField] private GameObject[] white; // { white_Pawn, white_Bishop, white_Knight, white_Rook, white_Queen  ,white_King }
    [SerializeField] private GameObject[] black; // black_Bishop, black_King, black_Knight, black_Pawn, black_Queen, black_Rook;

    [SerializeField] private ChessBase[,] position = new ChessBase[8, 8];

    [SerializeField] private ChessBase[] playerBlack = new ChessBase[16];
    [SerializeField] private ChessBase[] playerWhite = new ChessBase[16];

    [SerializeField] private GameObject cccccp;

    [SerializeField] private WaitForSeconds delay = new WaitForSeconds(0.5f);

    void Start()
    {
        SettingGame();
    }



    private void SettingGame()
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

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
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


    //private IEnumerator SpawnWhiteChessPiece()
    //{
    //    ChessBase cb;
    //    int i = 0;
    //    //cb = Creat(white[pawn], 1, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[pawn], 2, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[pawn], 3, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[pawn], 4, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[pawn], 5, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[pawn], 6, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[pawn], 7, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[pawn], 0, 1);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    cb = Creat(white[rook], 0, 0);
    //    playerWhite.SetValue(cb, i++);
    //    yield return delay;
    //    //cb = Creat(white[knight], 1, 0);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[bishop], 2, 0);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    cb = Creat(white[queen], 3, 0);
    //    playerWhite.SetValue(cb, i++);
    //    yield return delay;
    //    cb = Creat(white[king], 4, 0);
    //    playerWhite.SetValue(cb, i++);
    //    yield return delay;
    //    //cb = Creat(white[bishop], 5, 0);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(white[knight], 6, 0);
    //    //playerWhite.SetValue(cb, i++);
    //    //yield return delay;
    //    cb = Creat(white[rook], 7, 0);
    //    playerWhite.SetValue(cb, i++);
    //}

    //private IEnumerator SpawnBlackChessPiece()
    //{
    //    ChessBase cb;
    //    int i = 0;

    //    //cb = Creat(black[pawn], 0, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[pawn], 1, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[pawn], 2, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[pawn], 3, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[pawn], 4, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[pawn], 5, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[pawn], 6, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[pawn], 7, 6);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    cb = Creat(black[rook], 0, 7);
    //    playerBlack.SetValue(cb, i++);
    //    yield return delay;
    //    //cb = Creat(black[knight], 1, 7);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[bishop], 2, 7);
    //    //playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    cb = Creat(black[queen], 3, 7);
    //    playerBlack.SetValue(cb, i++);
    //    yield return delay;
    //    cb = Creat(black[king], 4, 7);
    //    playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[bishop], 5, 7);
    //    //playerBlack.SetValue(cb, i++);
    //    yield return delay;
    //    cb = Creat(black[rook], 7, 7);
    //    playerBlack.SetValue(cb, i++);
    //    //yield return delay;
    //    //cb = Creat(black[knight], 6, 7);
    //    //playerBlack.SetValue(cb, i++);
    //}

    public ChessBase Creat(GameObject chessPiece, int x, int y)
    {
        GameObject obj = NetworkManager.Inst.SpawnChessPiece(chessPiece);
        ChessBase cb = obj.GetComponent<ChessBase>();

        obj.transform.SetParent(cccccp.transform);

        cb.SetXBoard(x);
        cb.SetYBoard(y);
        SetCoords(obj, x, y);

        return cb;
    }

    public void SetCoords(GameObject obj, int xBoard, int yBoard)
    {
        Debug.Log(obj.name);
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

    public void FalsIsMoving()
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
        Destroy(cp.gameObject);

        if (mp.Getreference().isAttacking)
        {
            GameManager.Inst.RemoveAttackings(mp.Getreference());
        }

        GameManager.Inst.AddAttackings(mp.Getreference());
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
        TurnManager.Instance.ButtonActive();
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
}
