using UnityEngine;
public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource = null;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            if (objs[0].GetComponent<AudioSource>().clip.name == objs[1].GetComponent<AudioSource>().clip.name)
                Destroy(gameObject);
            else
            {
                Destroy(objs[0]);
                DontDestroyOnLoad(audioSource);
            }
        }
    }
    private void OnEnable()
    {
        AudioLevels.Instance.OnBgmVolumeChanged += UpdateVolume;
        UpdateVolume();
    }
    private void OnDisable()
    {
        try { AudioLevels.Instance.OnBgmVolumeChanged -= UpdateVolume; } catch { }
    }
    private void UpdateVolume()
    {
        if (null != audioSource)
            audioSource.volume = AudioLevels.Instance.BgmVolume;
    }
}