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
        ChessSetting();
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

    private void ChessSetting()
    {

        SetPosition(White_Pawn, -2.4f, -1.8f); SetPosition(White_Pawn, -1.72f, -1.8f); SetPosition(White_Pawn, -1.03f, -1.8f); SetPosition(White_Pawn, -0.336f, -1.8f);
        SetPosition(White_Pawn, 0.343f, -1.8f); SetPosition(White_Pawn, 1.05f, -1.8f); SetPosition(White_Pawn, 1.73f, -1.8f); SetPosition(White_Pawn, 2.41f, -1.8f);
        SetPosition(White_Rook, -2.4f, -2.53f); SetPosition(White_Knight, -1.72f, -2.53f); SetPosition(White_Knight, -1.03f, -2.53f); SetPosition(White_Bishop, -0.336f, -2.53f);
        SetPosition(White_Queen, 0.343f, -2.53f); SetPosition(White_King, 1.05f, -2.53f); SetPosition(White_Bishop, 1.73f, -2.53f); SetPosition(White_Rook, 2.41f, -2.53f);
    }

    private void SetPosition(GameObject _chessName, float _x, float _y)
    {
        Instantiate(_chessName, new Vector3(_x, _y, 0f), Quaternion.identity);
    }
}
