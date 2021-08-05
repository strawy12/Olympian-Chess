using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager inst;
    public static GameManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<GameManager>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }

    public GameObject chesspiece;
    [SerializeField] private GameObject chess = null;
    public PoolManager pool { get; private set; }
    // Positions and team for each chesspiece

    // create a two-dimensional array
    [SerializeField] private Chessman[,] position = new Chessman[8, 8];

    [SerializeField] private Chessman[] playerBlack = new Chessman[16];
    [SerializeField] private Chessman[] playerWhite = new Chessman[16];

    public List<Chessman> attackings;

    private string currentPlayer = "white";

    public bool gameOver = false;
    [Multiline(10)]
    [SerializeField] string cheatInfo;

    Chessman cm;

    WaitForSeconds delay2 = new WaitForSeconds(2);
    private void Awake()
    {
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        pool = FindObjectOfType<PoolManager>();
        StartGame();
        SetingGame();
    }
    private void SetingGame()
    {
        RotationBoard.camera = Camera.main;

        playerWhite = new Chessman[]
        {
           Creat("white_pawn", 1,1), Creat("white_pawn", 2,1), Creat("white_pawn", 3,1),
           Creat("white_pawn", 4,1), Creat("white_pawn", 5,1), Creat("white_pawn", 6,1),
           Creat("white_pawn", 7,1), Creat("white_pawn", 0,1), Creat("white_rook", 0,0),
           Creat("white_knight", 1,0), Creat("white_bishop", 2,0), Creat("white_queen", 3,0),
           Creat("white_king", 4,0), Creat("white_bishop", 5,0), Creat("white_knight", 6,0),
           Creat("white_rook", 7,0)
        };

        RotationBoard.playerWhite = playerWhite;

        playerBlack = new Chessman[]
        {
           Creat("black_pawn", 0,6), Creat("black_pawn", 1,6), Creat("black_pawn", 2,6),
           Creat("black_pawn", 3,6), Creat("black_pawn", 4,6), Creat("black_pawn", 5,6),
           Creat("black_pawn", 6,6), Creat("black_pawn", 7,6), Creat("black_rook", 0,7),
           Creat("black_knight", 1,7), Creat("black_bishop", 2,7), Creat("black_queen", 3,7),
           Creat("black_king", 4,7), Creat("black_bishop", 5,7),
           Creat("black_rook", 7,7), Creat("black_knight", 6,7)
        };

        RotationBoard.playerBlack = playerBlack;
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }
    public Chessman Creat(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        obj.transform.SetParent(chess.transform);
        Chessman cm = obj.GetComponent<Chessman>();

        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();

        return cm;
    }
    private void Update()
    {
        InputCheatKey();
    }
    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            DeletePawn();
        }
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game");
        }
    }
    public void CheckPosition()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Debug.Log(position[i, j]);
            }
        }
    }
    public void FasleIsMoving()
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
    public void SetPosition(Chessman obj)
    {
        if (obj == null) return;
        position[obj.GetXBoard(), obj.GetYBoard()] = obj;
    }
    public void SetChessPiecePosition(int x, int y, Chessman obj)
    {
        position[x, y] = obj;
    }
    public void DestroyMovePlates()
    {
        MovePlate[] movePlates = FindObjectsOfType<MovePlate>();

        for (int i = 0; i < movePlates.Length; i++) //�����÷���Ʈ ��� ���ǰ� ����
        {
            Destroy(movePlates[i].gameObject);
        }
    }
    public void CheckDeadSkillPiece(Chessman cp)
    {
        Skill[] sks = FindObjectsOfType<Skill>();

        for (int i = 0; i < sks.Length; i++)
        {
            if (sks[i].GetSelectPiece() == cp || sks[i].GetSelectPieceTo() == cp)
            {
                SkillManager.Inst.DeleteSkillList(sks[i]);
                cp.spriteRenderer.material.SetColor("_Color", new Color(0, 0, 0, 0));
                Destroy(sks[i].gameObject);
                return;
            }
        }
    }
    public void AllMovePlateSpawn(Chessman cm, bool isMine)
    {
        if (isMine)
        {
            if (currentPlayer == "white")
            {
                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] == null || playerWhite[i] == cm)
                        continue;
                    playerWhite[i].MovePlateSpawn(playerWhite[i].GetXBoard(), playerWhite[i].GetYBoard());
                }
            }
            else
            {
                for (int i = 0; i < playerBlack.Length; i++)
                {
                    if (playerBlack[i] == null || playerBlack[i] == cm)
                        continue;
                    playerBlack[i].MovePlateSpawn(playerBlack[i].GetXBoard(), playerBlack[i].GetYBoard());
                }
            }
        }
        else
        {

            if (currentPlayer != "white")
            {
                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] == null || playerWhite[i] == cm)
                        continue;
                    cm.SetMovePlateColor(new Color32(255, 0, 0, 255));
                    playerWhite[i].MovePlateSpawn(playerWhite[i].GetXBoard(), playerWhite[i].GetYBoard());
                    playerWhite[i].SetMovePlateColor(new Color32(255, 255, 36, 255));
                }
            }
            else if (currentPlayer != "black")
            {
                for (int i = 0; i < playerBlack.Length; i++)
                {
                    if (playerBlack[i] == null || playerBlack[i] == cm)
                        continue;
                    playerBlack[i].SetMovePlateColor(new Color32(255, 0, 0, 255));
                    playerBlack[i].MovePlateSpawn(playerBlack[i].GetXBoard(), playerBlack[i].GetYBoard());
                    playerBlack[i].SetMovePlateColor(new Color32(255, 255, 36, 255));
                }
            }
        }

    }
    public int CheckNull(bool isXY, bool isPlma, int pos)
    {
        int cnt = 0;
        if (isXY && isPlma)
        {
            for (int i = 7; i >= 0; i--)
            {
                if (GetPosition(i, pos) == null)
                {
                    cnt = i;
                    return cnt;
                }
            }
        }

        else if (isXY && !isPlma)
        {
            for (int i = 0; i < 8; i++)
            {
                if (GameManager.Inst.GetPosition(i, pos) == null)
                {
                    cnt = i;
                    return cnt;
                }

            }
        }

        else if (!isXY && isPlma)
        {
            for (int i = 7; i >= 0; i--)
            {
                if (GameManager.Inst.GetPosition(pos, i) == null)
                {
                    cnt = i;
                    return cnt;
                }
            }
        }

        else if (!isXY && !isPlma)
        {
            for (int i = 0; i < 8; i++)
            {
                if (GameManager.Inst.GetPosition(pos, i) == null)
                {
                    cnt = i;
                    return cnt;
                }
            }
        }
        return 0;
    }
    private void DeleteRook()
    {
        for (int i = 0; i < playerBlack.Length; i++)
        {
            if (playerBlack[i] == null) continue;
            if (playerBlack[i].gameObject.name == "black_rook")
            {
                Destroy(playerBlack[i].gameObject);
                UpdateArr(playerBlack[i]);
            }
        }
    }
    private void DeletePawn()
    {
        for (int i = 0; i < playerWhite.Length; i++)
        {
            if (playerWhite[i] == null) continue;
            if (playerWhite[i].gameObject.name == "white_pawn")
            {
                Destroy(playerWhite[i].gameObject);
                UpdateArr(playerWhite[i]);
            }
            if (playerBlack[i] == null) continue;
            if (playerBlack[i].gameObject.name == "black_pawn")
            {
                Destroy(playerBlack[i].gameObject);
                UpdateArr(playerBlack[i]);
            }
        }
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
    public void UpdateArr(Chessman chessPiece)
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
        RotationBoard.playerWhite = playerWhite;
        RotationBoard.playerBlack = playerBlack;

    }
    public void AddArr(Chessman chessPiece)
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

        RotationBoard.playerWhite = playerWhite;
        RotationBoard.playerBlack = playerBlack;
    }
    public void StartGame()
    {
        TurnManager.Inst.StartGame();
    }

    public IEnumerator GameOver(bool isMyWin)
    {
        yield return delay2;

        TurnManager.Inst.isLoading = true;

        TurnManager.Inst.isLoading = true;
        //endTurnButton.SetActive(false);
        //resultPanal.Show(isMyWin ? "�¸�" : "�й�");
        //cameraEffect.SetGrayScale(true);
    }
    #region

    // Set empty position for set position
    public void SetPositionEmpty(int x, int y)
    {
        position[x, y] = null;
    }
    //return positions
    public Chessman GetPosition(int x, int y)
    {
        return position[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= position.GetLength(0) || y >= position.GetLength(1)) return false;
        return true;
    }
    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (gameOver) return;

        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }

        StartCoroutine(CameraDelayRotate());
    }
    private IEnumerator CameraDelayRotate()
    {
        yield return new WaitForSeconds(1f);
        if (gameOver) yield break;
        RotationBoard.Rotate();
        CardManager.Inst.ChangeCard(GetCurrentPlayer() == "white");
        CardManager.Inst.CardAlignment(true);
        CardManager.Inst.CardAlignment(false);
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        CardManager.Inst.DestroyCards();
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
    public void GameOver()
    {
        gameOver = true;
    }

    public void ResetIsMoving()
    {
        for (int i = 0; i < playerBlack.Length; i++)
        {
            playerBlack[i].isMoving = false;
        }

        for (int i = 0; i < playerWhite.Length; i++)
        {
            playerBlack[i].isMoving = false;
        }
    }
    #endregion
}