using UnityEngine;
public class crashEffectPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Material texture = collision.gameObject.GetComponent<MeshRenderer>().Null()?.material;
        if (null != texture)
        {
            ParticleSystem particleEffect = GetComponentInChildren<effectController>().TriggerParticleEffects[(int)particleEffectTypesEnum.crash];
            particleEffect.GetComponent<Renderer>().material = texture;
            particleEffect.GetComponent<Renderer>().materials[1] = texture;
            particleEffect.Play();
        }
    }
}