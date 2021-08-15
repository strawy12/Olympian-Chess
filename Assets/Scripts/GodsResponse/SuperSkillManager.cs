using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSkillManager : MonoBehaviour
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
    [SerializeField] Sprite poseidon;

    public string whiteGodsRes = "Poseidon";
    public string blackGodsRes = "Poseidon";

    private void Start()
    {
        SetGodsResponse();
    }

    public void UsingSkill(MovePlate mp)
    {
        if (superList.Count < 1) return;

        SkillBase sb = superList[superList.Count - 1];
        sb.SetPosX(mp.GetPosX());
        sb.SetPosY(mp.GetPosY());
        sb.StandardSkill();

        CardManager.Inst.SetSelectCard(null);
    }

    public GameObject SpawnSkill()
    {
        GameObject obj = null;
        SkillBase sb;
        obj = Instantiate(skillPrefab, transform);
        obj.transform.SetParent(null);

        if (GameManager.Inst.GetCurrentPlayer() == "white")
            obj.name = whiteGodsRes;

        else
            obj.name = blackGodsRes;

        AddComponent(obj);
        sb = obj.GetComponent<SkillBase>();
        sb.UsingSkill();
        superList.Add(sb);
        return obj;
    }

    private void SetGodsResponse()
    {
        whiteGodsRes = "Zeus";
        SetSprite(whiteIcon, whiteGodsRes);
        blackGodsRes = "Poseidon";
        SetSprite(blackIcon, blackGodsRes);
    }

    private void SetSprite(SuperSkill icon, string response)
    {
        if (response == "Zeus")
        {
            icon.spriteRenderer.sprite = zeus;
        }

        else
        {
            icon.spriteRenderer.sprite = poseidon;
        }
    }

    private void AddComponent(GameObject obj)
    {
        if (obj.name == "Zeus")
            obj.AddComponent<Zeus>();

        else if (obj.name == "Poseidon")
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
}