using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deIntensifyScript : MonoBehaviour
{

    Material bluLight;
    Material betterLight;
    float timeToMove;
    float currentTime;

    void Start()
    {
        betterLight = gameObject.GetComponent<Renderer>().material;
        bluLight = Resources.Load("lambert1") as Material;

        currentTime = 0.0f;
        timeToMove = 15.0f;

        
       // GetComponent<Renderer>().materials[0] = bluLight;
        //GetComponent<Renderer>().materials[1] = betterLight;
        
        //foreach (Material matt in GetComponent<Renderer>().materials)
       //{
       //    if (matt.name == "lambert1 (Instance)")
       //    {
       //        bluLight = matt;
       //        break;
       //    }
       //}
    }



    void Update()
    {
        if (keepPlayerStill.tutorialOn == false)
        {
            if (currentTime <= timeToMove)
            {
                currentTime += Time.deltaTime;
                betterLight.Lerp(betterLight, bluLight, currentTime / timeToMove);

            }
            else
                currentTime = 0.0f;
        }
    }
}
