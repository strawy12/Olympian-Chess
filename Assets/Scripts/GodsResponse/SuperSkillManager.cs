using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSkillManager : MonoBehaviourPunCallbacks
{
    #region Singleton
    private static SuperSkillManager inst;
    public static SuperSkillManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<SuperSkillManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<SuperSkillManager>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }
    #endregion

    [SerializeField] private List<SkillBase> superList = new List<SkillBase>();
    [SerializeField] SuperSkill whiteIcon;
    [SerializeField] SuperSkill blackIcon;
    [SerializeField] GameObject skillPrefab;
    [SerializeField] Sprite zeus;
    [SerializeField] Sprite zeusUsing;
    [SerializeField] Sprite poseidon;
    [SerializeField] Sprite poseidonUsing;

    private string whiteGodsRes;
    private string blackGodsRes;

    private int godNum = 0;

    public void UsingSkill(MovePlate mp)
    {
        if (superList.Count < 1) return;

        SkillBase sb = superList[superList.Count - 1];
        sb.SetPosX(mp.GetPosX());
        sb.SetPosY(mp.GetPosY());
        sb.StandardSkill();

        CardManager.Inst.SetSelectCard(null);
    }

    public GameObject SpawnSkill(SuperSkill superSkill)
    {
        string response = GameManager.Inst.GetPlayer() == "white" ? whiteGodsRes : blackGodsRes;
        User user = NetworkManager.Inst.LoadDataFromJson<User>();

        if (response != "")
            user.superSkills[godNum].amount--;

        GameObject obj = null;
        int skillID;
        SkillBase sb;
        photonView.RPC("ChangeUsingSprite", RpcTarget.AllBuffered, superSkill.GetPlayer());

        obj = NetworkManager.Inst.SpawnObject(skillPrefab);
        obj.transform.SetParent(null);

        if (TurnManager.Instance.GetCurrentPlayer() == "white")
        {
            obj.name = whiteGodsRes;
        }

        else
        {
            obj.name = blackGodsRes;
        }

        skillID = obj.GetPhotonView().ViewID;

        photonView.RPC("AddComponent", RpcTarget.AllBuffered, skillID, obj.name);
        sb = obj.GetComponent<SkillBase>();
        sb.UsingSkill();
        superList.Add(sb);
        return obj;
    }

    [Photon.Pun.PunRPC]
    private void ChangeUsingSprite(string name)
    {
        if (name == "white")
        {
            whiteIcon.UsingSkill();
        }
        else
        {
            blackIcon.UsingSkill();
        }
    }

    public void UnUsingSkill(string name)
    {
        photonView.RPC("ChangeDefaultSprite", RpcTarget.AllBuffered, name);
    }


    [Photon.Pun.PunRPC]
    private void ChangeDefaultSprite(string name)
    {
        if (name == whiteGodsRes)
        {
            whiteIcon.UnUsingSkill();
        }
        else
        {
            blackIcon.UnUsingSkill();
        }
    }

    public void SetActive(bool isActive)
    {
        whiteIcon.gameObject.SetActive(isActive);
        blackIcon.gameObject.SetActive(isActive);

        if (isActive)
        {
            SetGodsResponse();
        }
    }

    private void SetGodsResponse()
    {
        User user = NetworkManager.Inst.LoadDataFromJson<User>();
        string response = "";

        for (int i = 0; i < user.superSkills.Length; i++)
        {
            if (user.superSkills[i].isSelect)
            {
                response = user.superSkills[i].skillName;
                godNum = i;
                break;
            }
        }

        if (GameManager.Inst.GetPlayer() == "white")
        {
            whiteGodsRes = response;
            SetSprite(whiteIcon, response);
        }

        else if (GameManager.Inst.GetPlayer() == "black")
        {
            blackGodsRes = response;
            SetSprite(blackIcon, response);
        }

        NetworkManager.Inst.SaveDataToJson(user, true);
    }

    private void SetSprite(SuperSkill icon, string response)
    {
        if (response == "Zeus")
        {
            icon.ChangeSprite(zeus, zeusUsing);
        }

        else if (response == "Poseidon")
        {
            icon.ChangeSprite(poseidon, poseidonUsing);
        }

        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void AddComponent(int skillID, string name)
    {
        GameObject obj = PhotonView.Find(skillID).gameObject;
        obj.name = name;

        if (name == "Zeus")
            obj.AddComponent<Zeus>();

        else if (name == "Poseidon")
            obj.AddComponent<Poseidon>();
    }
    public void SuperListCntPlus()
    {
        for (int i = 0; i < superList.Count; i++)
        {
            superList[i].TurnCntPlus();
            superList[i].ResetSkill();
        }
    }
    public void CheckSuperSkill()
    {
        whiteIcon.CheckSkill();
        blackIcon.CheckSkill();
    }
    public void RemoveSuperList(SkillBase sb)
    {
        superList.Remove(sb);
    }

    public string GetResponse(bool isWhite)
    {
        if (isWhite)
            return whiteGodsRes;
        else
            return blackGodsRes;
    }
}