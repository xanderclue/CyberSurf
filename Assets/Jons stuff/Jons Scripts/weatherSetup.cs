using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class weatherSetup : MonoBehaviour {

    private GameObject rainSystem;
    private GameObject player;
    private ParticleSystem rain;
    private ParticleSystem snow;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
       

	}
	
    void getCorrectWeather()
    {
        if(SceneManager.GetActiveScene().buildIndex > 1)
        {
            switch((int)WeatherOptions.ActualWeather)
            {
                //sunny
                case 0:
                    {
                        //take away any particle systems besides dust
                        effectController.triggerParticleEffects[0].Stop();
                        effectController.triggerParticleEffects[1].Stop();


                    }
                    break;
                    //rainy
                case 1:
                    {
                        //add rain particle system
                        effectController.triggerParticleEffects[0].Play();


                    }
                    break;
                    //snowy
                case 2:
                    {
                        //add and make active snow particle system
                        effectController.triggerParticleEffects[1].Play();


                    }
                    break;
            }
        }
    }

	// Update is called once per frame
	void Update () {
        getCorrectWeather();
	}
}
