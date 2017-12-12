using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Race_Mode_Script : MonoBehaviour {
    [SerializeField] public Vector3[] Ring_path;
   [SerializeField] private Vector3 Joe;
    public int counter = 0;
    public GameObject the_player;
    // Use this for initialization
    void Start () {
       /* if (GameModes.Race == GameManager.instance.gameMode.currentMode)
        {
            Joe = this.transform.position;
            the_player = GameObject.FindGameObjectWithTag("Player");
            Joe = the_player.transform.position;
            this.gameObject.transform.rotation = the_player.transform.rotation;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }*/
        GameObject.Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
       
       /* Vector3 the_goal = Ring_path[counter];
        Joe.x = Mathf.MoveTowards(Joe.x, the_goal.x, 0.15f);
        Joe.y = Mathf.MoveTowards(Joe.y, the_goal.y, 0.05f);
        Joe.z = Mathf.MoveTowards(Joe.z, the_goal.z, 0.15f);
        //this.transform.position = Joe;
        Turn_Proper(the_goal);
        
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
        }*/
      }
    void FixedUpdate()
    {
        Vector3 the_goal = Ring_path[counter];
        Joe.x = Mathf.MoveTowards(Joe.x, the_goal.x, 0.3f);
        Joe.y = Mathf.MoveTowards(Joe.y, the_goal.y, 0.05f);
        Joe.z = Mathf.MoveTowards(Joe.z, the_goal.z, 0.3f);
        this.transform.position = Joe;
        Turn_Proper(the_goal);

        float checkx, checky, checkz;
        checkx = Joe.x - the_goal.x;
        checky = Joe.y - the_goal.y;
        checkz = Joe.z - the_goal.z;
        if (checkx < 5 && checkx > -5 && checky < 5 && checky > -5 && checkz < 5 && checkz > -5)
        {
            {
                counter += 5;
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
}

