using UnityEngine;
using TMPro;
public class AudioMenuTab : MenuTab
{
    [SerializeField, Range(2, 1000), Tooltip("Number representing the maximum volume level")] private int maxNumber = 16;
    [Space, SerializeField, LabelOverride("BGM Volume Text")] private TextMeshPro bgmVolume = null;
    [SerializeField, LabelOverride("SFX Volume Text")] private TextMeshPro sfxVolume = null;
    [SerializeField, LabelOverride("ENV Volume Text")] private TextMeshPro envVolume = null;
    [Space, SerializeField] private EventSelectedObject bgmUpButton = null;
    [SerializeField] private EventSelectedObject bgmDownButton = null;
    [SerializeField] private EventSelectedObject sfxUpButton = null;
    [SerializeField] private EventSelectedObject sfxDownButton = null;
    [SerializeField] private EventSelectedObject envUpButton = null;
    [SerializeField] private EventSelectedObject envDownButton = null;
    private float volumeIncrement { get { return 1.0f / maxNumber; } }
    private void SetBgmText()
    {
        bgmVolume.SetText(Mathf.RoundToInt(AudioLevels.Instance.BgmVolume * maxNumber).ToString());
    }
    private void SetSfxText()
    {
        sfxVolume.SetText(Mathf.RoundToInt(AudioLevels.Instance.SfxVolume * maxNumber).ToString());
    }
    private void SetEnvText()
    {
        envVolume.SetText(Mathf.RoundToInt(AudioLevels.Instance.EnvVolume * maxNumber).ToString());
    }
    private void OnEnable()
    {
        AudioLevels.Instance.OnBgmVolumeChange += SetBgmText;
        AudioLevels.Instance.OnSfxVolumeChange += SetSfxText;
        AudioLevels.Instance.OnEnvVolumeChange += SetEnvText;
        bgmUpButton.OnSelectSuccess += BgmUp;
        bgmDownButton.OnSelectSuccess += BgmDown;
        sfxUpButton.OnSelectSuccess += SfxUp;
        sfxDownButton.OnSelectSuccess += SfxDown;
        envUpButton.OnSelectSuccess += EnvUp;
        envDownButton.OnSelectSuccess += EnvDown;
        SetBgmText();
        SetSfxText();
        SetEnvText();
    }
    private void OnDisable()
    {
        try
        {
            AudioLevels.Instance.OnBgmVolumeChange -= SetBgmText;
            AudioLevels.Instance.OnSfxVolumeChange -= SetSfxText;
            AudioLevels.Instance.OnEnvVolumeChange -= SetEnvText;
        }
        catch { }
        bgmUpButton.OnSelectSuccess -= BgmUp;
        bgmDownButton.OnSelectSuccess -= BgmDown;
        sfxUpButton.OnSelectSuccess -= SfxUp;
        sfxDownButton.OnSelectSuccess -= SfxDown;
        envUpButton.OnSelectSuccess -= EnvUp;
        envDownButton.OnSelectSuccess -= EnvDown;
    }
    private void BgmUp()
    {
        AudioLevels.Instance.BgmVolume += volumeIncrement;
    }
    private void BgmDown()
    {
        AudioLevels.Instance.BgmVolume -= volumeIncrement;
    }
    private void SfxUp()
    {
        AudioLevels.Instance.SfxVolume += volumeIncrement;
    }
    private void SfxDown()
    {
        AudioLevels.Instance.SfxVolume -= volumeIncrement;
    }
    private void EnvUp()
    {
        AudioLevels.Instance.EnvVolume += volumeIncrement;
    }
    private void EnvDown()
    {
        AudioLevels.Instance.EnvVolume -= volumeIncrement;
    }
}