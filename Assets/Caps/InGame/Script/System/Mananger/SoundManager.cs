using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum VolumeType
{
    masterVol, bgmVol, sfxVol
}

public enum BGM
{
    Main, NoneBattle, Battle, Boss, GameOver
}

public enum SFX
{
    Walk, Roll, Hit, AK_Reload, M870_Reload, AWP_Reload, Eagle_Reload, Anaconda_Reload,
    Knife_Shot, Punch_Shot, AK_Shot, M870_Shot, AWP_Shot, Eagle_Shot, Anaconda_Shot, 
    SoldOut, DrugPickUp, ItemPickUp, MoneyPickUp, UseBand, UseBFS, UseDrug, UseGaugeLock, UseGrenade, UseKey, UseMoney,
    Box_Open, Door_Open, Table_Kick, DiaryOpen, DiaryClose
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;

    public AudioMixer mixer;


    [Header("Sounds")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    [Header("AudioSource")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public bool[] toggleCheck;

    private void Awake()
    {
        Init();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        if (Instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public float GetVolume(VolumeType volType)
    {
        float volume;
        mixer.GetFloat(volType.ToString(), out volume);

        return volume;
    }

    public void SetVolume(Slider slider, VolumeType volType)
    {
        mixer.SetFloat(volType.ToString(), slider.value);

        if (slider.value == slider.minValue)
        {
            mixer.SetFloat(volType.ToString(), -80);
        }
    }

    public bool GetToggle(int toggleNum)
    {
        return toggleCheck[toggleNum];
    }

    public void SetToggle(Toggle[] toggles)
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggleCheck[i] = toggles[i].isOn;
        }
    }

    /*public void PlaySFX(SFX sfx)
    {
        sfxSource.clip = sfxClips[(int)sfx];
        sfxSource.Play();
    }*/
    public void PlaySFX(SFX sfx)
    {
        //sfxSource.clip = sfxClips[(int)sfx];
        //sfxSource.pitch = 1;
        sfxSource.PlayOneShot(sfxClips[(int)sfx]);
    }


    /*public void PlaySFX(SFX sfx, float pitch)
    {
        sfxSource.clip = sfxClips[(int)sfx];
        sfxSource.pitch = pitch;
        sfxSource.Play();
    }*/

    public void PlayBGM(BGM bgm)
    {
        bgmSource.clip = bgmClips[(int)bgm];
        bgmSource.Play();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }
}
