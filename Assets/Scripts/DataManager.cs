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


}
