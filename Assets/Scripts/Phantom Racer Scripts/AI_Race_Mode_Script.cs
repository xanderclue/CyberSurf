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
        Joe = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (wait > 2)
        {
            Vector3 the_goal = Ring_path[counter];
            Joe = the_goal;
            if (Joe == the_goal)
            {
                this.transform.position = Joe;
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

