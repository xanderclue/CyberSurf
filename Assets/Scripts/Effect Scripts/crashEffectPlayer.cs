using UnityEngine;
public class crashEffectPlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        MeshRenderer theMesh = collision.gameObject.GetComponent<MeshRenderer>();
        if (null != theMesh)
        {
            Material texture = theMesh.material;
            ParticleSystem particleEffect = GetComponentInChildren<effectController>().TriggerParticleEffects[(int)particleEffectTypesEnum.crash];
            particleEffect.GetComponent<Renderer>().material = texture;
            particleEffect.GetComponent<Renderer>().materials[1] = texture;
            particleEffect.Play();
        }
    }
}