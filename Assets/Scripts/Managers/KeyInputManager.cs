using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class KeyInputManager : MonoBehaviour
{
    ManagerClasses.GameState state;

    //variables for returning back to menu
    public float flippedTimer = 3f;
    public bool hubOnFlippedHMD = false;
    bool countingDown = false;
    float timeUpsideDown = 0f;
    Quaternion flippedQuaternion;

    public void setupKeyInputManager(ManagerClasses.GameState s)
    {
        state = s;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state.currentState != GameStates.MainMenu)
            {
                EventManager.OnTriggerTransition(1);
            }
            else
            { 
                SaveLoader.save();
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("XBox Back"))
        {
            StartCoroutine(GameManager.instance.CalibrationCoroutine());
        }

        if (state.currentState != GameStates.MainMenu && Input.GetButtonDown("XBox Start"))
        {
            EventManager.OnTriggerTransition(1);
        }

        if (state.currentState != GameStates.MainMenu && hubOnFlippedHMD && VRDevice.isPresent)
        {
            flippedQuaternion = InputTracking.GetLocalRotation(VRNode.Head);

            //if we're upside down, start the countdown and reset our timer
            if (flippedQuaternion.eulerAngles.z > 150f && flippedQuaternion.eulerAngles.z < 210f && !countingDown)
            {
                countingDown = true;
                timeUpsideDown = 0f;
            }
            else if (countingDown)
            {
                //if we're still upside down
                if (flippedQuaternion.eulerAngles.z > 150f && flippedQuaternion.eulerAngles.z < 210f)
                    timeUpsideDown += Time.deltaTime;
                else
                    countingDown = false;

                //go back to main menu once we've been upside down long enough
                if (timeUpsideDown > flippedTimer)
                {
                    countingDown = false;
                    EventManager.OnTriggerTransition(1);
                }
            }
        }
    }

}
