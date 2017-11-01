using UnityEngine;
public class LetThereBeNoLight : MonoBehaviour
{
    private void Start()
    {
        Light[] all = FindObjectsOfType<Light>();
        foreach (Light i in all)
            i.color = Color.clear;
        Destroy(this);
    }
}