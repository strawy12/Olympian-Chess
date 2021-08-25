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

    #region ��������
    [Header("�� ���ϱ�")]
    [SerializeField] [Tooltip("���� �� ��带 ���մϴ�.")] EturnMode eTurnMode;
    [Header("������� ����")]
    [SerializeField] [Tooltip("ī�� ����� �ſ� �������ϴ�.")] bool fastMode;
    [Header("�����Ҷ� ī�� ����")]
    [SerializeField] [Tooltip("���� ī�� ������ ���մϴ�.")] int startCardCount;

    [Header("���� ���� ��ư")]
    [SerializeField] Button buttonWhite; // player's button
    [SerializeField] Button buttonBlack; // opponent's button

    [Header("Ȱ��ȭ,��Ȱ��ȭ ��ư �̹���")]
    [SerializeField] Sprite buttonActive; // Activated button image
    [SerializeField] Sprite buttonInactive; // Disabled button image

    [Header("���� ���� ��ư ��ġ")]
    [SerializeField] Transform posUp; // Opponent's button position
    [SerializeField] Transform posDown; // button position of the current player
    [SerializeField] GameObject loadingDisplay; 

    private bool isActive = false;

    [SerializeField]private string currentPlayer = "white";

    [Header("�Ӽ�")]
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
        #region �� �����̶� ���� ��
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