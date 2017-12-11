using UnityEngine;
public class Portal_Audio : MonoBehaviour
{
    private AudioSource audioSource = null;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioLevels.Instance.OnEnvVolumeChanged += UpdateVolume;
        UpdateVolume();
    }
    private void OnDestroy() { try { AudioLevels.Instance.OnEnvVolumeChanged -= UpdateVolume; } catch { } }
    private void UpdateVolume() { audioSource.volume = AudioLevels.Instance.EnvVolume; }
}