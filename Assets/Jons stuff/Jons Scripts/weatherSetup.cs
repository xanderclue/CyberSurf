using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class weatherSetup : MonoBehaviour
{

    private GameObject rainSystem;
    //private GameObject player;
    private GameObject rain;
    private GameObject worldSnow;
    private GameObject playerSnow;
   private GameObject cloud;
    // Use this for initialization
    void Start()
    {
        worldSnow = GameObject.FindGameObjectWithTag("worldSnow");
        playerSnow = GameObject.FindGameObjectWithTag("playerSnow");

        rain = GameObject.FindGameObjectWithTag("rain");

        cloud = GameObject.FindGameObjectWithTag("cloud");
    }

    void getCorrectWeather()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            switch ((int)WeatherOptions.ActualWeather)
            {
                //sunny
                case 0:
                    //take away any particle systems besides dust 
                    rain.GetComponent<ParticleSystem>().Stop();
                    worldSnow.GetComponent<ParticleSystem>().Stop();
                    playerSnow.GetComponent<ParticleSystem>().Stop();
                    cloud.GetComponent<ParticleSystem>().Stop();

                    break;
                //rainy
                case 1:
                    //add rain particle system
                    rain.GetComponent<ParticleSystem>().Play();
                    cloud.GetComponent<ParticleSystem>().Play();
                    break;
                //snowy
                case 2:
                    //add and make active snow particle system 
                    worldSnow.GetComponent<ParticleSystem>().Play();
                    playerSnow.GetComponent<ParticleSystem>().Play();
                    cloud.GetComponent<ParticleSystem>().Play();

                    break;

                default:
                    {
                        rain.GetComponent<ParticleSystem>().Stop();
                        worldSnow.GetComponent<ParticleSystem>().Stop();
                        playerSnow.GetComponent<ParticleSystem>().Stop();
                        cloud.GetComponent<ParticleSystem>().Stop();

                    }
                    break;
            }
        }
        else
        {
            rain.GetComponent<ParticleSystem>().Stop();
            if(worldSnow)
            worldSnow.GetComponent<ParticleSystem>().Stop();
            playerSnow.GetComponent<ParticleSystem>().Stop();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
            getCorrectWeather();
        
    }
}
