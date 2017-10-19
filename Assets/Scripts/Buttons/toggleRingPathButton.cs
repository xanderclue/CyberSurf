using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class toggleRingPathButton : SelectedObject
{
    TextMeshPro onOffText;
    bool IsOn { get { return GameManager.instance.levelScript.RingPathIsOn; } set { GameManager.instance.levelScript.RingPathIsOn = value; } }

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
        onOffText = gameObject.GetComponentsInChildren<TextMeshPro>()[0];
        isOnUpdate();
    }
    public override void SuccessFunction()
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
}