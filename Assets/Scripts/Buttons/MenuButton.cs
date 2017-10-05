using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : SelectedObject
{
    [SerializeField] int sceneIndex = 0;

    override public void SuccessFunction()
    {
        //changes the current state of the game
        EventManager.OnTriggerTransition(sceneIndex);
    }
}