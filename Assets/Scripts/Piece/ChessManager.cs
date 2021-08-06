using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessManager : MonoBehaviour
{
    public GameObject White_Bishop, White_King, White_Knight, White_Pawn, White_Queen, White_Rook;
    public GameObject Black_Bishop, Black_King, Black_Knight, Black_Pawn, Black_Queen, Black_Rook;

    public bool isMoving = false;
    public bool isAttacking = false;
    void Start()
    {
        Instantiate(White_Bishop,new Vector3(-2.5f,-1.8f,0f),Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetIsMoved(bool _isMoved)
    {
        isMoving = _isMoved;
    }
    public void SetAttack(bool _isAttack)
    {
        isAttacking = _isAttack;
    }
}
