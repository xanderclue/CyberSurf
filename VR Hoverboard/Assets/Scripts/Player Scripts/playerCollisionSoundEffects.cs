using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCollisionSoundEffects : MonoBehaviour
{
    private AudioSource source;
    PlayerRespawn respawnScript;

    [SerializeField] AudioClip wallCollision = null;
    [SerializeField] AudioClip ringCollision = null;
    [SerializeField] AudioClip portalEnter = null;

    GameObject prevRingObject;

    void Start()
    {
        source = GetComponent<AudioSource>();

        respawnScript = gameObject.GetComponent<PlayerRespawn>();

        prevRingObject = null;

        AudioLevels.Instance.OnSfxVolumeChange += UpdateVolume;
        UpdateVolume();
    }
    private void OnDestroy() { try { AudioLevels.Instance.OnSfxVolumeChange -= UpdateVolume; } catch { } }
    private void UpdateVolume() { source.volume = AudioLevels.Instance.SfxVolume; }
    private void OnCollisionEnter(Collision collision)
    {
        source.clip = wallCollision;

        source.Play();
    }

    //called by RingEffects.cs
    public void PlayRingClip(GameObject ringObject)
    {
        if (ringObject != prevRingObject && !respawnScript.IsRespawning)
        {
            source.clip = ringCollision;
            source.Play();
            prevRingObject = ringObject;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Portal" && !respawnScript.IsRespawning)
        {
            source.clip = portalEnter;
            source.Play();
        }
    }

}
