using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Race_Mode_Script : MonoBehaviour {
    [SerializeField] public Vector3[] Ring_path;
   [SerializeField] private Vector3 Joe;
    public int counter = 0;
    public float wait = 0;
	// Use this for initialization
	void Start () {
        if (GameMode.Race == GameManager.gameMode)
        {
            Joe = this.transform.position;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
       
            Vector3 the_goal = Ring_path[counter];
           Joe.x = Mathf.MoveTowards(Joe.x, the_goal.x, 0.5f);
            Joe.y = Mathf.MoveTowards(Joe.y, the_goal.y, 0.5f);
            Joe.z = Mathf.MoveTowards(Joe.z, the_goal.z, 0.5f);
            this.transform.position = Joe;
        float checkx, checky, checkz;
        checkx = Joe.x - the_goal.x;
        checky = Joe.y - the_goal.y;
        checkz = Joe.z - the_goal.z;
        if (checkx < 5 && checkx > -5 && checky < 5 && checky > -5 && checkz < 5 && checkz > -5)
        {
            {
                counter++;
                wait = 0;
                if (counter >= Ring_path.Length)
                {
                    counter = 0;
                }
            }
        }
        else
        {
            wait += 0.1f;
        }
      }
	}

