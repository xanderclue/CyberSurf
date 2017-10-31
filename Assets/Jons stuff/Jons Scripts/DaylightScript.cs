using UnityEngine;
using UnityEngine.SceneManagement;
using Xander.Debugging;
public class DaylightScript : MonoBehaviour
{
    public enum TimeOfDay { Noon, Afternoon, Evening, Midnight, Morning, NumTimesOfDay }
    [SerializeField] private GameObject sun = null;
    private Material ogSkybox = null;
    private ParticleSystem.MainModule stars;
    private int currentScene = -1;
    public static TimeOfDay currentTimeOfDay = TimeOfDay.Noon;
    private void Start()
    {
        if (null == sun)
            sun = GameObject.FindGameObjectWithTag("sun");
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
                        sun.transform.localRotation = Quaternion.Euler(73.0f, 0.0f, 0.0f);
                        sun.SetActive(true);
                        stars.maxParticles = 1;
                        RenderSettings.ambientLight = new Color(1.0f, 1.0f, 0.5f);
                        RenderSettings.skybox = ogSkybox;
                    }
                    break;
                case TimeOfDay.Afternoon:
                    {
                        sun.transform.localRotation = Quaternion.Euler(120.0f, 0.0f, 0.0f);
                        sun.SetActive(true);
                        stars.maxParticles = 1;
                        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f);
                        RenderSettings.skybox = ogSkybox;
                    }
                    break;
                case TimeOfDay.Evening:
                    {
                        sun.transform.localRotation = Quaternion.Euler(166.24f, 0.0f, 0.0f);
                        sun.SetActive(true);
                        stars.maxParticles = 1;
                        RenderSettings.ambientLight = new Color(0.75f, 0.75f, 0.75f);
                        RenderSettings.skybox = ogSkybox;
                    }
                    break;
                case TimeOfDay.Midnight:
                    {
                        sun.SetActive(false);
                        stars.maxParticles = 2000;
                        RenderSettings.ambientLight = Color.gray;
                        RenderSettings.skybox = null;
                    }
                    break;
                case TimeOfDay.Morning:
                    {
                        sun.transform.localRotation = Quaternion.Euler(36.0f, 0.0f, 0.0f);
                        sun.SetActive(true);
                        stars.maxParticles = 1;
                        RenderSettings.ambientLight = Color.gray;
                        RenderSettings.skybox = ogSkybox;
                    }
                    break;
                default:
                    Debug.LogWarning("bad" + this.Info(), this);
                    break;
            }
        }
    }
}