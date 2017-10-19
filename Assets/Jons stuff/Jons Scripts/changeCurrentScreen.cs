using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCurrentScreen : SelectedObject
{
    enum ButtonType { Confirm, Next, Skip }

    [SerializeField, Space] ButtonType buttonType;

    public override void SuccessFunction()
    {
        switch (buttonType)
        {
            case ButtonType.Confirm:
                startupSplashScreenPlayer.timePlayingCurrent = 0.0f;
                startupSplashScreenPlayer.currentScreen++;
                break;
            case ButtonType.Next:
                if (startupSplashScreenPlayer.currentScreen != 6)
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
                break;
            case ButtonType.Skip:
                startupSplashScreenPlayer.currentScreen = 6;
                break;
            default:
                Debug.LogWarning("Missing case: \"" + buttonType.ToString("F") + "\"");
                break;
        }
    }

}
