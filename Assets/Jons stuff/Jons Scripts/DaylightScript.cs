using UnityEngine;
using UnityEngine.SceneManagement;
using Xander.Debugging;
public class DaylightScript : MonoBehaviour
{
    public enum TimeOfDay { Noon, Afternoon, Evening, Midnight, Morning, NumTimesOfDay }
    [SerializeField] private Light sun = null;
    private Material ogSkybox = null;
    private ParticleSystem.MainModule stars;
    private int currentScene = -1;
    public static TimeOfDay currentTimeOfDay = TimeOfDay.Noon;
    private void Start()
    {
        if (null == sun)
            sun = GameObject.FindGameObjectWithTag("sun").GetComponent<Light>();
        ogSkybox = RenderSettings.skybox;

        stars = GameObject.FindGameObjectWithTag("stars").GetComponent<ParticleSystem>().main;
    }
    private void OnEnable()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += GetCurrentScene;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= GetCurrentScene;
    }
    private void GetCurrentScene(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
    }
    private void Update()
    {
        if (currentScene >= LevelSelectOptions.LevelBuildOffset)
        {
            switch (currentTimeOfDay)
            {
                case TimeOfDay.Noon:
                    {
                        sun = GameObject.FindGameObjectWithTag("sun").GetComponent<Light>();
                        RenderSettings.sun = sun;
                        sun.transform.localRotation = Quaternion.Euler(73.0f, 0.0f, 0.0f);
                       // sun.SetActive(true);
                        sun.enabled = true;
                        stars.maxParticles = 1;
                        RenderSettings.ambientLight = new Color(.75f, .75f, .75f);
                        RenderSettings.skybox = ogSkybox;
                        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 1.0f);

                    }
                    break;
                case TimeOfDay.Afternoon:
                    {
                        sun = GameObject.FindGameObjectWithTag("sun").GetComponent<Light>();
                        RenderSettings.sun = sun;

                        sun.transform.localRotation = Quaternion.Euler(120.0f, 0.0f, 0.0f);
                        //sun.SetActive(true);
                        sun.enabled = true;

                        stars.maxParticles = 1;
                        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f);
                        RenderSettings.skybox = ogSkybox;
                        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 1.1f);

                    }
                    break;
                case TimeOfDay.Evening:
                    {

                        sun = GameObject.FindGameObjectWithTag("eveningSun").GetComponent<Light>();
                        RenderSettings.sun = sun;

                        sun.transform.localRotation = Quaternion.Euler(166.24f, 0.0f, 0.0f);
                        // sun.SetActive(true);
                        //sun.enabled = true;

                        stars.maxParticles = 1;
                        RenderSettings.ambientLight = new Color(1.0f, 0.75f, 1.0f);
                        RenderSettings.skybox = ogSkybox;
                          RenderSettings.skybox.SetFloat("_Exposure", Mathf.Sin(Time.deltaTime * Mathf.Deg2Rad * 100) + 1);
                        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 1.25f);
                        
                        //  RenderSettings.skybox.SetFloat("_Exposure", 1000);

                    }
                    break;
                case TimeOfDay.Midnight:
                    {
                        // sun.SetActive(false);
                        sun = GameObject.FindGameObjectWithTag("sun").GetComponent<Light>();
                        RenderSettings.sun = sun;

                        sun.enabled = false;
                        stars.maxParticles = 2000;
                        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.75f);
                        RenderSettings.skybox = null;

                    }
                    break;
                case TimeOfDay.Morning:
                    {
                        sun = GameObject.FindGameObjectWithTag("sun").GetComponent<Light>();
                        RenderSettings.sun = sun;

                        sun.transform.localRotation = Quaternion.Euler(36.0f, 0.0f, 0.0f);
                        //  sun.SetActive(true);
                        sun.enabled = true;
                        stars.maxParticles = 1;
                        RenderSettings.skybox = ogSkybox;
                        RenderSettings.ambientLight = new Color(1.0f, 1.0f, 1.0f);
                        RenderSettings.skybox.SetFloat("_AtmosphereThickness", 0.9f);
                        
                    }
                    break;
                default:
                    Debug.LogWarning("bad" + this.Info(), this);
                    break;
            }
        }
    }
}