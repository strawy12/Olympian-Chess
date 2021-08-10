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

    public GameObject white_Bishop, white_King, white_Knight, white_Pawn, white_Queen, white_Rook;
    public GameObject black_Bishop, black_King, black_Knight, black_Pawn, black_Queen, black_Rook;

    [SerializeField] private ChessBase[,] position = new ChessBase[8, 8];

    [SerializeField] private ChessBase[] playerBlack = new ChessBase[16];
    [SerializeField] private ChessBase[] playerWhite = new ChessBase[16];

    [SerializeField] private GameObject cccccp;

    [SerializeField] private GameObject movePlate;
    void Start()
    {
        SettingGame();
    }

    private void SettingGame()
    {
        playerWhite = new ChessBase[]
       {
           Creat(white_Pawn, 1,1), Creat(white_Pawn, 2,1), Creat(white_Pawn, 3,1),
           Creat(white_Pawn, 4,1), Creat(white_Pawn, 5,1), Creat(white_Pawn, 6,1),
           Creat(white_Pawn, 7,1), Creat(white_Pawn, 0,1), Creat(white_Rook, 0,0),
           Creat(white_Knight, 1,0), Creat(white_Bishop, 2,0), Creat(white_Queen, 3,0),
           Creat(white_King, 4,0), Creat(white_Bishop, 5,0), Creat(white_Knight, 6,0),
           Creat(white_Rook, 7,0)
       };

        playerBlack = new ChessBase[]
        {
           Creat(black_Pawn, 0,6), Creat(black_Pawn, 1,6), Creat(black_Pawn, 2,6),
           Creat(black_Pawn, 3,6), Creat(black_Pawn, 4,6), Creat(black_Pawn, 5,6),
           Creat(black_Pawn, 6,6), Creat(black_Pawn, 7,6), Creat(black_Rook, 0,7),
           Creat(black_Knight, 1,7), Creat(black_Bishop, 2,7), Creat(black_Queen, 3,7),
           Creat(black_King, 4,7), Creat(black_Bishop, 5,7),
           Creat(black_Rook, 7,7), Creat(black_Knight, 6,7)
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

    public GameObject MovePlateSpawn(ChessBase cp, int matrixX, int matrixY)
    {
        //if (CheckReturnMovePlate(matrixX, matrixY, "서풍"))
        //    return;

        float x = matrixX;
        float y = matrixY;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.Setreference(cp);
        mpScript.SetCoords(matrixX, matrixY);
        return mp;
    }

    public void MovePlateAttackSpawn(ChessBase cp, int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.Setreference(cp);
        mpScript.SetCoords(matrixX, matrixY);

    }
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++) //무브플레이트 모두 살피고 제거
        {
            Destroy(movePlates[i]);
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

    public void AttackChessPiece(int matrixX, int matrixY)
    {
        ChessBase cp = GetPosition(matrixX, matrixY);
        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        UpdateArr(cp);
        Destroy(cp.gameObject);
    }

    public void MoveChessPiece(ChessBase cp, int matrixX, int matrixY)
    {
        SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.PlusMoveCnt();
        SetPosition(cp);
        StartCoroutine(SetCoordsAnimation(cp));
        TurnManager.Instance.ButtonColor();
        DestroyMovePlates();
    }
    public IEnumerator SetCoordsAnimation(ChessBase cp)
    {
        
        // start position if (this == null) gameObject.SetActive(false);
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
            t += Time.deltaTime / distance * 10f;
            cp.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

    }
}
