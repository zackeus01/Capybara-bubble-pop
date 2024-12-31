using System.Collections;
using UnityEngine;


public class SoundManager : Singleton<SoundManager>
{
    public static SoundManager instance;

    [SerializeField] private Sound[] sounds;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioSource BGMSource => bgmSource;
    public AudioSource SFXSource => sfxSource;

    private void Start()
    {
        LoadData();
        PlayBGM(SoundKey.Ingame);
        Debug.Log("Sound");

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Sound1");

            PlayOneShotSFX(SoundKey.Click);
        }
    }

    public void SetBGMVolume(float value)
    {
        Debug.Log("Sound2");

        bgmSource.volume = value;
        PlayerPrefs.SetFloat(PlayerPrefsConst.VOLUMEBGM, value);
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat(PlayerPrefsConst.VOLUMESFX, value);
    }

    public void ToggleBGMMute()
    {
        bgmSource.mute = !bgmSource.mute;
        PlayerPrefs.SetInt(PlayerPrefsConst.MUTEBGM, bgmSource.mute ? 1 : 0);
    }

    public void TogggleSFXMute()
    {
        sfxSource.mute = !sfxSource.mute;
        PlayerPrefs.SetInt(PlayerPrefsConst.MUTESFX, sfxSource.mute ? 1 : 0);
    }

    private AudioClip GetSoundClip(SoundKey soundKey)
    {
        foreach (var sound in sounds)
        {
            if (sound.soundKey == soundKey)
            {
                Debug.Log("Sound5");

                return sound.soundClip;
            }
        }
        return null;
    }

    public void PlayBGM(SoundKey soundKey)
    {
        AudioClip clip = GetSoundClip(soundKey);
        if (bgmSource != null && clip != null)
        {
            Debug.Log("Sound4");

            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayOneShotSFX(SoundKey soundKey)
    {
        AudioClip clip = GetSoundClip(soundKey);
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySFXFrom(SoundKey soundKey, float time)
    {
        AudioClip clip = GetSoundClip(soundKey);
        if (sfxSource != null && clip != null)
        {
            sfxSource.clip = clip;
            sfxSource.time = time;
            sfxSource.Play();
        }
    }

    public void PlaySFXDuration(SoundKey soundKey, float duration)
    {
        AudioClip clip = GetSoundClip(soundKey);
        if (sfxSource != null && clip != null)
        {
            sfxSource.clip = clip;
            sfxSource.Play();
            StartCoroutine(StopAudioAfterDuration(sfxSource, duration));
        }
    }

    private IEnumerator StopAudioAfterDuration(AudioSource audioSource, float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }

    private void LoadData()
    {
        bgmSource.mute = PlayerPrefs.GetInt(PlayerPrefsConst.MUTEBGM) == 1;
        sfxSource.mute = PlayerPrefs.GetInt(PlayerPrefsConst.MUTESFX) == 1;
        Debug.Log("Sound3");

        SetBGMVolume(PlayerPrefs.GetFloat(PlayerPrefsConst.VOLUMEBGM, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(PlayerPrefsConst.VOLUMESFX, 1f));
    }
}