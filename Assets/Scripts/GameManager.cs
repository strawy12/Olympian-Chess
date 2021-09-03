using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviourPunCallbacks
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

    private string player = "qwer";

    public bool gameOver = false;
    public bool isBacchrs = false;
    private bool usingSkill = false;
    private bool moving = true;
    private bool isStop = false;

    private Sprite[] backgrounds;
    [SerializeField]
    private SpriteRenderer back;

    [Multiline(10)]
    [SerializeField] string cheatInfo;
    [SerializeField] private List<MovePlate> movePlateList = new List<MovePlate>();
    [SerializeField] private Text currentPlayerText;

    [SerializeField] private Image matchPanel;

    WaitForSeconds delay2 = new WaitForSeconds(2);

    private void Awake()
    {
        player = NetworkManager.Inst.LoadDataFromJson<User>().player;
    }
    private void Start()
    {
        TurnManager.Instance.StartGame();
        SetCamera();
        pool = FindObjectOfType<PoolManager>();
        SetBackground();
        SoundManager.Instance.SetGameBGM(Random.Range(0, 2));
        SoundManager.Instance.StartSound();
    }
    private void Update()
    {
        InputCheatKey();
        Win();
        currentPlayerText.text = TurnManager.Instance.GetCurrentPlayer();
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            DestroyMovePlates();
        }
        //gameOver && click => reloading game
    }
    public string GetPlayer()
    {
        return player;
    }
    public void SetCamera()
    {
        Canvas cv = FindObjectOfType<Canvas>();

        if (player == "white")
        {
            blackCamera.enabled = false;
            whiteCamera.enabled = true;
            cv.worldCamera = whiteCamera;
            whiteCamera.gameObject.AddComponent<CameraResolution>();
            CardManager.Inst.ChangeCardArea(true);
        }

        else
        {
            whiteCamera.enabled = false;
            blackCamera.enabled = true;
            cv.worldCamera = blackCamera;
            blackCamera.gameObject.AddComponent<CameraResolution>();
            CardManager.Inst.ChangeCardArea(false);
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
                ChessManager.Inst.UpdateArr(white[i]);
                Destroy(white[i].gameObject);
            }
        }

        for (int i = 0; i < black.Length; i++)
        {
            if (black[i] == null) continue;
            if (black[i].gameObject.name == "black_pawn")
            {
                ChessManager.Inst.UpdateArr(black[i]);
                Destroy(black[i].gameObject);

            }
        }
    }

    public void LeavingRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public void DestroyMovePlates()
    {
        int cnt = movePlateList.Count;
        MovePlate movePlate;
        for (int i = 0; i < cnt; i++)
        {
            movePlate = movePlateList[0];
            RemoveMovePlateList(movePlate);
            Destroy(movePlate.gameObject);
        }
    }
    // Functions checking if the parameter is equal to current player

    public GameObject MovePlateSpawn(int matrixX, int matrixY, ChessBase cp)
    {
        if (SkillManager.Inst.CheckReturnMovePlate(matrixX, matrixY, "¼­Ç³") ||
            (SkillManager.Inst.CheckReturnMovePlate(matrixX, matrixY, "½Ã°£Á¤Áö")))
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
        AddMovePlateList(mpScript);
        return mp;
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY, ChessBase cp)
    {
        if (cp.IsAttackSpawn(matrixX, matrixY)) return;
        if (cp.NonAttackPiece(matrixX, matrixY)) return;

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
        //Debug.Log(matrixX+ ", " + matrixY);
        mpScript.SetCoords(matrixX, matrixY);
        AddMovePlateList(mpScript);
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
            if (player == "white")
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

            if (player != "white")
            {
                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] == null || playerWhite[i] == cp)
                        continue;

                    mp = MovePlateSpawn(playerWhite[i].GetXBoard(), playerWhite[i].GetYBoard(), cp);
                    mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color32(255, 0, 0, 255));
                }
            }
            else if (player != "black")
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

    public void DestroyNonemptyMovePlate()
    {
        List<MovePlate> nums = new List<MovePlate>();
        for (int i = 0; i < movePlateList.Count; i++)
        {
            if (movePlateList[i].Getreference() != null)
            {
                nums.Add(movePlateList[i]);
            }
        }

        for (int i = 0, cnt = nums.Count; i < cnt; i++)
        {
            RemoveMovePlateList(nums[0]);
            Destroy(nums[0].gameObject); ;
            nums.RemoveAt(0);
        }
    }

    private void SetBackground()
    {
        backgrounds = Resources.LoadAll<Sprite>("Images/ingameBackground");
        User user = NetworkManager.Inst.LoadDataFromJson<User>();
        back.sprite = backgrounds[user.backGround];

        if (player == "white")
        {
            back.gameObject.transform.rotation = Quaternion.identity;
        }

        else
        {
            back.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
        }
    }

    private void Win()
    {
        if (TurnManager.Instance.isLoading || gameOver) return;

        if (player == "white")
        {
            if (!ChessManager.Inst.CheckArr(false, "black_king"))
            {
                photonView.RPC("WinEffect", RpcTarget.OthersBuffered, false);
                WinEffect(true);
            }
        }

        else
        {
            if (!ChessManager.Inst.CheckArr(true, "white_king"))
            {
                photonView.RPC("WinEffect", RpcTarget.OthersBuffered, false);
                WinEffect(true);
            }
        }
    }
    [PunRPC]
    public void WinEffect(bool isTrue)
    {
        matchPanel.gameObject.SetActive(true);
        matchPanel.transform.DOScale(1, 0.4f);
        Text coinText = matchPanel.transform.GetChild(2).GetComponent<Text>();
        User user = NetworkManager.Inst.LoadDataFromJson<User>();

        if (isTrue && !gameOver)
        {
            SoundManager.Instance.WinOrLose(isTrue);
            matchPanel.transform.GetChild(0).gameObject.SetActive(true);
            coinText.transform.parent.gameObject.SetActive(true);
            coinText.text = "+200";
            user.gold += 200;
            NetworkManager.Inst.SaveDataToJson(user, true);
        }

        else if (!isTrue && !gameOver)
        {
            SoundManager.Instance.WinOrLose(isTrue);
            matchPanel.transform.GetChild(1).gameObject.SetActive(true);
            coinText.transform.parent.gameObject.SetActive(true);
            coinText.text = "+100";
            user.gold += 100;
            NetworkManager.Inst.SaveDataToJson(user, true);
        }

        gameOver = true;
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

    public void AddMovePlateList(MovePlate mp)
    {
        movePlateList.Add(mp);
    }

    // Function removing cp from dontClickPiece list
    public void RemoveMovePlateList(MovePlate mp)
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
        //CardManager.Inst.CardAlignment(true);
        //CardManager.Inst.CardAlignment(false);
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
            }

            else
            {
                attackings[i].PlusAttackCnt();

                if (attackings[i].GetAttackCnt() > 2)
                {
                    RemoveAttackings(attackings[i]);
                }
            }
        }
    }

    public List<ChessBase> GetAttackings()
    {
        return attackings;
    }
    public void AddAttackings(ChessBase cp)
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(cp.GetChessData(), false);
        photonView.RPC("AddAttackings", RpcTarget.AllBuffered, jsonData);
    }

    [PunRPC]
    public void AddAttackings(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        ChessBase chessBase = ChessManager.Inst.GetChessPiece(chessData);
        attackings.Add(chessBase);
        chessBase.SetIsAttacking(true);
    }
    public void RemoveAttackings(ChessBase cp)
    {
        string jsonData = NetworkManager.Inst.SaveDataToJson(cp.GetChessData(), false);
        photonView.RPC("RemoveAttackings", RpcTarget.AllBuffered, jsonData);
    }
    [PunRPC]
    public void RemoveAttackings(string jsonData)
    {
        ChessData chessData = NetworkManager.Inst.LoadDataFromJson<ChessData>(jsonData);
        ChessBase chessBase = ChessManager.Inst.GetChessPiece(chessData);
        attackings.Remove(chessBase);
        chessBase.ClearAttackCnt();
    }

    //public void StartEndTurn()
    //{
    //    TurnManager.Instance.EndTurn();

    //    if(attackings.Contains(cp))
    //    {
    //        attackings.Remove(cp);

    //        if(cp != null)
    //        {
    //            cp.attackCount = 0;
    //        }
    //    }
    //}
    public List<MovePlate> GetMovePlates()
    {
        return movePlateList;
    }
    #endregion
}