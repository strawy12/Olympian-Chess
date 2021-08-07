using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    protected Chessman selectPiece;
    protected Chessman selectPieceTo;
    protected int posX;
    protected int posY;
    protected int turnCnt = 0;

    public void UsingSkill() {}

    public void StandardSkill() {}

    public void ResetSkill() {}

}
