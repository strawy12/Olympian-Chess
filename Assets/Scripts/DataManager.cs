using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private static DataManager inst;
    public static DataManager Inst
    {
        get
        {
            if (inst == null)
            {
                var obj = FindObjectOfType<DataManager>();
                if (obj != null)
                {
                    inst = obj;
                }
                else
                {
                    var newObj = new GameObject("DataManager").AddComponent<DataManager>();
                    inst = newObj;
                }
            }
            return inst;
        }
    }

    void SavePlayerDataToJson(PlayerData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.dataPath, "playerData.Json");
        File.WriteAllText(path, jsonData);
    }

    PlayerData LoadPlayerDataFromJson(PlayerData playerData)
    {
        string path = Path.Combine(Application.dataPath, "playerData.Json");
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<PlayerData>(jsonData);

    }

    void SavePostitonDataToJson(PositionData positionData)
    {
        string jsonData = JsonUtility.ToJson(positionData, true);
        string path = Path.Combine(Application.dataPath, "positionData.Json");
        File.WriteAllText(path, jsonData);
    }

    PositionData LoadPostitonDataFromJson(PositionData positionData)
    {
        string path = Path.Combine(Application.dataPath, "positionData.Json");
        string jsonData = File.ReadAllText(path);
        return JsonUtility.FromJson<PositionData>(jsonData);

    }
}
