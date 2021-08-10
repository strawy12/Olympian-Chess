using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
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

    private bool isActive = false;


    [Header("�Ӽ�")]
    public bool isLoading;
    public bool myTurn; 
    #endregion

    enum EturnMode { Random, My, Other }

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
        TurnSetting();
        Coroutine();
    }
    private void Coroutine()
    {
        StartCoroutine(CardShare());
        StartCoroutine(StartTurnCo());
    }

    void TurnSetting()
    {
        // 5 second delay in fast mode
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        // normal turn mode
        switch (eTurnMode)
        {
            // In Random mode, choose a turn at random with your opponent
            case EturnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;

            // If it's my turn, activate my turn
            case EturnMode.My:
                myTurn = true;
                break;

            // If it's your opponent's turn, activate your opponent's turn.
            case EturnMode.Other:
                myTurn = false;
                break;
        }
        isLoading = true;
    }

    private IEnumerator CardShare()
    {
        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            CardManager.Inst.AddCard(false);
            yield return delay05;
            CardManager.Inst.AddCard(true);
        }
    }

    // Start a new turn & Win, lose decision
    public IEnumerator StartTurnCo()
    {
        // turn division
        isLoading = true;
        if (myTurn)
            Debug.Log("���� ��");
        else
            Debug.Log("���� ��");

        yield return delay07;
        isLoading = false;
                 
        OnTurnStarted?.Invoke(myTurn);
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
    public void ButtonColor()
    {
        if (myTurn)
            buttonWhite.image.sprite = buttonActive;

        else if(!myTurn)
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
        if (GameManager.Inst.GetCurrentPlayer() == "white")
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

        if (GameManager.Inst.gameOver) return;

        if (CardManager.Inst.GetSelectCard() != null)
        {
            var targetCards = CardManager.Inst.GetMyCards();
            CardManager.Inst.DestroyCard(CardManager.Inst.GetSelectCard(), targetCards);
        }
        //,,..CardManager.Inst.UpdateCard();
        myTurn = !myTurn;

        ChessManager.Inst.FalseIsMoving();
        ChangeButtonTransform();
        ButtonInactive();

        CardManager.Inst.ChangeIsUse(false);
        SkillManager.Inst.SkillListCntPlus();
        GameManager.Inst.NextTurn();
        GameManager.Inst.PlusAttackCnt();

        GameManager.Inst.SetUsingSkill(false);
        GameManager.Inst.SetMoving(true);
        GameManager.Inst.SetIsStop(false);


        StartCoroutine(StartTurnCo());
        WinOrLose();
    }

}
