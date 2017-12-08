using UnityEngine;
using Xander.NullConversion;
public class WeatherScript : MonoBehaviour
{
    public enum Weather { Sunny, Rainy, Snowy, NumWeathers }
    public static Weather currentWeather = Weather.Sunny;
    private static ParticleSystem playerRain = null, playerSnow = null;
    [SerializeField] private ParticleSystem worldSnow = null, worldCloud = null;
    private void Start()
    {
        if (null == playerRain) playerRain = GameObject.FindGameObjectWithTag("rain").ConvertNull()?.GetComponent<ParticleSystem>();
        if (null == playerSnow) playerSnow = GameObject.FindGameObjectWithTag("playerSnow").ConvertNull()?.GetComponent<ParticleSystem>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex < LevelSelectOptions.LevelBuildOffset)
        {
            playerRain.Stop();
            playerSnow.Stop();
            Destroy(gameObject);
        }
        else
        {
            switch (currentWeather)
            {
                case Weather.Rainy:
                    playerRain.Play();
                    worldCloud.Play();
                    break;
                case Weather.Snowy:
                    playerSnow.Play();
                    worldSnow.Play();
                    worldCloud.Play();
                    break;
                default:
                    playerRain.Stop();
                    playerSnow.Stop();
                    worldSnow.Stop();
                    worldCloud.Stop();
                    break;
            }
            Destroy(this);
        }
    }
}