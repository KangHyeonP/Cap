using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Slider[] sliders; //0: ALL, 1: BGM, 2: SFX
    [SerializeField]
    private Toggle[] toggles;

    private bool isUpdating = false;

    public bool uiManagerCheckc = false;

    private void OnDisable()
    {
        if (uiManagerCheckc) Close();
    }

    private void Start()
    {
        Init();

        if (UIManager.Instance != null) uiManagerCheckc = true;
        else uiManagerCheckc = false;
    }

    public void Open()
    {
        Debug.Log("üũ");
        UIManager.Instance.TempUI = this.gameObject;
        UIManager.Instance.IsPopup++;
    }

    public void Close()
    {
        if (uiManagerCheckc)
        {
            UIManager.Instance.pauseUI.SelectButtonOn();
            UIManager.Instance.IsPopup--;
        }
    }

    private void Init()
    {
        isUpdating = true;
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = SoundManager.Instance.GetVolume((VolumeType)i);
            toggles[i].isOn = SoundManager.Instance.GetToggle(i);
        }
        isUpdating = false;
    }

    public void SliderUpdate(int volNum)
    {
        SoundManager.Instance.SetVolume(sliders[volNum], (VolumeType)volNum);
    }

    public void ToggleUpdate(int volNum)
    {
        if (isUpdating)
            return;

        isUpdating = true;

        switch (volNum)
        {
            case 0:
                if (toggles[volNum].isOn)
                {
                    SoundManager.Instance.sfxSource.mute = false;
                    if(!SoundManager.Instance.bgmSource.isPlaying)
                        SoundManager.Instance.bgmSource.Play();
                    toggles[1].isOn = true;
                    toggles[2].isOn = true;
                }
                else
                {
                    SoundManager.Instance.sfxSource.mute = true;
                    SoundManager.Instance.bgmSource.Pause();
                    toggles[1].isOn = false;
                    toggles[2].isOn = false;
                }
                break;
            case 1:
                if (toggles[volNum].isOn)
                {
                    SoundManager.Instance.bgmSource.Play();
                    if (toggles[2].isOn)
                    {
                        toggles[0].isOn = true;
                    }
                }
                else
                {
                    SoundManager.Instance.bgmSource.Pause();
                    toggles[0].isOn = false;
                }
                break;
            case 2:
                if (toggles[volNum].isOn)
                {
                    SoundManager.Instance.sfxSource.mute = false;
                    if (toggles[1].isOn)
                    {
                        toggles[0].isOn = true;
                    }
                }
                else
                {
                    SoundManager.Instance.sfxSource.mute = true;
                    toggles[0].isOn = false;
                }
                break;
        }
        SoundManager.Instance.SetToggle(toggles);
        isUpdating = false;
    }

    public void SetLanguage(int index)
    {

        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];
    }
    /*
    public enum ELanguage 
    { 
    Korean, English
    }
    public ELanguage languageValue;
    Locale currentSelectedLocale;
     currentSelectedLocale = LocalizationSettings.SelectedLocale;
        languageValue = (ELanguage)LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        Debug.Log("languageIndex : " + languageValue);

    languageValue = (ELanguage)num; 

        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[num];
     */
}
