using UnityEngine;
public class respawnAndDespawnSphere : MonoBehaviour
{
    private delegate void SphereStateChangeEvent(bool bValue);
    private static SphereStateChangeEvent OnSphereStateChange;
    public static bool SphereState { set { if (null != OnSphereStateChange) OnSphereStateChange(value); } }
    private void Awake()
    {
        OnSphereStateChange += gameObject.SetActive;
    }
    private void OnDestroy()
    {
        OnSphereStateChange -= gameObject.SetActive;
    }
}