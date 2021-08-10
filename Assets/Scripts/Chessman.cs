using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // Reference
    public GameObject movePlate;
    public SpriteRenderer spriteRenderer = null;
    [SerializeField] GameObject particle;
    // Position
    private int xBoard = -1;
    private int yBoard = -1;
    private int moveCnt = 0;

    public int attackCount = 0;

    // Player "White" or "Black"
    public string player;

    // Reference all chesspiece(in Game)
    public Sprite black_bishop, black_king, black_knight, black_pawn, black_queen, black_rook;
    public Sprite white_bishop, white_king, white_knight, white_pawn, white_queen, white_rook;
    private List<SkillBase> chosenSkill = new List<SkillBase>();
    // Check if the chess pieces moved or not
    public bool isMoving = false;
    public bool isAttacking = false;
    private bool noneAttack = false;
    private bool isSelecting = false;
    private bool attackSelecting = false;
    private bool isMySkill = false;

    private void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        particle.SetActive(false);
    }
    //public void Activate()
    //{

    //    // take the instantiated location and adjust the transform
    //    SetCoords();

    //    switch (this.gameObject.name)
    //    {
    //        //what to fix(heavy)
    //        case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
    //        case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
    //        case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
    //        case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
    //        case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
    //        case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;

    //        case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
    //        case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
    //        case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
    //        case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
    //        case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
    //        case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
    //    }
    //}

    //public void SetCoords()
    //{
    //    float x = xBoard;
    //    float y = yBoard;

    //    x *= 0.684f;
    //    y *= 0.684f;

    //    x += -2.4f;
    //    y += -2.4f;

    //    // Aligns according the board
    //    this.transform.position = new Vector3(x, y, -1.0f);
    //}

    public IEnumerator SetCoordsAnimation()
    {
        Vector3 startPos = transform.position;

        float x = xBoard;
        float y = yBoard;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        // end position
        Vector3 endPos = new Vector3(x, y, -1.0f);

        // calculate distance for move speed
        float distance = (endPos - startPos).magnitude;

        float t = 0f;

        while (t < 1f)
        {
            if (transform.position == null) yield break;
            t += Time.deltaTime / distance * 10f;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }
    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        //if (PilSalGi.Inst.GetisUsePilSalGi()) return;
        List<Chessman> attack = GameManager.Inst.attackings;
        if (TurnManager.Instance.GetIsActive()) return;
        if (SkillManager.Inst.CheckDontClickPiece(this)) return;
<<<<<<< HEAD


=======
        //SkillManager.Inst.CheckSkillCancel();
>>>>>>> suan

        if (!GameManager.Inst.IsGameOver() && GameManager.Inst.GetCurrentPlayer() == player)
        {
            SkillManager.Inst.CheckSkillCancel("에로스의 사랑,수면,죽음의 땅,파도");
            GameManager.Inst.DestroyMovePlates(); // Destroy
            if (SkillManager.Inst.MoveControl(this))
            {
                return;
            }

            InitiateMovePlates(); // Instatiate
        }
    }
    public void PlusMoveCnt()
    {
        moveCnt++;
    }

    public int GetMoveCnt()
    {
        return moveCnt;
    }
    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    public void SetAttackSelecting(bool attackSelecting)
    {
        this.attackSelecting = attackSelecting;
    }
    
    public bool GetAttackSelecting()
    {
        return attackSelecting;
    }
    public void AddChosenSkill(SkillBase skill)
    {
        chosenSkill.Add(skill);
    }

    public void RemoveChosenSkill(SkillBase skill)
    {
        chosenSkill.Remove(skill);
    }

    public void InitiateMovePlates()
    {
        switch (name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;

            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;

            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;

            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;

            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;

            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);

                break;

            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);

                break;
        }
    }
<<<<<<< HEAD

=======
    // ----------------not------------------
    //public List<Chessman> CheckOnMovePlate()
    //{
    //    MovePlate[] movePlates = null;
    //    //GameManager.Inst.DestroyMovePlates();
    //    if (movePlates == null)
    //    {
    //        InitiateMovePlates();
    //        movePlates = FindObjectsOfType<MovePlate>();
    //    }
    //        List<Chessman> cps = new List<Chessman>();
    //    for (int i = 0; i < movePlates.Length; i++)
    //    {
    //        cps.Add(movePlates[i].GetChessPiece());
    //    }
        
    //    Debug.Log("응애 나 체크끝났당");
    //    return cps;
    //}
>>>>>>> suan
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        //if(PilSalGi.Inst.GetisUsePilSalGi())
        //{
        //    while (GameManager.Inst.PositionOnBoard(x, y))
        //    {
        //        MovePlateSpawn(x, y);
        //        x += xIncrement;
        //        y += yIncrement;
        //    }
        //    return;
        //}
<<<<<<< HEAD

        while (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y) == null)
=======
        while (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y) == null)
>>>>>>> suan
        {
            GameManager.Inst.MovePlateSpawn(x, y, this);
            x += xIncrement;
            y += yIncrement;
        }

        if (ChessManager.Inst.PositionOnBoard(x, y) && ChessManager.Inst.GetPosition(x, y).player != player)
        {
<<<<<<< HEAD
            GameManager.Inst.MovePlateAttackSpawn(x, y, this);
=======
            //ChessManager.Inst.MovePlateAttackSpawn(this, x, y);
>>>>>>> suan
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 0);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 0);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
<<<<<<< HEAD
        if (GameManager.Inst.PositionOnBoard(x, y))
=======
        
        if (ChessManager.Inst.PositionOnBoard(x, y))
>>>>>>> suan
        {
            ChessBase cp = ChessManager.Inst.GetPosition(x, y);

            //if (PilSalGi.Inst.GetisUsePilSalGi())
            //{
            //    MovePlateSpawn(x, y);
            //    return;
            //}

            if (cp == null)
            {
                GameManager.Inst.MovePlateSpawn(x, y, this);
            }

            else if (cp.player != player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x, y, this);
            }
        }
    }
    public void PawnMovePlate(int x, int y)
    {
        


        if (ChessManager.Inst.PositionOnBoard(x, y))
        {
            //if (PilSalGi.Inst.GetisUsePilSalGi())
            //{
            //    if (isMoved)
            //        MovePlateSpawn(x, y);

            //    else
            //    {
            //        if (player == "white")
            //        {
            //            MovePlateSpawn(x, y);
            //            MovePlateSpawn(x, y + 1);
            //        }

            //        else if (player == "black")
            //        {
            //            MovePlateSpawn(x, y);
            //            MovePlateSpawn(x, y - 1);
            //        }
            //    }
            //    return;
            //}

            if (ChessManager.Inst.GetPosition(x, y) == null)
            {
                if (moveCnt != 0)
                    GameManager.Inst.MovePlateSpawn(x, y, this);

                else
                {
                    if (player == "white")
                    {
<<<<<<< HEAD
                        GameManager.Inst.MovePlateSpawn(x, y, this);
                        if (GameManager.Inst.GetPosition(x, y + 1) == null)
                            GameManager.Inst.MovePlateSpawn(x, y + 1, this);
=======
                        MovePlateSpawn(x, y);
                        //if (CheckSkillList("바카스", GetCurrentPlayer(false))) return;
                        if (ChessManager.Inst.GetPosition(x, y + 1) == null)
                            MovePlateSpawn(x, y + 1);
>>>>>>> suan
                    }

                    else if (player == "black")
                    {
<<<<<<< HEAD
                        GameManager.Inst.MovePlateSpawn(x, y, this);
                        if (GameManager.Inst.GetPosition(x, y - 1) == null)
                            GameManager.Inst.MovePlateSpawn(x, y - 1, this);
=======
                        MovePlateSpawn(x, y);
                        //if (CheckSkillList("바카스", GetCurrentPlayer(false))) return;
                        if (ChessManager.Inst.GetPosition(x, y - 1) == null)
                            MovePlateSpawn(x, y - 1);
>>>>>>> suan
                    }
                }
            }

            if (ChessManager.Inst.PositionOnBoard(x + 1, y) && ChessManager.Inst.GetPosition(x + 1, y) != null &&
               ChessManager.Inst.GetPosition(x + 1, y).GetComponent<ChessBase>().player != player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x + 1, y, this);
            }

            if (ChessManager.Inst.PositionOnBoard(x - 1, y) && ChessManager.Inst.GetPosition(x - 1, y) != null &&
                ChessManager.Inst.GetPosition(x - 1, y).GetComponent<ChessBase>().player != player)
            {
                GameManager.Inst.MovePlateAttackSpawn(x - 1, y, this);
            }
        }
    }
    public SkillBase CheckSkillList(string name)
    {
<<<<<<< HEAD
        for (int i = 0; i < chosenSkill.Count; i++)
        {
            if (chosenSkill[i].gameObject.name == name)
            {
                return chosenSkill[i];
            }
        }
        return null;
=======
        //if (CheckReturnMovePlate(matrixX, matrixY, "서풍"))
        //    return;

        float x = matrixX;
        float y = matrixY;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        //mpScript.Setreference(this);
        mpScript.SetCoords(matrixX, matrixY);
        return mp;

        //if (isMySkill)
        //{
        //    mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", movePlateColor);
        //    mp.GetComponent<MovePlate>().SetIsSelected(true);
        //    MovePlate mpScript = mp.GetComponent<MovePlate>();
        //    mpScript.Setreference(this);
        //    mpScript.SetCoords(matrixX, matrixY);

        //    return;
        //}
        //else
        //{
        //    mp.GetComponent<SpriteRenderer>().material.SetColor("_Color", movePlateColor);
        //    MovePlate mpScript = mp.GetComponent<MovePlate>();
        //    mpScript.Setreference(this);
        //    mpScript.SetCoords(matrixX, matrixY);
        //}
>>>>>>> suan
    }

    public List<SkillBase> GetSkillList(string name)
    {
        List<SkillBase> _skillList = new List<SkillBase>();
        SkillBase skill;
        string[] names = name.Split(',');
        for (int i = 0; i < names.Length; i++)
        {
            skill = CheckSkillList(names[i]);
            if (skill != null)
            {
                _skillList.Add(skill);
            }
               
        }
        return _skillList;
    }

    public bool CheckIsMine()
    {
        if (player == GameManager.Inst.GetCurrentPlayer())
            return true;
        else
            return false;
    }

    public void SetNoneAttack(bool noneAttack)
    {
        this.noneAttack = noneAttack;
    }

    public bool IsAttackSpawn(int x, int y)
    {
        if (noneAttack && GameManager.Inst.GetPosition(x, y).name.Contains("king")) return true;
        else return false;
    }

    public void SetIsSelecting(bool _isHidden)
    {
        isSelecting = _isHidden;
    }

<<<<<<< HEAD
=======
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        //mpScript.Setreference(this);
        mpScript.SetCoords(matrixX, matrixY);
>>>>>>> suan

}

