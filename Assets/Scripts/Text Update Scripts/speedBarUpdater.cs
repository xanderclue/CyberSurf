﻿using UnityEngine;
using UnityEngine.UI;
public class speedBarUpdater : MonoBehaviour
{
    [SerializeField] private float currentValue = 0.0f, maxValue = 0.0f, minValue = 0.0f;
    private Image fillUpBar;
    private ManagerClasses.PlayerMovementVariables moveVars = null;
    private Rigidbody player = null;
    private float prevValue = 0.0f;
    private void Start()
    {
        fillUpBar = GetComponentsInParent<Image>()[1];
        player = GetComponentInParent<Rigidbody>();
        fillUpBar.color = gameObject.GetComponent<Image>().color;
    }
    private void OnEnable()
    {
        moveVars = GetComponentInParent<PlayerGameplayController>().movementVariables;
        maxValue = moveVars.maxSpeed;
        minValue = moveVars.minSpeed;
        
    }
    private void Update()
    {
        currentValue = player.velocity.magnitude;
        if (prevValue != currentValue)
            fillUpBar.fillAmount = (currentValue - minValue) / (maxValue - minValue);
        prevValue = currentValue;
    }
}