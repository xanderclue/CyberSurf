using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Race_Mode_Script : MonoBehaviour {
    [SerializeField] public Vector3[] Ring_path;
   [SerializeField] private Vector3 Joe;
    private float pitch = 0.0f, yaw = 0.0f, gyroPrevPitch = 0.0f, newAcceleration = 0.0f, currAcceleration = 0.0f, debugSpeedIncrease = 0.0f;
    public PlayerMovementVariables movementVariables = null;
    private Rigidbody AI_body;
    public int counter = 0;
    public GameObject the_player;
    // Use this for initialization
    void Start () {
        AI_body = GetComponent<Rigidbody>();
       if (GameMode.Race == GameManager.gameMode)
        {
            Joe = this.transform.position;
            the_player = GameObject.FindGameObjectWithTag("Player");
            transform.position = the_player.transform.position;
            this.gameObject.transform.rotation = the_player.transform.rotation;
            
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    void FixedUpdate()
    {
        Vector3 the_goal = Ring_path[counter];
        Joe = transform.position;
        //this.transform.position = Joe;
        Turn_Proper(the_goal);
        ApplyForce();
        float checkx, checky, checkz;
        checkx = Joe.x - the_goal.x;
        checky = Joe.y - the_goal.y;
        checkz = Joe.z - the_goal.z;
        if (checkx < 5 && checkx > -5 && checky < 5 && checky > -5 && checkz < 5 && checkz > -5)
        {
            {
                counter ++;
                if (counter >= Ring_path.Length)
                {
                    counter = 0;
                }
            }
        }
    }

    private void Turn_Proper(Vector3 Target)
    {
        Vector3 targetDir = Target - this.transform.position;
        float step = 1.0f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(this.transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void ApplyForce()
    {
        float playerSpeed = AI_body.velocity.magnitude;
        if (RoundTimer.timeInLevel < 1.0f)
            currAcceleration = movementVariables.downwardAcceleration;
        else
        {
            currAcceleration = Mathf.Lerp(currAcceleration, newAcceleration, movementVariables.momentum);
        }
            if (pitch > 360.0f - movementVariables.restingThreshold || pitch < movementVariables.restingThreshold)
            {
            newAcceleration = movementVariables.restingAcceleration;
            if (playerSpeed < movementVariables.restingSpeed)
                    AI_body.AddRelativeForce(0.0f, 0.0f, currAcceleration + debugSpeedIncrease, ForceMode.Acceleration);
                else
                    AI_body.AddRelativeForce(0.0f, 0.0f, playerSpeed, ForceMode.Acceleration);
            }
            else if (pitch < 180.0f)
            {
            newAcceleration = movementVariables.downwardAcceleration;
                if (playerSpeed < movementVariables.maxSpeed + debugSpeedIncrease)
                    AI_body.AddRelativeForce(0.0f, 0.0f, currAcceleration + debugSpeedIncrease, ForceMode.Acceleration);
                else
                    AI_body.AddRelativeForce(0.0f, 0.0f, playerSpeed, ForceMode.Acceleration);
            }
            else
            {
            newAcceleration = movementVariables.upwardAcceleration;
                if (playerSpeed < movementVariables.minSpeed + debugSpeedIncrease)
                    AI_body.AddRelativeForce(0.0f, 0.0f, currAcceleration + debugSpeedIncrease, ForceMode.Acceleration);
                else
                    AI_body.AddRelativeForce(0.0f, 0.0f, playerSpeed, ForceMode.Acceleration);
            }
        
    }
}

