using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    private string SAVE_PATH;

    private void Awake()
    {
        SAVE_PATH = Path.Combine(Application.persistentDataPath, "Save");

        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
    }
    public string SaveDataToJson<T>(T data, bool isSave)
    {
        SAVE_PATH = Path.Combine(Application.persistentDataPath, "Save");

        string jsonData = JsonUtility.ToJson(data, true);
        if (isSave)
        {
            string path = Path.Combine(SAVE_PATH, typeof(T).ToString() + ".json");
            File.WriteAllText(path, jsonData);
        }

        return jsonData;
    }

    public T LoadDataFromJson<T>(string jsonData = null)
    {
        SAVE_PATH = Path.Combine(Application.persistentDataPath, "Save");

        if (jsonData == null)
        {
            string path = Path.Combine(SAVE_PATH, typeof(T).ToString() + ".json");
            if (File.Exists(path))
                jsonData = File.ReadAllText(path);

        }
        return JsonUtility.FromJson<T>(jsonData);
    }
}
