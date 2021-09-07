using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviourPunCallbacks
{
    #region SingleTon

    private static TurnManager _instance;

    public static TurnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TurnManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("TurnManager").AddComponent<TurnManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region 변수정의
    [Header("턴 정하기")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다.")] EturnMode eTurnMode;
    [Header("빠른모드 설정")]
    [SerializeField] [Tooltip("카드 배분이 매우 빨라집니다.")] bool fastMode;
    [Header("시작할때 카드 개수")]
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount;

    [Header("상대와 나의 버튼")]
    [SerializeField] Button button; // player's button

    [Header("활성화,비활성화 버튼 이미지")]
    [SerializeField] Sprite buttonActive; // Activated button image
    [SerializeField] Sprite buttonInactive; // Disabled button image

    [Header("상대와 나의 버튼 위치")]
    [SerializeField] Transform posUp; // Opponent's button position
    [SerializeField] Transform posDown; // button position of the current player
    [SerializeField] private GameObject loadingDisplay;
    public GameObject loadingObj { get { return loadingDisplay; } }
    [SerializeField] GameObject uiObject;

    private bool isActive = false;
    private bool isTimeBtnClicked = false;
    private bool isBreak = false;

    private Coroutine turnTimeCoroutine = null;

    [SerializeField] private string currentPlayer = "white";

    [SerializeField] private Image turnPanel;
    [SerializeField] private Slider turnTimeSlider;

    [Header("속성")]
    public bool isLoading;
    #endregion

    enum EturnMode { Random, White, Black }

    // delay
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(2f);

    public static event Action<bool> OnTurnStarted;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
        }
    }
    public void StartGame()
    {
        loadingDisplay.SetActive(true);
        uiObject.SetActive(false);
        SuperSkillManager.Inst.SetActive(false);

        if (GameManager.Inst.GetPlayer() == "black")
        {
            loadingDisplay.transform.Rotate(0f, 0f, 180f);
        }

        StartCoroutine(TurnSetting());
    }

    // Function calling next turn

    [PunRPC]
    public void NextTurn()
    {
        if (GameManager.Inst.gameOver) return;

        if (currentPlayer == "white")
        {
            currentPlayer = "black";

            if (GameManager.Inst.GetPlayer() == currentPlayer)
                StartCoroutine(TurnPanel());
        }
        else
        {
            currentPlayer = "white";

            if (GameManager.Inst.GetPlayer() == currentPlayer)
                StartCoroutine(TurnPanel());
        }

        SuperSkillManager.Inst.CheckSuperSkill();
        //StartCoroutine(CameraDelayRotate());
    }

    IEnumerator TurnSetting()
    {
        // 5 second delay in fast mode
        int i = Random.Range(0, 2);
        // normal turn mode
        switch (eTurnMode)
        {
            // In Random mode, choose a turn at random with your opponent
            case EturnMode.Random:
                if (i == 1) currentPlayer = "white";
                else currentPlayer = "black";
                break;

            // If it's my turn, activate my turn
            case EturnMode.White:
                currentPlayer = "white";
                break;

            // If it's your opponent's turn, activate your opponent's turn.
            case EturnMode.Black:
                currentPlayer = "black";
                break;
        }
        isLoading = true;
        ChessManager.Inst.SettingGame();
        yield return new WaitForSeconds(3f);
        loadingDisplay.SetActive(false);
        SuperSkillManager.Inst.SetActive(true);
        uiObject.SetActive(true);
        CardManager.Inst.CardShare();
        yield return new WaitForSeconds(2f);

        isLoading = false;
    }

    public bool GetCurrentPlayerTF()
    {
        return currentPlayer == GameManager.Inst.GetPlayer();
    }

    public bool CheckPlayer(string player)
    {
        return currentPlayer == player;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    // Start a new turn & Win, lose decision
    public IEnumerator StartTurnCo()
    {
        // turn division
        isLoading = true;

        yield return delay07;

        isLoading = false;

        OnTurnStarted?.Invoke(GetCurrentPlayerTF());
        #region 위 문법이랑 같은 뜻
        /*if(OnTurnStarted!=null)
        {
            OnTurnStarted(myTurn);
        } */
        #endregion
    }

    public void ClickTurnTimeBtn()
    {
        Debug.Log("sdsdfsdfs");
        if (GetCurrentPlayerTF()) return;
        Debug.Log("sdf");
        if (isTimeBtnClicked) return;
        Debug.Log("dfdfdf");
        isTimeBtnClicked = true;
        photonView.RPC("TurnTime", RpcTarget.AllBuffered);
    }

    private void WinOrLose()
    {
        // When these conditions are met, the search is the winner and the game is over.
        if (!ChessManager.Inst.CheckArr(true, "white_king"))
        {
            GameManager.Inst.Winner("black");
            GameManager.Inst.GameOver();
        }
        // When these conditions are met, White is the winner and the game is over.
        else if (!ChessManager.Inst.CheckArr(false, "black_king"))
        {
            GameManager.Inst.Winner("white");
            GameManager.Inst.GameOver();
        }
    }

    // Re Active
    public bool GetIsActive()
    {
        return isActive;
    }

    // Setting Active
    public void SetIsActive(bool isActive)
    {
        this.isActive = isActive;
    }

    // Change my button or someone else's button to the active button image depending on context
    public void ButtonActive()
    {
        button.image.sprite = buttonActive;

        isActive = true;
    }

    // Change the other party and my button to a disabled button image
    public void ButtonInactive()
    {
        button.image.sprite = buttonInactive;

        isActive = false;
    }

    // There are so many functions referenced elsewhere here that it is impossible to interpret
    public void EndTurn()
    {
        if (!isActive) return;
        if (!GetCurrentPlayerTF()) return;
        if (GameManager.Inst.gameOver) return;

        isBreak = true;
        if (CardManager.Inst.GetSelectCard() != null)
        {
            CardManager.Inst.DestroyCard(CardManager.Inst.GetSelectCard());
        }
        GameManager.Inst.DestroyMovePlates();
        ChessManager.Inst.FalseIsMoving();
        ChessManager.Inst.SetIsMoving(false);
        //ChangeButtonTransform();
        ButtonInactive();

        CardManager.Inst.ChangeIsUse(false);
        SkillManager.Inst.SkillListCntPlus();
        photonView.RPC("NextTurn", RpcTarget.All);
        isTimeBtnClicked = false;
        SuperSkillManager.Inst.SuperListCntPlus();
        GameManager.Inst.PlusAttackCnt();

        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetIsStop(false);

        StartCoroutine(StartTurnCo());
        isBreak = false;   
        //WinOrLose();
    }

    private IEnumerator TurnPanel()
    {
        turnPanel.transform.DOScale(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        turnPanel.transform.DOScale(0f, 0.1f);
    }

    [PunRPC]
    private IEnumerator TurnTime()
    {
        Debug.Log("sdfsdfsdfsdf");

        for (int i = 1; i < 11; i++)
        {
            if(isBreak)
            {
                Debug.Log("sdfsdfsdfsdf");
                yield break;
            }
            turnTimeSlider.value = i;
            Debug.Log(turnTimeSlider.value);
            yield return new WaitForSeconds(1f);
        }

        if(GetCurrentPlayerTF())
        {
            Debug.Log("sdf");
            TurnTimeFuc();
        }
    }

    private void TurnTimeFuc()
    {
        ButtonInactive();
        EndTurn();
    }
}