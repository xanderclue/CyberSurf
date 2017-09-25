using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFanController : MonoBehaviour
{
    MotorData motor;
    Rigidbody playerRB;
    Transform playerTransform;
    ManagerClasses.PlayerMovementVariables pmv;

    int motorCount = 0;
    float updatedVelocity = 0f;
    float sampledVelocity = 0f;

    float invertedDenominator = 0f;
    float twoPercentIncrease = 0f;

    [SerializeField] [Range(0f, 1.0f)] float interpolateAmount = 0.05f;

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
        twoPercentIncrease = (pmv.maxSpeed - pmv.minSpeed) * -0.02f;
        invertedDenominator = 1f / (pmv.maxSpeed - pmv.minSpeed);
    }

    IEnumerator DetectFanCoroutine()
    {
        motor = new MotorData();

        //wait for the motor to attatch
        yield return new WaitForSeconds(0.1f);

        if (null != motor.device)
            motorCount = motor.device.motors.Count;

        if (motorCount > 0)
            StartCoroutine(FanCoroutine());
        else
            motor.Close();
    }

    IEnumerator FanCoroutine()
    {
        yield return new WaitForFixedUpdate();

        //get our velocity based on if we are going forward/backwards
        sampledVelocity = playerTransform.InverseTransformDirection(playerRB.velocity).z;
        
        //multiplying by -1 to try and reverse the motor
        sampledVelocity = (sampledVelocity - pmv.minSpeed + twoPercentIncrease) * 100f * invertedDenominator *-1f;
        Mathf.Clamp(sampledVelocity, 0f, 100f);

        updatedVelocity = Mathf.Lerp(updatedVelocity, sampledVelocity, interpolateAmount);

        for (int i = 0; i < motorCount; i++)
            motor.device.motors[i].Velocity = updatedVelocity;

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
