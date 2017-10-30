using UnityEngine;
public class dimSpotlight : MonoBehaviour
{
    private Light spotlight = null;
    private float currentTime = 0.0f;
    private const float timeToPlay = 15.0f;
    private void Start()
    {
        currentTime = 0.0f;
        spotlight = GetComponent<Light>();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (keepPlayerStill.tutorialOn)
        {
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, 50.0f, currentTime / timeToPlay);
            if (50.0f == spotlight.intensity)
                currentTime = 0.0f;
        }
        else
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, 0.0f, currentTime / timeToPlay);
    }
}