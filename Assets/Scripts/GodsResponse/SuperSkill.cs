using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSkill : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    [SerializeField] private string player;
    private bool isUsed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer.color = Color.gray;
        col.enabled = false;
    }

    private void OnMouseUp()
    {
        if(!isUsed)
        {
            SuperSkillManager.Inst.SpawnSkill();
            spriteRenderer.color = Color.gray;
            col.enabled = false;
            isUsed = true;
        }
    }

    public void CheckSkill()
    {
        if (isUsed) return;

        if (player == "white")
        {
            if (SkillActive(SuperSkillManager.Inst.whiteGodsRes))
            {
                spriteRenderer.color = Color.white;
                col.enabled = true;
            }
        }

        else
        {
            if (SkillActive(SuperSkillManager.Inst.blackGodsRes))
            {
                spriteRenderer.color = Color.white;
                col.enabled = true;
            }
        }

        if (player != TurnManager.Instance.GetCurrentPlayer())
            col.enabled = false;
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

    public void ChangeSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
