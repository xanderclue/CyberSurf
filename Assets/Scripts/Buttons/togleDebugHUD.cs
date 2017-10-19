using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class togleDebugHUD : SelectedObject
{
    TextElementControllerScript textElementController;
    bool safeCheck = false;

    TextMeshPro onOffText;
    bool IsOn { get { return textElementController.hudElementsControl.debugGUIBool; } set { textElementController.setDebugGUI(value); } }

    public void isOnUpdate()
    {
        if (IsOn)
        {
            onOffText.SetText("On");
        }
        else
        {
            onOffText.SetText("Off");
        }
    }

    private void OnEnable()
    {
        textElementController = GameManager.player.GetComponentInChildren<TextElementControllerScript>();
        if (textElementController != null)
        {
            safeCheck = true;
        }
        onOffText = gameObject.GetComponentsInChildren<TextMeshPro>()[0];
        isOnUpdate();
    }

    public override void SuccessFunction()
    {
        if (safeCheck)
        {
            IsOn = !IsOn;
            if (IsOn)
            {
                onOffText.SetText("On");
            }
            else
            {
                onOffText.SetText("Off");
            }
        }
        else
        {
            Debug.Log("The buttong couldnt find the players text element to toggle");
        }
    }
}