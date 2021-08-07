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

    public virtual void UsingSkill(){}

    public virtual void StandardSkill(){}

    public virtual void ResetSkill(){}

}
