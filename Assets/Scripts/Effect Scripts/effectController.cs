using UnityEngine;
using UnityEngine.SceneManagement;
public enum particleEffectTypesEnum { rain, snow, crash, sandDust, other }
public class effectController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] triggerParticleEffects = null;
    [SerializeField] private ParticleSystem dustField = null;
    private int particleType = 0;
    private const int particleLayer = 9;
    public ParticleSystem[] TriggerParticleEffects { get { return triggerParticleEffects; } }
    public ParticleSystem DustField { get { return dustField; } }
    public ParticleSystem getParticleToDo()
    {
        return triggerParticleEffects[particleType];
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += dustFieldActivation;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= dustFieldActivation;
    }
    private void Start()
    {
        disableAllEffects();
    }
    private void dustFieldActivation(Scene scene, LoadSceneMode loadMode)
    {
        if (SceneManager.GetActiveScene().buildIndex >= LevelSelectOptions.LevelBuildOffset)
            dustField.Play();
        else
            dustField.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (particleLayer == other.gameObject.layer)
        {
            particleEffectTypesEnum theEffect = other.GetComponent<effectZoneProperties>().myEffect;
            switch (theEffect)
            {
                case particleEffectTypesEnum.rain:
                    triggerParticleEffects[0].Play();
                    particleType = 0;
                    break;
                case particleEffectTypesEnum.snow:
                    triggerParticleEffects[1].Play();
                    particleType = 1;
                    break;
                case particleEffectTypesEnum.sandDust:
                    triggerParticleEffects[3].Play();
                    break;
                case particleEffectTypesEnum.other:
                    foreach (ParticleSystem triggerParticleEffect in triggerParticleEffects)
                        triggerParticleEffect.Play();
                    break;
                default:
                    break;
            }
        }
    }
    public void disableAllEffects()
    {
        foreach (ParticleSystem triggerParticleEffect in triggerParticleEffects)
            triggerParticleEffect.Stop();
    }
    private void OnTriggerExit(Collider other)
    {
        if (particleLayer == other.gameObject.layer)
        {
            particleEffectTypesEnum theEffect = other.GetComponent<effectZoneProperties>().myEffect;
            switch (theEffect)
            {
                case particleEffectTypesEnum.rain:
                    triggerParticleEffects[0].Stop();
                    break;
                case particleEffectTypesEnum.snow:
                    triggerParticleEffects[1].Stop();
                    break;
                case particleEffectTypesEnum.sandDust:
                    triggerParticleEffects[3].Stop();
                    break;
                case particleEffectTypesEnum.other:
                    foreach (ParticleSystem triggerParticleEffect in triggerParticleEffects)
                        triggerParticleEffect.Stop();
                    break;
                default:
                    break;
            }
        }
    }
}