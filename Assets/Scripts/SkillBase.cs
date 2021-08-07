using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public Chessman selectPiece { get; private set; }
    public Chessman selectPieceTo { get; private set; }
    protected int posX;
    protected int posY;
    protected int turnCnt = 0;
    protected SkillController skillController;

    private void Start()
    {
        skillController = GetComponent<SkillController>();
    }
    public virtual void UsingSkill() {}

    public virtual void StandardSkill() {}

    public virtual void ResetSkill() {}

    public void SetSelectPiece(Chessman cp) 
    {
        selectPiece = cp;
    }
    public Chessman GetSelectPieceTo() 
    {
        return selectPieceTo;
    }

    public void TurnCntPlus()
    {
        turnCnt++;
    }
}
