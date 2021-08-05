using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilSalGi : MonoBehaviour
{
    #region SingleTon
    private static PilSalGi inst;
    public static PilSalGi Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<PilSalGi>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<PilSalGi>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PilSalGi>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    #region SerializeField Var
    [SerializeField] Sprite firstSp;
    [SerializeField] Sprite secondSp;
    [SerializeField] Sprite thirdSp;
    [SerializeField] Sprite fourthSp;
    [SerializeField] Sprite lastSp;
    #endregion

    #region Var List

    private int attackCnt = 0;
    private bool isUsePilSalGi = false;
    private SpriteRenderer spriteRenderer = null;
    private Chessman selectPiece = null;
    public List<Chessman> cps { get; private set; } = new List<Chessman>();

    #endregion

    #region System
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            AttackCntPlus();
    }

    private void OnMouseUp() // PilSalGi Object Click
    {
        if (!isUsePilSalGi) return;
        TurnManager.Inst.SetIsActive(false); // Btn Disabled
        GameManager.Inst.AllMovePlateSpawn(null, true);
    }

    #endregion

    #region Script Access 

    private int GetTurnTime()
    {
        return SkillManager.Inst.turnTime; 
    }

    public bool GetisUsePilSalGi()
    {
        return isUsePilSalGi;
    }

    #endregion

    #region PilSalGi System

    public void AttackCntPlus()
    {
        attackCnt++;
        ChangeSprite();
    }

    private void ChangeSprite() // When AttackCntPlus ChangeSprite
    {
        switch(attackCnt)
        {
            case 1:
                spriteRenderer.sprite = firstSp;
                break;
            case 2:
                spriteRenderer.sprite = secondSp;
                break;
            case 3:
                spriteRenderer.sprite = thirdSp;
                break;
            case 4:
                spriteRenderer.sprite = fourthSp;
                break;
            case 5:
                spriteRenderer.sprite = lastSp;
                isUsePilSalGi = true;
                break;
            default:
                isUsePilSalGi = true;
                break;

        }
    }
    
    public void SetselectPiece(Chessman cp) //Set of ChessPiece to be used for PilSalGi
    {
        GameManager.Inst.DestroyMovePlates();
        selectPiece = cp;
        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(133, 101, 0, 0));
        selectPiece.InitiateMovePlates(); //Create MovePlates with the movement range of the ChessPiece
        UsingPilSalGi();
    }

    private void UsingPilSalGi()
    {
        MovePlate[] mps = FindObjectsOfType<MovePlate>();
        for (int i = 0; i < mps.Length; i++)
        {
            if(mps[i].Getreference() == selectPiece && mps[i].GetChessPiece() != selectPiece)
            {
                cps.Add(mps[i].GetChessPiece());
            }

        } // Check the ChessPieces on the MovePlate

        for (int i = 0; i < cps.Count; i++)
        {
            if (cps[i] == null)
                continue;
            SkillManager.Inst.SetDontClickPiece(cps[i]);
        }

        StartCoroutine(SkillEffect());
        isUsePilSalGi = false;
        selectPiece.DestroyMovePlates();
        TurnManager.Inst.ButtonColor(); // Btn Activate
        attackCnt = 0;
    }

    private void ResetPilSalGi()
    {
        ChangeSprite();
        this.selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
        isUsePilSalGi = false;
        this.selectPiece = null;
        cps = null;
    }

    private IEnumerator SkillEffect()
    {
        Chessman cp = null;
        int k = GetTurnTime() + 2;
        
        while (GetTurnTime() < k)
        {
            for (int i = 0; i < cps.Count; i++)
            {
                if (cps[i] == null)
                    continue;
                cp = cps[i];

                cp.spriteRenderer.material.SetColor("_Color", new Color32(255, 228, 0, 0));
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < cps.Count; i++)
            {
                if (cps[i] == null)
                    continue;
                cp = cps[i];

                cp.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
            }
            yield return new WaitForSeconds(0.2f);

            // 설정된 체스말들에게 이펙트 적용시키는 코드
        } 
        for (int i = 0; i < cps.Count; i++) 
        {
            if (cps[i] == null)
                continue;
            cp = cps[i];

            SkillManager.Inst.RemoveDontClickPiece(cp); // 다 끝나면 모두 초기화 시킴
        }
        
        ResetPilSalGi(); // 리셋
    }
    #endregion
}
