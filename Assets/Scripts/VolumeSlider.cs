using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    private Text sliderText;

    void Start()
    {
        User user = DataManager.Inst.LoadDataFromJson<User>();
        slider = GetComponent<Slider>();
        sliderText = GetComponentInChildren<Text>();

        Debug.Log(user.bgmVolume);
        Debug.Log(user.effectVolume);

        if (gameObject.name.Contains("BGM"))
        {
            Debug.Log(user.bgmVolume);
            slider.value = user.bgmVolume * 10;
        }

        else
        {
            Debug.Log(user.effectVolume);

            slider.value = user.effectVolume * 10;
        }

        sliderText.text = slider.value.ToString();
    }

    public void SetValue()
    {
        if (slider == null) return;

        if (gameObject.name.Contains("BGM"))
        {
            SoundManager.Instance.BGMVolume(slider);
        }

        else
        {
            SoundManager.Instance.EffectVolume(slider);
        }

        sliderText.text = slider.value.ToString();
    }
}
