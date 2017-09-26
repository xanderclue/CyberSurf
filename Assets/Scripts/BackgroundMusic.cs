using UnityEngine;
public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource = null;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        UpdateVolume();
    }
    private void OnEnable()
    {
        AudioLevels.Instance.OnBgmVolumeChange += UpdateVolume;
    }
    private void OnDisable()
    {
        try { AudioLevels.Instance.OnBgmVolumeChange -= UpdateVolume; } catch { }
    }
    private void UpdateVolume()
    {
        if (null != audioSource)
            audioSource.volume = AudioLevels.Instance.BgmVolume;
    }
}