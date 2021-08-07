//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PilSalGi : MonoBehaviour
//{
//    private static PilSalGi inst;
//    public static PilSalGi Inst
//    {
//        get
//        {
//            if (inst == null)
//            {
//                var obj = FindObjectOfType<PilSalGi>();
//                if (obj != null)
//                {
//                    inst = obj;
//                }
//                else
//                {
//                    var newObj = new GameObject().AddComponent<PilSalGi>();
//                    inst = newObj;
//                }
//            }
//            return inst;
//        }
//    }
//    private void Awake()
//    {
//        var objs = FindObjectsOfType<PilSalGi>();
//        if (objs.Length != 1)
//        {
//            Destroy(gameObject);
//            return;
//        }
//    }

//    [SerializeField] Sprite firstSp;
//    [SerializeField] Sprite secondSp;
//    [SerializeField] Sprite thirdSp;
//    [SerializeField] Sprite fourthSp;
//    [SerializeField] Sprite lastSp;
//    private int attackCnt = 0;
//    private bool isUsePilSalGi = false;
//    private SpriteRenderer spriteRenderer = null;
//    private Chessman selectPiece = null;
//    public List<Chessman> cps { get; set; } = new List<Chessman>();

//    private void Start()
//    {
//        spriteRenderer = GetComponent<SpriteRenderer>();
//    }
//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Y))
//            attackCntPlus();
//    }
//    //private int GetTurnTime()
//    //{
//    //    return SkillManager.Inst.turnTime;
//    //}
//    public void attackCntPlus()
//    {
//        attackCnt++;
//        ChangeSprite();
//    }
//    private void ChangeSprite()
//    {
//        switch(attackCnt)
//        {
//            case 1:
//                spriteRenderer.sprite = firstSp;
//                break;
//            case 2:
//                spriteRenderer.sprite = secondSp;
//                break;
//            case 3:
//                spriteRenderer.sprite = thirdSp;
//                break;
//            case 4:
//                spriteRenderer.sprite = fourthSp;
//                break;
//            case 5:
//                spriteRenderer.sprite = lastSp;
//                isUsePilSalGi = true;
//                break;
//            default:
//                isUsePilSalGi = true;
//                break;

//        }
//    }
//    private void OnMouseUp()
//    {
//        if (!isUsePilSalGi) return;
//        TurnManager.Instance.SetIsActive(false);
//        GameManager.Inst.AllMovePlateSpawn(null, true);
//    }
//    public bool GetisUsePilSalGi()
//    {
//        return isUsePilSalGi;
//    }
//    public void SetselectPiece(Chessman cp)
//    {
//        GameManager.Inst.DestroyMovePlates();
//        selectPiece = cp;
//        selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(133, 101, 0, 0));
//        selectPiece.InitiateMovePlates();
//        UsingPilSalGi();
//    }
//    private void UsingPilSalGi()
//    {
//        MovePlate[] mps = FindObjectsOfType<MovePlate>();
//        for (int i = 0; i < mps.Length; i++)
//        {
//            if(mps[i].Getreference() == selectPiece && mps[i].GetChessPiece() != selectPiece)
//            {
//                cps.Add(mps[i].GetChessPiece());
//            }
            
//        }

//        for (int i = 0; i < cps.Count; i++)
//        {
//            if (cps[i] == null)
//                continue;
//            SkillManager.Inst.SetDontClickPiece(cps[i]);
//        }
//        StartCoroutine(SkillEffect());
//        isUsePilSalGi = false;
//        selectPiece.DestroyMovePlates();
//        TurnManager.Instance.ButtonColor();
//        attackCnt = 0;
//    }
//    private void ResetPilSalGi()
//    {
//        ChangeSprite();
//        this.selectPiece.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
//        isUsePilSalGi = false;
//        this.selectPiece = null;
//        cps = null;
//    }
//    private IEnumerator SkillEffect()
//    {
//        Chessman cp = null;
//        int k = GetTurnTime() + 2;
        
//        while (GetTurnTime() < k)
//        {
//            for (int i = 0; i < cps.Count; i++)
//            {
//                if (cps[i] == null)
//                    continue;
//                cp = cps[i];

//                cp.spriteRenderer.material.SetColor("_Color", new Color32(255, 228, 0, 0));
//            }
//            yield return new WaitForSeconds(0.2f);
//            for (int i = 0; i < cps.Count; i++)
//            {
//                if (cps[i] == null)
//                    continue;
//                cp = cps[i];

//                cp.spriteRenderer.material.SetColor("_Color", new Color32(0, 0, 0, 0));
//            }
//            yield return new WaitForSeconds(0.2f);
//        }
//        for (int i = 0; i < cps.Count; i++)
//        {
//            if (cps[i] == null)
//                continue;
//            cp = cps[i];

//            SkillManager.Inst.RemoveDontClickPiece(cp);
//        }
        
//        ResetPilSalGi();
//    }
//}
