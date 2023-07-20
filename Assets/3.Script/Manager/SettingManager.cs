using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public AudioSource BGM;
    public AudioSource SFX;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        transform.gameObject.SetActive(false);
        // BGM �����̴��� �̺�Ʈ�� SetBGM �޼��� ����
        bgmSlider.onValueChanged.AddListener(SetBGM);
        // SFX �����̴��� �̺�Ʈ�� SetSFX �޼��� ����
        sfxSlider.onValueChanged.AddListener(SetSFX);
    }
    public void SetBGM(float sliderValue)
    {
        BGM.volume = sliderValue;
    }
    public void SetSFX(float sliderValue)
    {
        SFX.volume = sliderValue;
    }
    public void Return()
    {
        AudioManager.instance.PlaySFX("UIClick");
        transform.gameObject.SetActive(false);
    }

}
