using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    private static TurnManager inst;
    public static TurnManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<TurnManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<TurnManager>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<TurnManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    private GameManager gameManager = null;

    [Header("Develop")]
    [SerializeField] [Tooltip("시작 턴 모드를 정합니다.")] EturnMode eTurnMode;
    [SerializeField] [Tooltip("카드 배분이 매우 빨라집니다.")] bool fastMode;
    [SerializeField] [Tooltip("시작 카드 개수를 정합니다.")] int startCardCount;

    [SerializeField] Button buttonWhite;
    [SerializeField] Button buttonBlack;
    [SerializeField] Sprite buttonActive;
    [SerializeField] Sprite buttonInactive;
    [SerializeField] Transform posUp;
    [SerializeField] Transform posDown;

    private bool isActive = false;


    [Header("Properties")]
    public bool isLoading;
    public bool myTurn;

    Chessman cm;

    enum EturnMode { Random, My, Other }
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static event Action<bool> OnTurnStarted;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void GameSetUp()
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTurnMode)
        {
            case EturnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;

            case EturnMode.My:
                myTurn = true;
                break;

            case EturnMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetUp();
        isLoading = true;

        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            CardManager.Inst.AddCard(false);
            yield return delay05;
            CardManager.Inst.AddCard(true);
        }
        StartCoroutine(StartTurnCo());
    }

    // 게임 끝나는 함수 
    public IEnumerator StartTurnCo()
    {
        isLoading = true;
        if (myTurn)
            Debug.Log("나의 턴");
        else
            Debug.Log("너의 턴");

        yield return delay07;
        isLoading = false;
        OnTurnStarted?.Invoke(myTurn);
        //yield return new WaitForSeconds(0.5f);
        if (!GameManager.Inst.CheckArr(true, "white_king"))
        {
            GameManager.Inst.Winner("black");
            GameManager.Inst.GameOver();
        }
        else if (!GameManager.Inst.CheckArr(false, "black_king"))
        {
            GameManager.Inst.Winner("white");
            GameManager.Inst.GameOver();
        }
    }

   
    public bool GetIsActive()
    {
        return isActive;
    }
    public void SetIsActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public void ButtonColor()
    {
        if(myTurn)
            buttonWhite.image.sprite = buttonActive;

        else if(!myTurn)
            buttonBlack.image.sprite = buttonActive;

        isActive = true;
    }

    public void ButtonInactive()
    {
        buttonWhite.image.sprite = buttonInactive;
        buttonBlack.image.sprite = buttonInactive;

        isActive = false;
    }

    private void ChangeButtonTransform()
    {
        if(GameManager.Inst.GetCurrentPlayer() == "white")
        {
            buttonWhite.transform.position = new Vector2(posUp.position.x, posUp.position.y);
            buttonBlack.transform.position = new Vector2(posDown.position.x, posDown.position.y);
        }

        else
        {
            buttonWhite.transform.position = new Vector2(posDown.position.x, posDown.position.y);
            buttonBlack.transform.position = new Vector2(posUp.position.x, posUp.position.y);
        }
    }

    public void EndTurn()
    {
        if (!isActive) return;

        List<Chessman> attack = GameManager.Inst.attackings;
        Skill sk = SkillManager.Inst.GetSkillList("달빛", GameManager.Inst.GetCurrentPlayer());

        if (GameManager.Inst.gameOver) return;

        if (CardManager.Inst.selectCard != null)
        {
            var targetCards = CardManager.Inst.myCards;
            CardManager.Inst.DestroyCard(CardManager.Inst.selectCard, targetCards);
        }
        CardManager.Inst.UpdateCard();
        myTurn = !myTurn;

        GameManager.Inst.FasleIsMoving();
        ChangeButtonTransform();
        ButtonInactive();

        CardManager.Inst.ChangeIsUse(false);
        SkillManager.Inst.SetTurnTime();
        GameManager.Inst.NextTurn();

        for (int i = 0; i < attack.Count; i++)
            attack[i].attackCount++;

        StartCoroutine(StartTurnCo());
    }

    private bool CheckSkillList(string name, string player)
    {
        if (SkillManager.Inst.CheckSkillList(name, player))
            return true;
        else
            return false;
    }
}
