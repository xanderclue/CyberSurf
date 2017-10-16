using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedBarUpdater : MonoBehaviour
{
    //must set these values
    public float currentValue = 0;
    public float maxValue = 0;
    public float minValue = 0;
    
    Image fillUpBar;

    private ManagerClasses.PlayerMovementVariables moveVars;
    private Rigidbody player;

    //so we dont do division as often as possible
    float prevValue;

    void Start()
    {
        fillUpBar = gameObject.GetComponentsInParent<Image>()[1];
        player = GetComponentInParent<Rigidbody>();
    }

    private void OnEnable()
    {
        moveVars = gameObject.GetComponentInParent<PlayerGameplayController>().movementVariables;
        maxValue = moveVars.maxSpeed;
        minValue = moveVars.minSpeed;
    }

    private void Update()
    {
        currentValue = player.velocity.magnitude;
        if (prevValue != currentValue)
        {
            fillUpBar.fillAmount = (currentValue - minValue) / (maxValue - minValue);
        }

        prevValue = currentValue;
    }
}
