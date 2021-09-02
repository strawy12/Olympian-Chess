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
        User user = NetworkManager.Inst.LoadDataFromJson<User>();
        slider = GetComponent<Slider>();
        sliderText = GetComponentInChildren<Text>();

        if (gameObject.name.Contains("BGM"))
        {
            slider.value = user.bgmVolume * 10;
        }

        else
        {
            slider.value = user.effectVolume * 10;
            sliderText.text = slider.value.ToString();
        }

        sliderText.text = slider.value.ToString();
    }

    public void SetValue()
    {
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
