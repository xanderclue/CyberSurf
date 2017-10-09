using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dimSpotlight : MonoBehaviour
{
    Light spotlight;
    float currentTime;
    float timeToPlay;
     
    void Start()
    {
        timeToPlay = 15.0f;
        currentTime = 0.0f;
        spotlight = this.gameObject.GetComponent<Light>();
    }


    void Update()
    {
        if (keepPlayerStill.tutorialOn == false)
        {

            currentTime += Time.deltaTime;
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, 0.0f, currentTime / timeToPlay);
        }
        else
        {
            currentTime += Time.deltaTime;
            spotlight.intensity = Mathf.Lerp(spotlight.intensity, 50.0f, currentTime / timeToPlay);
            if (spotlight.intensity == 50)
            {
                currentTime = 0.0f;
            }
        }

    }
}
