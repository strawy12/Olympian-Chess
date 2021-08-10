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
    // Check if the chess pieces moved or not
    public bool isMoving = false;
    public bool isAttacking = false;
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
        
        // start position if (this == null) gameObject.SetActive(false);
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
        //if (WarbuffCheck()) return;
        //if (CheckSkillList("정의구현", GetCurrentPlayer(false)) && attackCount != 0 && attackCount < 3) return;

        //SkillManager.Inst.SetIsUsingCard(false);
        //SkillManager.Inst.CheckSkillCancel();

        if (!GameManager.Inst.IsGameOver() && GameManager.Inst.GetCurrentPlayer() == player)
        {
            DestroyMovePlates(); // Destroy
            InitiateMovePlates(); // Instatiate
        }
    }
    private string GetCurrentPlayer(bool reverse)
    {
        if (!reverse)
        {
            if (GameManager.Inst.GetCurrentPlayer() == "white")
            {
                return "black";
            }
            else if (GameManager.Inst.GetCurrentPlayer() == "black")
            {
                return "white";
            }
        }
        else
        {
            return GameManager.Inst.GetCurrentPlayer();
        }

        return GameManager.Inst.GetCurrentPlayer();
    }
    //private bool WarbuffCheck()
    //{
    //    if (SkillManager.Inst.CheckSkillList("전쟁광", GetCurrentPlayer(true)) || SkillManager.Inst.CheckSkillList("전쟁광", GetCurrentPlayer(false)))
    //    {
    //        Skill sk = SkillManager.Inst.GetSkillList("전쟁광", GetCurrentPlayer(true));

    //        if (sk == null)
    //        {
    //            sk = SkillManager.Inst.GetSkillList("전쟁광", GetCurrentPlayer(false));
    //        }
    //        if (sk.GetSelectPiece() == this)
    //            return false;
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
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
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++) //무브플레이트 모두 살피고 제거
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
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
                //if (CheckSkillList("바카스", GetCurrentPlayer(false)))
                //{
                //    LineMovePlate(1, 0);
                //    LineMovePlate(0, 1);
                //    LineMovePlate(-1, 0);
                //    LineMovePlate(0, -1);
                //}
                //else
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
    public List<Chessman> CheckOnMovePlate()
    {
        MovePlate[] movePlates = null;
        //GameManager.Inst.DestroyMovePlates();
        if (movePlates == null)
        {
            InitiateMovePlates();
            movePlates = FindObjectsOfType<MovePlate>();
        }
            List<Chessman> cps = new List<Chessman>();
        for (int i = 0; i < movePlates.Length; i++)
        {
            cps.Add(movePlates[i].GetChessPiece());
        }
        
        Debug.Log("응애 나 체크끝났당");
        return cps;
    }
    public void LineMovePlate(int xIncrement, int yIncrement)
    {

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        //if (CheckSkillList("바카스", GetCurrentPlayer(false)))
        //{
        //    if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y) == null)
        //        MovePlateSpawn(x, y);

        //    else if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y).player != player)
        //        MovePlateAttackSpawn(x, y);

        //    return;
        //}

        //if (SkillManager.Inst.CheckSkillList("질서", GetCurrentPlayer(true)) &&
        //    SkillManager.Inst.GetSkillList("질서", GetCurrentPlayer(true)).GetSelectPiece() == this)
        //{
        //    Skill sk;
        //    sk = SkillManager.Inst.GetSkillList("질서", GetCurrentPlayer(true));

        //    while (GameManager.Inst.PositionOnBoard(x, y))
        //    {

        //        if (GameManager.Inst.GetPosition(x, y) != null && sk.cnt != 0)
        //        {
        //            sk.ResetCnt();
        //            break;
        //        }

        //        else if (GameManager.Inst.GetPosition(x, y) != null && sk.cnt == 0)
        //        {
        //            sk.PlusCnt();
        //            if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y).player != player)
        //            {
        //                MovePlateAttackSpawn(x, y);
        //            }
        //            x += xIncrement;
        //            y += yIncrement;
        //            continue;
        //        }

        //        MovePlateSpawn(x, y);
        //        x += xIncrement;
        //        y += yIncrement;
        //    }

        //    if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y).player != player)
        //    {
        //        MovePlateAttackSpawn(x, y);
        //    }

        //    sk.ResetCnt();
        //    return;
        //}
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
        while (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (GameManager.Inst.PositionOnBoard(x, y) && GameManager.Inst.GetPosition(x, y).player != player)
        {
            ChessManager.Inst.MovePlateAttackSpawn(x, y);
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
        
        if (GameManager.Inst.PositionOnBoard(x, y))
        {
            Chessman cp = GameManager.Inst.GetPosition(x, y);

            //if (PilSalGi.Inst.GetisUsePilSalGi())
            //{
            //    MovePlateSpawn(x, y);
            //    return;
            //}

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }

            else if (cp.player != player)
            {
                MovePlateAttackSpawn(x, y);
            }

        }
    }
    //private bool OD_PawnMovePlate(int x, int y)
    //{
    //    //if (!SkillManager.Inst.CheckSkillList("질서", GetCurrentPlayer(true))) return false;
    //    //Skill sk = SkillManager.Inst.GetSkillList("질서", GetCurrentPlayer(true));
    //    //if (sk == null) return false;
    //    //if (sk.GetSelectPiece().name != name) return false;

    //    if (isMoved)
    //    {
    //        if (GameManager.Inst.GetPosition(x, y) != null)
    //            MovePlateSpawn(x, y + 1);
    //        else
    //            MovePlateSpawn(x, y);
    //    }

    //    else
    //    {
    //        if (player == "white")
    //        {
    //            MovePlateSpawn(x, y);
    //            if (GameManager.Inst.GetPosition(x, y + 1) == null)
    //                MovePlateSpawn(x, y + 1);
    //            else if (GameManager.Inst.GetPosition(x, y + 1) != null && GameManager.Inst.GetPosition(x, y + 2) == null)
    //                MovePlateSpawn(x, y + 2);
    //        }

    //        else if (player == "black")
    //        {
    //            MovePlateSpawn(x, y);
    //            if (GameManager.Inst.GetPosition(x, y + 1) == null)
    //                MovePlateSpawn(x, y - 1);
    //            else if (GameManager.Inst.GetPosition(x, y + 1) != null && GameManager.Inst.GetPosition(x, y + 2) == null)
    //                MovePlateSpawn(x, y - 2);
    //        }
    //    }
    //    if (GameManager.Inst.PositionOnBoard(x + 1, y) && GameManager.Inst.GetPosition(x + 1, y) != null &&
    //       GameManager.Inst.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
    //    {
    //        MovePlateAttackSpawn(x + 1, y);
    //    }

    //    if (GameManager.Inst.PositionOnBoard(x - 1, y) && GameManager.Inst.GetPosition(x - 1, y) != null &&
    //        GameManager.Inst.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
    //    {
    //        MovePlateAttackSpawn(x - 1, y);
    //    }


    //    return true;
    //}
    public void PawnMovePlate(int x, int y)
    {
        //if (OD_PawnMovePlate(x, y))
        //    return;


        if (GameManager.Inst.PositionOnBoard(x, y))
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

            if (GameManager.Inst.GetPosition(x, y) == null)
            {
                if (moveCnt != 0)
                    MovePlateSpawn(x, y);

                else
                {
                    if (player == "white")
                    {
                        MovePlateSpawn(x, y);
                        //if (CheckSkillList("바카스", GetCurrentPlayer(false))) return;
                        if (GameManager.Inst.GetPosition(x, y + 1) == null)
                            MovePlateSpawn(x, y + 1);
                    }

                    else if (player == "black")
                    {
                        MovePlateSpawn(x, y);
                        //if (CheckSkillList("바카스", GetCurrentPlayer(false))) return;
                        if (GameManager.Inst.GetPosition(x, y - 1) == null)
                            MovePlateSpawn(x, y - 1);
                    }
                }
            }

            if (GameManager.Inst.PositionOnBoard(x + 1, y) && GameManager.Inst.GetPosition(x + 1, y) != null &&
               GameManager.Inst.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (GameManager.Inst.PositionOnBoard(x - 1, y) && GameManager.Inst.GetPosition(x - 1, y) != null &&
                GameManager.Inst.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    //public void SleepParticle(bool isShow, string player)
    //{
    //    particle.SetActive(isShow);
    //    if (player == "black")
    //        particle.GetComponent<ParticleSystem>().gravityModifier = -0.01f;
    //    else
    //        particle.GetComponent<ParticleSystem>().gravityModifier = 0.005f;
    //}

    //public bool GetisMySkill()
    //{
    //    return isMySkill;
    //}
    //public void SetMySkill(bool isTrue)
    //{
    //    isMySkill = isTrue;
    //}



    //public void WV_MovePlateSpawn(int matrixX, int matrixY)
    //{
    //    //Skill sk = SkillManager.Inst.GetSkillList("파도", GetCurrentPlayer(true));
    //    float x = matrixX;
    //    float y = matrixY;

    //    x *= 0.614f;
    //    y *= 0.614f;

    //    x += -2.154f;
    //    y += -2.154f;

    //    GameObject mp = Instantiate(movePlate, new Vector3(x, y, -2.0f), Quaternion.identity);
    //    MovePlate mv = mp.GetComponent<MovePlate>();
    //    mv.Setreference(sk.GetSelectPiece());
    //    mv.Setreference(this);
    //    mv.SetCoords(matrixX, matrixY);
    //}
    public GameObject MovePlateSpawn(int matrixX, int matrixY)
    {
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
    }

    //private bool CheckReturnMovePlate(int x, int y, string name)
    //{
    //    if (SkillManager.Inst.CheckSkillList(name, GetCurrentPlayer(false)) && SkillManager.Inst.CheckSkillList(name, GetCurrentPlayer(true)))
    //    {
    //        SkillController sk1 = SkillManager.Inst.GetSkillList(name, GetCurrentPlayer(false));
    //        SkillController sk2 = SkillManager.Inst.GetSkillList(name, GetCurrentPlayer(true));

    //        if (sk1 != null && sk2 != null)
    //        {
    //            if (sk1.CheckPos(x, y) || sk2.CheckPos(x, y))
    //            {
    //                return true;
    //            }

    //        }
    //    }
    //    else
    //    {
    //        if (SkillManager.Inst.CheckSkillList(name, GetCurrentPlayer(false)) || SkillManager.Inst.CheckSkillList(name, GetCurrentPlayer(true)))
    //        {
    //            Skill sk = SkillManager.Inst.GetSkillList(name, GetCurrentPlayer(false));
    //            if (sk == null)
    //            {
    //                sk = SkillManager.Inst.GetSkillList(name, GetCurrentPlayer(true));
    //            }
    //            if (sk != null)
    //            {
    //                if (sk.CheckPos(x, y))
    //                {
    //                    return true;
    //                }

    //            }
    //        }
    //    }

    //    return false;
    //}

    public bool CheckIsMine()
    {
        if (player == GameManager.Inst.GetCurrentPlayer())
            return true;
        else
            return false;
    }
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        //Skill sk = SkillManager.Inst.GetSkillList("달빛", GetCurrentPlayer(true));

        //if ((CheckSkillList("달빛", GetCurrentPlayer(true)) && !sk.isMLMoved()))
        //    if (GameManager.Inst.GetPosition(matrixX, matrixY).name == "black_king" || GameManager.Inst.GetPosition(matrixX, matrixY).name == "white_king")
        //        return;
        //if (CheckSkillList("질서", GetCurrentPlayer(true)))
        //    if (GameManager.Inst.GetPosition(matrixX, matrixY).name == "black_king" || GameManager.Inst.GetPosition(matrixX, matrixY).name == "white_king")
        //        return;

        float x = matrixX;
        float y = matrixY;

        x *= 0.684f;
        y *= 0.684f;

        x += -2.4f;
        y += -2.4f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        //mpScript.Setreference(this);
        mpScript.SetCoords(matrixX, matrixY);

        
    }
   
}
