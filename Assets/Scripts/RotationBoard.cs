using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBoard
{
    public static Camera camera;
    public static Chessman[] playerWhite;
    public static Chessman[] playerBlack;
    public static List<Card> mycards;
    public static List<Card> ohtercards;
     
    public static void Rotate() //카메라 돌리는 함수
    {
        camera.transform.Rotate(0f, 0f, 180f);
        foreach (var w in playerWhite)
        {
            if (w == null)
                continue;
            w.transform.Rotate(0f, 0f, 180f);
        }

        foreach (var b in playerBlack)
        {
            if (b == null)
                continue;
            b.transform.Rotate(0f, 0f, 180f);
        }
    }
}

