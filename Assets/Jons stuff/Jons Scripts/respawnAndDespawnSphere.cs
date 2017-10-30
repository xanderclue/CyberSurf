using UnityEngine;
public class respawnAndDespawnSphere : MonoBehaviour
{
    private delegate void SphereStateChangeEvent(bool bValue);
    private static event SphereStateChangeEvent OnSphereStateChange;
    public static bool SphereState { set { OnSphereStateChange?.Invoke(value); } }
    private void Awake()
    {
        OnSphereStateChange += gameObject.SetActive;
    }
    private void OnDestroy()
    {
        OnSphereStateChange -= gameObject.SetActive;
    }
}