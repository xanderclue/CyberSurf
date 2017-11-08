using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class weatherSetup : MonoBehaviour {

    private GameObject rainSystem;
    //private GameObject player;
    private GameObject rain;
    private GameObject snow;
    private GameObject cloud;
	// Use this for initialization
	void Start () {
        //player = GameObject.FindGameObjectWithTag("Player");
        rain = GameObject.FindGameObjectWithTag("rain");
        snow = GameObject.FindGameObjectWithTag("snow");
        cloud = GameObject.FindGameObjectWithTag("cloud");
	}
	
    void getCorrectWeather()
  {
      if(SceneManager.GetActiveScene().buildIndex > 1)
      {
          switch((int)WeatherOptions.ActualWeather)
          {
              //sunny
              case 0: 
                        //take away any particle systems besides dust 
                        rain.GetComponent<ParticleSystem>().Stop();
                    snow.GetComponent<ParticleSystem>().Stop();
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
                        snow.GetComponent<ParticleSystem>().Play();
                    cloud.GetComponent<ParticleSystem>().Play();

                    break;

                default:
                    {
                        rain.GetComponent<ParticleSystem>().Stop();
                        snow.GetComponent<ParticleSystem>().Stop();
                        cloud.GetComponent<ParticleSystem>().Stop();

                    }
                    break;
          }
      }
      else
        {
            rain.GetComponent<ParticleSystem>().Stop();
            snow.GetComponent<ParticleSystem>().Stop();
        }
  }
  
	// Update is called once per frame
	void FixedUpdate () {
        getCorrectWeather();
	}
}
