using UnityEngine;

[System.Serializable]
public class SuperSkillData
{
    public string skillName;
    public int amount;
    public bool isSelect;
}

[CreateAssetMenu(fileName = "SuperSkillSO", menuName = "Sprictable Object/SuperSkillSO")]
public class SuperSkillSO : ScriptableObject
{
    public SuperSkillData[] superSkills;
}