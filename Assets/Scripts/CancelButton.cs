using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField]
    private Slider bgm, effect;

    public void OnPointerUp(PointerEventData eventData)
    {
        SaveSound();
    }

    public void SaveSound()
    {
        User user = DataManager.Inst.LoadDataFromJson<User>();
        user.bgmVolume = bgm.value / 10;
        user.effectVolume = effect.value / 10;
        DataManager.Inst.SaveDataToJson(user, true);
    }
}
