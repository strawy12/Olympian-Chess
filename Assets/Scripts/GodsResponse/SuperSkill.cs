using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSkill : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    [SerializeField] private string player;
    private Sprite defaultSprite;
    private Sprite usingSprite;
    private bool isUsed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        col = GetComponent<Collider2D>();

        col.enabled = false;
        spriteRenderer.color = Color.gray;
    }

    private void OnMouseUp()
    {
        if (GameManager.Inst.GetPlayer() != player) return;

        if (!isUsed)
        {
            SuperSkillManager.Inst.SpawnSkill(this);
            spriteRenderer.color = Color.gray;
            col.enabled = false;
            isUsed = true;
        }
    }

    public void CheckSkill()
    {
        if (isUsed) return;
        if (!gameObject.activeSelf) return;

        if (player == "white")
        {
            if (SkillActive(SuperSkillManager.Inst.GetResponse(true))) //white
            {
                spriteRenderer.color = Color.white;
                ActiveCollider();
            }
        }

        else
        {
            if (SkillActive(SuperSkillManager.Inst.GetResponse(false))) //black
            {
                spriteRenderer.color = Color.white;
                ActiveCollider();
            }
        }
    }

    private void ActiveCollider()
    {
        if(player != GameManager.Inst.GetPlayer())
        {
            Debug.Log("sdf");
            col.enabled = false;
            return;
        }

        if (player == GameManager.Inst.GetPlayer())
        {
            if (player != TurnManager.Instance.GetCurrentPlayer())
            {
                col.enabled = false;
                Debug.Log(player + ", " + TurnManager.Instance.GetCurrentPlayer());

            }
            else if(player == TurnManager.Instance.GetCurrentPlayer())
            {
                col.enabled = true;
                Debug.Log(player + ", " + TurnManager.Instance.GetCurrentPlayer());
            }
        }
    }

    private bool SkillActive(string response)
    {
        ChessBase[] black = ChessManager.Inst.GetPlayerBlack();
        ChessBase[] white = ChessManager.Inst.GetPlayerWhite();

        int cnt = 0;

        if (response == "Zeus")
        {
            if (player == "white")
            {
                for (int i = 0; i < black.Length; i++)
                {
                    if (black[i] == null) continue;
                    if (black[i].name.Contains("knight"))
                        cnt++;
                }

                if (cnt > 1) return false;
                else return true;
            }

            else
            {
                for (int i = 0; i < white.Length; i++)
                {
                    if (white[i] == null) continue;
                    if (white[i].name.Contains("knight"))
                        cnt++;
                }

                if (cnt > 1) return false;
                else return true;
            }
        }

        else
        {
            if (player == "white")
            {
                for (int i = 0; i < black.Length; i++)
                {
                    if (black[i] == null)
                    {
                        cnt++;
                    }
                }

                if (cnt > 4) return true;
                else return false;
            }

            else
            {
                for (int i = 0; i < white.Length; i++)
                {
                    if (white[i] == null)
                    {
                        cnt++;
                    }
                }

                if (cnt > 4) return true;
                else return false;
            }
        }
    }
    public void UsingSkill()
    {
        spriteRenderer.sprite = usingSprite;
    }

    public void UnUsingSkill()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    public string GetPlayer()
    {
        return player;
    }

    public void ChangeSprite(Sprite sprite, Sprite sprite2)
    {
        spriteRenderer.sprite = sprite;
        defaultSprite = sprite;
        usingSprite = sprite2;
    }
}