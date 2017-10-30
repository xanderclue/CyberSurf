using UnityEngine;
public class AudioLevels : MonoBehaviour
{
    private static AudioLevels instance = null;
    private static bool applicationRunning = true;
    public static AudioLevels Instance
    {
        get
        {
            if (null == instance && applicationRunning)
                instance = new GameObject("AudioManager").AddComponent<AudioLevels>();
            return instance;
        }
    }
    public delegate void SettingChangedEvent();
    public event SettingChangedEvent OnBgmVolumeChange, OnSfxVolumeChange, OnEnvVolumeChange;
    private float bgmVolume = 1.0f, sfxVolume = 1.0f, envVolume = 1.0f;
    public float BgmVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = Mathf.Clamp01(value);
            OnBgmVolumeChange?.Invoke();
        }
    }
    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = Mathf.Clamp01(value);
            OnSfxVolumeChange?.Invoke();
        }
    }
    public float EnvVolume
    {
        get { return envVolume; }
        set
        {
            envVolume = Mathf.Clamp01(value);
            OnEnvVolumeChange?.Invoke();
        }
    }
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (this != instance)
            Destroy(this);
    }
    private void OnEnable()
    {
        GameSettings.GetFloat("BgmVolume", ref bgmVolume);
        GameSettings.GetFloat("SfxVolume", ref sfxVolume);
        GameSettings.GetFloat("EnvVolume", ref envVolume);
        OnBgmVolumeChange?.Invoke();
        OnSfxVolumeChange?.Invoke();
        OnEnvVolumeChange?.Invoke();
    }
    private void OnDisable()
    {
        if (this == instance)
        {
            GameSettings.SetFloat("BgmVolume", bgmVolume);
            GameSettings.SetFloat("SfxVolume", sfxVolume);
            GameSettings.SetFloat("EnvVolume", envVolume);
        }
    }
    private void OnApplicationQuit() { applicationRunning = false; }
}