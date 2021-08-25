using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBirth : SkillBase
{
    public override void UsingSkill()
    {
        photonView.RPC("GB_UsingSkill", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void GB_UsingSkill()
    {
        selectPiece.SetAttackSelecting(true);
        selectPiece.spriteRenderer.material.color = new Color32(71, 200, 62, 225);
    }

    public override void StandardSkill()
    {
        Debug.Log("이게 되는지 실험 해보겠습니유진");
        ChessBase baby;

        if (selectPiece.GetChessData().player == "white")
        {
            if (ChessManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1) == null)
            {
                movePlate.SetCoords(selectPiece.GetXBoard(), selectPiece.GetYBoard() + 1);
            }
            else
            {
                movePlate.SetCoords(skillData.posX, skillData.posY);
            }
            

            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetWhiteObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
            Debug.Log(baby.GetXBoard() + ", " + baby.GetYBoard());

            photonView.RPC("ChangePiece", RpcTarget.AllBuffered, baby.gameObject.GetPhotonView().ViewID);
        }
        else
        {
            if (ChessManager.Inst.GetPosition(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1) == null)
            {
                movePlate.SetCoords(selectPiece.GetXBoard(), selectPiece.GetYBoard() - 1);
            }
            else
            {
                movePlate.SetCoords(skillData.posX, skillData.posY);
            }

            Debug.Log(selectPiece.GetXBoard() + ", " + selectPiece.GetYBoard());
            baby = ChessManager.Inst.Creat(ChessManager.Inst.GetBlackObject()[0], selectPiece.GetXBoard(), selectPiece.GetYBoard());
            Debug.Log(baby.GetXBoard() + ", " + baby.GetYBoard());

            photonView.RPC("ChangePiece", RpcTarget.AllBuffered, baby.gameObject.GetPhotonView().ViewID);

        }
        Debug.Log(movePlate.Getreference().GetXBoard() + ", " + movePlate.Getreference().GetYBoard());
        ChessManager.Inst.SetPosition(baby);
        ChessManager.Inst.AddArr(baby);

        if (selectPiece != null)
        {
            selectPiece.RemoveChosenSkill(this);
            selectPiece.SetAttackSelecting(false);
        }

        
        RPC_DetroySkill();
    }

    [PunRPC]
    private void ChangePiece(int num)
    {
        if (NetworkManager.Inst.GetPlayer() == "white") return;
        PhotonView.Find(num).gameObject.transform.Rotate(0f, 0f, 180f);
    }
}