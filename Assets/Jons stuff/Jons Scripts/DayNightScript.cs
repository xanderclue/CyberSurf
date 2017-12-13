using UnityEngine;
public class DayNightScript : MonoBehaviour
{
    public enum TimeOfDay { Day, Night, NumTimesOfDay }
    public static TimeOfDay currentTimeOfDay = TimeOfDay.Day;
    [SerializeField] private Light sun = null;
    [SerializeField] private ParticleSystem stars = null;
    private void Start()
    {
        ParticleSystem.MainModule starsMain = stars.main;
        RenderSettings.sun = sun;
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Day:
                sun.transform.localRotation = Quaternion.Euler(73.0f, 0.0f, 0.0f);
                sun.enabled = true;
                starsMain.maxParticles = 1;
                RenderSettings.ambientLight = new Color(0.25f, 0.25f, 0.25f);
                RenderSettings.skybox.SetFloat("_AtmosphereThickness", 1.0f);
                break;
            case TimeOfDay.Night:
                sun.enabled = false;
                starsMain.maxParticles = 2000;
                RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.65f);
                RenderSettings.skybox = null;
                break;
        }
        Destroy(this);
    }
}