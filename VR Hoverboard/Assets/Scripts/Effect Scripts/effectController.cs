using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum particleEffectTypesEnum { rain, snow, crash, other }

public class effectController : MonoBehaviour
{
    public ParticleSystem[] particleEffects;


    // Use this for initialization
    void Start()
    {
        disableAllEffects();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            particleEffectTypesEnum theEffect = other.gameObject.GetComponent<effectZoneProperties>().myEffect;
            switch (theEffect)
            {
                case particleEffectTypesEnum.rain:
                    particleEffects[0].Play();
                    break;
                case particleEffectTypesEnum.snow:
                    particleEffects[1].Play();
                    break;
                case particleEffectTypesEnum.other:
                    for (int i = 0; i < particleEffects.Length; i++)
                    {
                        particleEffects[i].Play();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void disableAllEffects()
    {
        for (int i = 0; i < particleEffects.Length; i++)
        {
            particleEffects[i].Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            particleEffectTypesEnum theEffect = other.gameObject.GetComponent<effectZoneProperties>().myEffect;
            switch (theEffect)
            {
                case particleEffectTypesEnum.rain:
                    particleEffects[0].Stop();
                    break;
                case particleEffectTypesEnum.snow:
                    particleEffects[1].Stop();
                    break;
                case particleEffectTypesEnum.other:
                    for (int i = 0; i < particleEffects.Length; i++)
                    {
                        particleEffects[i].Stop();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
