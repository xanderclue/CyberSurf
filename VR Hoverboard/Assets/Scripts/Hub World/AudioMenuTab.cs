using UnityEngine;
using TMPro;
public class AudioMenuTab : MenuTab
{
    [SerializeField]
    private TextMeshPro bgmVolume = null, sfxVolume = null, ambVolume = null;
    private void SetBgmText()
    {
        if (null != bgmVolume)
            bgmVolume.SetText(Mathf.RoundToInt(AudioLevels.Instance.BgmVolume * 16.0f).ToString());
    }
    private void SetSfxText()
    {
        if (null != sfxVolume)
            sfxVolume.SetText(Mathf.RoundToInt(AudioLevels.Instance.SfxVolume * 16.0f).ToString());
    }
    private void SetAmbText()
    {
        if (null != ambVolume)
            ambVolume.SetText(Mathf.RoundToInt(AudioLevels.Instance.AmbVolume * 16.0f).ToString());
    }
    private void OnEnable()
    {
        AudioLevels.Instance.OnBgmVolumeChange += SetBgmText;
        AudioLevels.Instance.OnSfxVolumeChange += SetSfxText;
        AudioLevels.Instance.OnAmbVolumeChange += SetAmbText;
        SetBgmText();
        SetSfxText();
        SetAmbText();
    }
    private void OnDisable()
    {
        try
        {
            AudioLevels.Instance.OnBgmVolumeChange -= SetBgmText;
            AudioLevels.Instance.OnSfxVolumeChange -= SetSfxText;
            AudioLevels.Instance.OnAmbVolumeChange -= SetAmbText;
        }
        catch { }
    }
}