using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFanController : MonoBehaviour
{
    private MotorData motor = null;
    private Rigidbody playerRB = null;
    private Transform playerTransform = null;
    private ManagerClasses.PlayerMovementVariables pmv = null;
    private int motorCount = 0;
    private float motorPercentage = 0.0f;
    private float invertedMaxSpeed100x = 0.0f;

    //called by our BoardManager
    public void SetupFanControllerScript()
    {
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        playerTransform = GameManager.player.GetComponent<Transform>();
        pmv = GameManager.player.GetComponent<PlayerGameplayController>().movementVariables;

        UpdateFanPercentage();

        StartCoroutine(DetectFanCoroutine());
    }

    //called by our BoardManager
    public void UpdateFanPercentage()
    {
        pmv = GameManager.player.GetComponent<PlayerGameplayController>().movementVariables;
        invertedMaxSpeed100x = 100.0f / pmv.maxSpeed;
    }

    IEnumerator DetectFanCoroutine()
    {
        motor = new MotorData();

        //wait for the motor to attach
        yield return new WaitForSeconds(0.1f);

        if (null != motor.MotorDevice)
            motorCount = motor.MotorDevice.motors.Count;

        if (motorCount > 0)
            StartCoroutine(FanCoroutine());
        else
            motor.Close();
    }

    IEnumerator FanCoroutine()
    {
        yield return new WaitForFixedUpdate();

        //get our velocity based on if we are going forward/backwards
        motorPercentage = Mathf.Clamp(playerTransform.InverseTransformDirection(playerRB.velocity).z * invertedMaxSpeed100x, 25.0f, 100.0f);

        for (int i = 0; i < motorCount; ++i)
            motor.MotorDevice.motors[i].Velocity = motorPercentage;

        StartCoroutine(FanCoroutine());
    }

    private void OnApplicationQuit()
    {
        if (motor != null)
        {
            StopAllCoroutines();
            motor.Close();
        }
    }
}