using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
    }
    private void OnCollisionEnter(Collision collision)
    {
            Debug.Log("hit the collider test");

        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerMovementVariables>().downwardAcceleration += 15;
            collision.transform.GetComponent<PlayerMovementVariables>().restingAcceleration += 15;
            collision.transform.GetComponent<PlayerMovementVariables>().upwardAcceleration += 15;
            wait();
            collision.transform.GetComponent<PlayerMovementVariables>().downwardAcceleration -= 15;
            collision.transform.GetComponent<PlayerMovementVariables>().restingAcceleration  -= 15;
            collision.transform.GetComponent<PlayerMovementVariables>().upwardAcceleration   -= 15;


        }
    }
}

