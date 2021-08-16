using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //GameManager Singleton
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

    [SerializeField] ECardState eCardState;
    [SerializeField] GameObject movePlate;
    [SerializeField] Camera blackCamera;
    [SerializeField] Camera whiteCamera;

    enum ECardState { Moving, Skill, MovingAndSkill }

    //List including attacking chesspiece
    private List<ChessBase> attackings = new List<ChessBase>();



    public bool gameOver = false;
    public bool isBacchrs = false;
    private bool usingSkill = false;
    private bool moving = true;
    private bool isStop = false;


    [Multiline(10)]
    [SerializeField] string cheatInfo;
    [SerializeField] private List<GameObject> movePlateList = new List<GameObject>();
    [SerializeField] private Text currentPlayerText;
    [SerializeField] private Text networkPlayerText;

    WaitForSeconds delay2 = new WaitForSeconds(2);
    private void Awake()
    {
        // remove another gamemanager object if it exists
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        pool = FindObjectOfType<PoolManager>();
        TurnManager.Instance.StartGame();
        CardManager.Inst.CardShare();
        SetCamera();
    }
    private void Update()
    {
        InputCheatKey();
        
    }
    // Functions including cheatkey
    void InputCheatKey()
    {
        //esc => quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //A => All Moveplate Spawn
        if (Input.GetKeyDown(KeyCode.A))
        {
            RealAllMovePlateSpawn();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            DeletePawn();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            DestroyMovePlates();
        }
        //gameOver && click => reloading game
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game");
        }
    }

    private void SetCamera()
    {
        Canvas cv = FindObjectOfType<Canvas>();
        if (NetworkManager.Inst.GetPlayer() == "white")
        {
            blackCamera.enabled = false;
            whiteCamera.enabled = true;
            cv.worldCamera = whiteCamera;
        }
        else
        {
            whiteCamera.enabled = false;
            blackCamera.enabled = true;
            cv.worldCamera = blackCamera;
        }
        
        
    }
    private void DeletePawn()
    {
        ChessBase[] white = ChessManager.Inst.GetPlayerWhite();
        ChessBase[] black = ChessManager.Inst.GetPlayerBlack();

        for (int i = 0; i < white.Length; i++)
        {
            if (white[i] == null) continue;
            if (white[i].gameObject.name == "white_pawn")
            {
                Destroy(white[i].gameObject);
                ChessManager.Inst.UpdateArr(white[i]);
            }
        }

        for(int i = 0; i < black.Length; i++)
        { 
            if (black[i] == null) continue;
            if (black[i].gameObject.name == "black_pawn")
            {
                Destroy(black[i].gameObject);
                ChessManager.Inst.UpdateArr(black[i]);
            }
        }
    }
    public void DestroyMovePlates()
    {
        int cnt = movePlateList.Count;
        for (int i = 0; i < cnt; i++)
        {
            Destroy(movePlateList[0]);
            RemoveMovePlateList(movePlateList[0]);
        }
    }
    // Functions checking if the parameter is equal to current player

    public GameObject MovePlateSpawn(int matrixX, int matrixY, ChessBase cp)
    {
        if (SkillManager.Inst.CheckReturnMovePlate(matrixX, matrixY, "¼­Ç³"))
            return null;

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
        AddMovePlateList(mp);
        return mp;
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY, ChessBase cp)
    {
        if (cp.IsAttackSpawn(matrixX, matrixY)) return;
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
        AddMovePlateList(mp);


    }


    // Function spawning move plates on each non-empty space
    // that exist parameter value(black or white) color
    public void AllMovePlateSpawn(ChessBase cp, bool isMine)
    {
        GameObject mp;
        ChessBase[] playerWhite = ChessManager.Inst.GetPlayerWhite();
        ChessBase[] playerBlack = ChessManager.Inst.GetPlayerBlack();
        if (isMine)
        {
            if (NetworkManager.Inst.GetPlayer() == "white")
            {
                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] == null || playerWhite[i] == cp)
                        continue;
                    MovePlateSpawn(playerWhite[i].GetXBoard(), playerWhite[i].GetYBoard(), cp);
                }
            }
            else
            {
                for (int i = 0; i < playerBlack.Length; i++)
                {
                    if (playerBlack[i] == null || playerBlack[i] == cp)
                        continue;
                    MovePlateSpawn(playerBlack[i].GetXBoard(), playerBlack[i].GetYBoard(), cp);
                }
            }
        }
        else
        {

            if (NetworkManager.Inst.GetPlayer() != "white")
            {
                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] == null || playerWhite[i] == cp)
                        continue;

                    mp = MovePlateSpawn(playerWhite[i].GetXBoard(), playerWhite[i].GetYBoard(), cp);
                    mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(255, 0, 0, 255));
                }
            }
            else if (NetworkManager.Inst.GetPlayer() != "black")
            {
                for (int i = 0; i < playerBlack.Length; i++)
                {
                    if (playerBlack[i] == null || playerBlack[i] == cp)
                        continue;

                    mp = MovePlateSpawn(playerBlack[i].GetXBoard(), playerBlack[i].GetYBoard(), cp);
                    mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(255, 0, 0, 255));

                }
            }
        }
    }

    public void RealAllMovePlateSpawn()
    {
        ChessBase cp;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                cp = ChessManager.Inst.GetPosition(i, j);
                MovePlateSpawn(i, j, cp);
            }
        }

    }
    // Game Over Coroutine
    public IEnumerator GameOver(bool isMyWin)
    {
        yield return delay2;

        TurnManager.Instance.isLoading = true;
        //endTurnButton.SetActive(false);
        //resultPanal.Show(isMyWin ? "½Â¸®" : "ÆÐ¹è");
        //cameraEffect.SetGrayScale(true);
    }
    #region


    public void SetIsStop(bool isStop)
    {
        this.isStop = isStop;
    }

    public bool GetIsStop()
    {
        return isStop;
    }

    public void AddMovePlateList(GameObject mp)
    {
        movePlateList.Add(mp);
    }

    // Function removing cp from dontClickPiece list
    public void RemoveMovePlateList(GameObject mp)
    {
        movePlateList.Remove(mp);

    }

    // Function returning gameOver
    public bool IsGameOver()
    {
        return gameOver;
    }

    public bool GetUsingSkill()
    {
        return usingSkill;
    }
    public bool GetMoving()
    {
        return moving;
    }

    public void SetUsingSkill(bool usingSkill)
    {
        this.usingSkill = usingSkill;
    }

    public void SetMoving(bool moving)
    {
        this.moving = moving;
    }

    // Coroutine rotating main camera
    private IEnumerator CameraDelayRotate()
    {
        yield return new WaitForSeconds(1f);
        if (gameOver) yield break;
        //RotationBoard.Rotate();
        //CardManager.Inst.ChangeCard(GetCurrentPlayer() == "white");
        CardManager.Inst.CardAlignment(true);
        CardManager.Inst.CardAlignment(false);
    }
    // Function setting winner
    public void Winner(string playerWinner)
    {
        gameOver = true;
        CardManager.Inst.DestroyCards();
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
    // Function setting gameOver to true
    public void GameOver()
    {
        gameOver = true;
    }

    public void PlusAttackCnt()
    {
        for (int i = 0; i < attackings.Count; i++)
        {
            if (attackings[i] == null)
            {
                attackings.RemoveAt(i);
                continue;
            }

            else
            {
                attackings[i].attackCount++;

                if (attackings[i].attackCount > 2)
                {
                    attackings.RemoveAt(i);
                }
            }
        }
    }

    public List<ChessBase> GetAttackings()
    {
        return attackings;
    }
    public void AddAttackings(ChessBase chessBase)
    {
        attackings.Add(chessBase);
        chessBase.isAttacking = true;
    }
    public void RemoveAttackings(ChessBase chessBase)
    {
        attackings.Remove(chessBase);
        chessBase.attackCount = 0;
    }

    public void StartEndTurn()
    {
        TurnManager.Instance.EndTurn();
    }

    #endregion
}