using UnityEngine;
public class FadeObjectScript : MonoBehaviour
{
    private Color color, invis;
    private const float howFarToFade = 15.0f;
    private Renderer objectRenderer = null;
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        color = objectRenderer.material.color;
        invis = color;
        invis.a = 0.0f;
    }
    private void Update()
    {
        float distance = (Camera.main.transform.position - transform.position).magnitude;
        if (distance < howFarToFade)
            objectRenderer.material.color = Color.Lerp(color, invis, (howFarToFade * 0.5f) / distance);
    }
}