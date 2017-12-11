using UnityEngine;
using TMPro;
public class AudioMenuTab : MenuTab
{
    [SerializeField, Range(2, 1000), Tooltip("Number representing the maximum volume level")] private int maxNumber = 16;
    [Space, SerializeField, LabelOverride("BGM Volume Text")] private TextMeshPro bgmVolume = null;
    [SerializeField, LabelOverride("SFX Volume Text")] private TextMeshPro sfxVolume = null;
    [SerializeField, LabelOverride("ENV Volume Text")] private TextMeshPro envVolume = null;
    [SerializeField] private EventSelectedObject bgmUpButton = null, bgmDownButton = null, sfxUpButton = null, sfxDownButton = null, envUpButton = null, envDownButton = null;
    private float VolumeIncrement { get { return 1.0f / maxNumber; } }
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
        AudioLevels.Instance.OnBgmVolumeChanged += SetBgmText;
        AudioLevels.Instance.OnSfxVolumeChanged += SetSfxText;
        AudioLevels.Instance.OnEnvVolumeChanged += SetEnvText;
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
            AudioLevels.Instance.OnBgmVolumeChanged -= SetBgmText;
            AudioLevels.Instance.OnSfxVolumeChanged -= SetSfxText;
            AudioLevels.Instance.OnEnvVolumeChanged -= SetEnvText;
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
        AudioLevels.Instance.BgmVolume += VolumeIncrement;
    }
    private void BgmDown()
    {
        AudioLevels.Instance.BgmVolume -= VolumeIncrement;
    }
    private void SfxUp()
    {
        AudioLevels.Instance.SfxVolume += VolumeIncrement;
    }
    private void SfxDown()
    {
        AudioLevels.Instance.SfxVolume -= VolumeIncrement;
    }
    private void EnvUp()
    {
        AudioLevels.Instance.EnvVolume += VolumeIncrement;
    }
    private void EnvDown()
    {
        AudioLevels.Instance.EnvVolume -= VolumeIncrement;
    }
}