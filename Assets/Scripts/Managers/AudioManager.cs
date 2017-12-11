using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public delegate void VolumeChangedEvent();
    public static event VolumeChangedEvent OnBgmVolumeChanged, OnSfxVolumeChanged, OnEnvVolumeChanged;
    private static float bgmVolume = 1.0f, sfxVolume = 1.0f, envVolume = 1.0f;
    public static float BgmVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = Mathf.Clamp01(value);
            OnBgmVolumeChanged?.Invoke();
        }
    }
    public static float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            OnSfxVolumeChanged?.Invoke();
        }
    }
    public static float EnvVolume
    {
        get { return envVolume; }
        set
        {
            envVolume = Mathf.Clamp01(value);
            OnEnvVolumeChanged?.Invoke();
        }
    }
    private void OnEnable()
    {
        GameSettings.GetFloat("BgmVolume", ref bgmVolume);
        GameSettings.GetFloat("SfxVolume", ref sfxVolume);
        GameSettings.GetFloat("EnvVolume", ref envVolume);
        OnBgmVolumeChanged?.Invoke();
        OnSfxVolumeChanged?.Invoke();
        OnEnvVolumeChanged?.Invoke();
    }
    private void OnDisable()
    {
        GameSettings.SetFloat("BgmVolume", bgmVolume);
        GameSettings.SetFloat("SfxVolume", sfxVolume);
        GameSettings.SetFloat("EnvVolume", envVolume);
    }
}