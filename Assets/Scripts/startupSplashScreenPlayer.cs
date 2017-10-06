using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startupSplashScreenPlayer : MonoBehaviour
{
    public GameObject selector;
    public GameObject[] splashScreens;
    public float[] timesToPlayScreens;
    GameObject confirmation;
    GameObject tutorial;
    GameObject nextPrevSkip;
    public static float timePlayingCurrent = 0.0f;
    public static int currentScreen = 0;
    private bool active = true;


    void Start()
    {
        for (int i = 1; i < splashScreens.Length; i++)
        {
            splashScreens[i].SetActive(false);
        }
        selector.SetActive(false);
        nextPrevSkip = GameObject.FindGameObjectWithTag("nextPrevSkip");
        tutorial = GameObject.FindGameObjectWithTag("tutorial");
        confirmation = GameObject.FindGameObjectWithTag("confirm");
        confirmation.SetActive(true);

    }
    private void checker()
    {
        switch (currentScreen)
        {
            case 0:
                {
                    for (int i = 1; i < splashScreens.Length; i++)
                    {
                        splashScreens[i].SetActive(false);
                    }
                    nextPrevSkip.SetActive(false);

                    confirmation.SetActive(true);
                    splashScreens[0].SetActive(true);

                }
                break;

            case 1:
                {
                    splashScreens[0].SetActive(false);
                    splashScreens[1].SetActive(true);
                    for (int i = 2; i < splashScreens.Length; i++)
                    {
                        splashScreens[i].SetActive(false);
                    }
                    confirmation.SetActive(false);
                    nextPrevSkip.SetActive(true);


                }
                break;

            case 2:
                {
                    splashScreens[0].SetActive(false);
                    splashScreens[1].SetActive(false);
                    splashScreens[2].SetActive(true);
                    for (int i = 3; i < splashScreens.Length; i++)
                    {
                        splashScreens[i].SetActive(false);
                    }
                    confirmation.SetActive(false);
                    nextPrevSkip.SetActive(true);


                }
                break;

            case 3:
                {

                    for (int i = 0; i < 3; i++)
                    {
                        splashScreens[i].SetActive(false);
                    }
                    splashScreens[3].SetActive(true);
                    for (int i = 4; i < splashScreens.Length; i++)
                    {
                        splashScreens[i].SetActive(false);
                    }
                    confirmation.SetActive(false);
                    nextPrevSkip.SetActive(true);


                }
                break;

            case 4:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        splashScreens[i].SetActive(false);

                    }
                    splashScreens[4].SetActive(true);
                    //     splashScreens[5].SetActive(false                     
                    confirmation.SetActive(false);
                    nextPrevSkip.SetActive(true);


                }
                break;
            case 5:
                {
                    for (int i = 0; i < splashScreens.Length - 2; i++)
                    {
                        splashScreens[i].SetActive(false);
                    }
                     splashScreens[5].SetActive(true);
                    keepPlayerStill.tutorialOn = false;

                    confirmation.SetActive(false);
                    nextPrevSkip.SetActive(true);


                }
                //  timePlayingCurrent = 0.0f;
                break;

            case 6:
                {
                    for (int i = 0; i < splashScreens.Length; i++)
                    {
                        splashScreens[i].SetActive(false);
                    }
                    keepPlayerStill.tutorialOn = false;
                    tutorial.SetActive(false);
                    confirmation.SetActive(false);
                    nextPrevSkip.SetActive(false);


                }
                //  timePlayingCurrent = 0.0f;
                break;
        }
    }
    private void FixedUpdate()
    {

        if (active)
        {
            timePlayingCurrent += Time.deltaTime;
            checker();

            if (timePlayingCurrent >= timesToPlayScreens[currentScreen])
            {
                splashScreens[currentScreen].SetActive(false);
                currentScreen++;
                if (currentScreen < splashScreens.Length)
                {
                    splashScreens[currentScreen].SetActive(true);
                }
                else
                {
                    selector.SetActive(true);
                    active = false;
                }
                timePlayingCurrent = 0;
                if (currentScreen == 5)
                {
                    keepPlayerStill.tutorialOn = false;
                    GameObject.FindGameObjectWithTag("tutorial").SetActive(false);
                }
            }
        }
    }
}
