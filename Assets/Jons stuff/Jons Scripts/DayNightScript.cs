using UnityEngine;
public class DayNightScript : MonoBehaviour
{
    public enum TimeOfDay { Day, Night, NumTimesOfDay }
    public static TimeOfDay currentTimeOfDay = TimeOfDay.Day;
    [SerializeField] private Light sun = null;
    [SerializeField] private ParticleSystem stars = null;
    private void Start()
    {
        //ParticleSystem.Particle[] particleArray = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
        //particleArray[0].startSize = 2.0f;
        //
        //
        //ParticleSystem.MainModule starsMain = stars.main;
        //stars.GetParticles(particleArray);
        //
        //stars.SetParticles(particleArray, particleArray.Length);
        RenderSettings.sun = sun;
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Day:
                sun.transform.localRotation = Quaternion.Euler(73.0f, 0.0f, 0.0f);
                sun.enabled = true;
                stars.maxParticles = 1;
                starsMain.maxParticles = 1;
                RenderSettings.ambientLight = Color.black;
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