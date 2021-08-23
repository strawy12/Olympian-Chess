using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePPawn : MonoBehaviour
{
    [SerializeField]
    private GameObject movePlate_me;
    [SerializeField]
    private GameObject movePlate_other;

    public static bool isClicked = false;
    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log(RedMovePlate.isClicke);
    }

    private void OnMouseUp()
    {
        MyMoveplate();
        OtherMovePlate();
    }

    private void MyMoveplate()
    {
        GameObject M;
        M=Instantiate(movePlate_me);
        //업뎃문에 이 조건문 넣었을 떈 잘 나오는데 여기선 안됨.ㅅㅂ

        if(RedMovePlate.isClicke==true)
        {
            Debug.Log(RedMovePlate.isClicke);

            Destroy(M);
        }
    }

    private void OtherMovePlate()
    {
        GameObject M;
        M=Instantiate(movePlate_other);
        if (RedMovePlate.isClicke==true)
        {
            Debug.Log(RedMovePlate.isClicke);

            Destroy(M);
        }
    }
}
