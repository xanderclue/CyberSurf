using UnityEngine;
public class lightingUpHubWorld : MonoBehaviour
{
    private const float timeToMove = 20.0f;
    private float currentTime = 0.0f;
    private void Start()
    {
        currentTime = 0.0f;
        RenderSettings.fog = true;
    }
    private void Update()
    {
        if (!keepPlayerStill.tutorialOn)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.0f, currentTime / timeToMove);
            }
            else
                currentTime = 0.0f;
        }
    }
}