using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCurrentScreen : SelectedObject
{

    GameObject button;
    GameObject player;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        button = this.gameObject;

    }

    public override void SuccessFunction()
    {
        if (button.transform.position.x < player.transform.position.x)
        {
            if (startupSplashScreenPlayer.currentScreen != 0)
            { 
                startupSplashScreenPlayer.currentScreen--; 
                startupSplashScreenPlayer.timePlayingCurrent = 0.0f;
            }
            else
                Debug.Log("boo");
        }

        else if (button.transform.position.x > player.transform.position.x)   
        {
            if (startupSplashScreenPlayer.currentScreen != 5)
            {

                startupSplashScreenPlayer.currentScreen++;
                startupSplashScreenPlayer.timePlayingCurrent = 0.0f;
            }
            else
            {
                keepPlayerStill.tutorialOn = false;

                Debug.Log("boohoo");
                GameObject.FindGameObjectWithTag("tutorial").SetActive(false);
            }

        }


    }

    
}
