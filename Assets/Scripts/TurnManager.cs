using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
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
                if(_instance==null)
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
    [SerializeField] Button buttonWhite; // player's button
    [SerializeField] Button buttonBlack; // opponent's button

    [Header("활성화,비활성화 버튼 이미지")]
    [SerializeField] Sprite buttonActive; // Activated button image
    [SerializeField] Sprite buttonInactive; // Disabled button image

    [Header("상대와 나의 버튼 위치")]
    [SerializeField] Transform posUp; // Opponent's button position
    [SerializeField] Transform posDown; // button position of the current player
    [SerializeField] GameObject loadingDisplay; 

    private bool isActive = false;

    [SerializeField]private string currentPlayer = "white";

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
        }
    }
    public void StartGame()
    {
        loadingDisplay.SetActive(true);
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
        }
        else
        {
            currentPlayer = "white";
        }

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
        CardManager.Inst.CardShare();
        yield return new WaitForSeconds(2f);
        

        isLoading = false;
    }

    public bool GetCurrentPlayerTF()
    {
        return currentPlayer == NetworkManager.Inst.GetPlayer();
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
        if (GetCurrentPlayerTF())
            buttonWhite.image.sprite = buttonActive;

        else if(GetCurrentPlayerTF())
            buttonBlack.image.sprite = buttonActive;

        isActive = true;
    }

    // Change the other party and my button to a disabled button image
    public void ButtonInactive()
    {
        buttonWhite.image.sprite = buttonInactive;
        buttonBlack.image.sprite = buttonInactive;

        isActive = false;
    }

    // Changing the position of the button according to the turn
    private void ChangeButtonTransform()
    {
        // If the current player is white, set the position of your button and the opposing team's button
        if (CheckPlayer("white"))
        {
            buttonWhite.transform.position = new Vector2(posUp.position.x, posUp.position.y);
            buttonBlack.transform.position = new Vector2(posDown.position.x, posDown.position.y);
        }
        // else, set the position of your button and the opposing team's button
        else
        {
            buttonWhite.transform.position = new Vector2(posDown.position.x, posDown.position.y);
            buttonBlack.transform.position = new Vector2(posUp.position.x, posUp.position.y);
        }
    }


    // There are so many functions referenced elsewhere here that it is impossible to interpret
    public void EndTurn()
    {
        if (!isActive) return;
        if (!GetCurrentPlayerTF()) return;
        if (GameManager.Inst.gameOver) return;

        if (CardManager.Inst.GetSelectCard() != null)
        {
            CardManager.Inst.DestroyCard(CardManager.Inst.GetSelectCard());
        }
        GameManager.Inst.DestroyMovePlates();
        ChessManager.Inst.FalseIsMoving();
        //ChangeButtonTransform();
        ButtonInactive();

        CardManager.Inst.ChangeIsUse(false);
        SkillManager.Inst.SkillListCntPlus();
        photonView.RPC("NextTurn", RpcTarget.All);
        SuperSkillManager.Inst.SuperListCntPlus();
        GameManager.Inst.PlusAttackCnt();
        SuperSkillManager.Inst.CheckSuperSkill();

        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetIsStop(false);

        SkillManager.Inst.SkillListCntPlus();

        StartCoroutine(StartTurnCo());
        //WinOrLose();
    }
}