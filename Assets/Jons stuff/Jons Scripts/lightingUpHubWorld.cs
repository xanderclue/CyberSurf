using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightingUpHubWorld : MonoBehaviour
{

    float timeToMove;
    float currentTime;

    void Start()
    {
        currentTime = 0.0f;
        timeToMove = 20.0f;
        RenderSettings.fog = true;
    }


    void Update()
    {
        if (keepPlayerStill.tutorialOn == false)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.0f, currentTime / timeToMove);
            }
            else
                currentTime = 0.0f;
        }
    }
}
