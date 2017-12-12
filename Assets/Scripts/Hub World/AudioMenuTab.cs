using UnityEngine;
using TMPro;
public class AudioMenuTab : MenuTab
{
    [SerializeField, Range(2, 1000), Tooltip("Number representing the maximum volume level")] private int maxNumber = 16;
    [Space, SerializeField, LabelOverride("BGM Volume Text")] private TextMeshPro bgmVolume = null;
    [SerializeField, LabelOverride("SFX Volume Text")] private TextMeshPro sfxVolume = null;
    [SerializeField, LabelOverride("ENV Volume Text")] private TextMeshPro envVolume = null;
    [SerializeField] private EventSelectedObject bgmUpButton = null, bgmDownButton = null, sfxUpButton = null, sfxDownButton = null, envUpButton = null, envDownButton = null;
    private float VolumeIncrement => 1.0f / maxNumber;
    private void SetBgmText() => bgmVolume.SetText(Mathf.RoundToInt(AudioManager.BgmVolume * maxNumber).ToString());
    private void SetSfxText() => sfxVolume.SetText(Mathf.RoundToInt(AudioManager.SfxVolume * maxNumber).ToString());
    private void SetEnvText() => envVolume.SetText(Mathf.RoundToInt(AudioManager.EnvVolume * maxNumber).ToString());
    private void OnEnable()
    {
        AudioManager.OnBgmVolumeChanged += SetBgmText;
        AudioManager.OnSfxVolumeChanged += SetSfxText;
        AudioManager.OnEnvVolumeChanged += SetEnvText;
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
        AudioManager.OnBgmVolumeChanged -= SetBgmText;
        AudioManager.OnSfxVolumeChanged -= SetSfxText;
        AudioManager.OnEnvVolumeChanged -= SetEnvText;
        bgmUpButton.OnSelectSuccess -= BgmUp;
        bgmDownButton.OnSelectSuccess -= BgmDown;
        sfxUpButton.OnSelectSuccess -= SfxUp;
        sfxDownButton.OnSelectSuccess -= SfxDown;
        envUpButton.OnSelectSuccess -= EnvUp;
        envDownButton.OnSelectSuccess -= EnvDown;
    }
    private void BgmUp() => AudioManager.BgmVolume += VolumeIncrement;
    private void BgmDown() => AudioManager.BgmVolume -= VolumeIncrement;
    private void SfxUp() => AudioManager.SfxVolume += VolumeIncrement;
    private void SfxDown() => AudioManager.SfxVolume -= VolumeIncrement;
    private void EnvUp() => AudioManager.EnvVolume += VolumeIncrement;
    private void EnvDown() => AudioManager.EnvVolume -= VolumeIncrement;
}