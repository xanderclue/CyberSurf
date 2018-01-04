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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit the collider test");

        if (other.transform.tag == "Player")
        {
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.upwardAcceleration +=   25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.downwardAcceleration += 25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.restingAcceleration +=  25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.momentum += 25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.minSpeed += 150.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.restingSpeed += 25.0f;

            Debug.Log(Time.time);
            // other.transform.GetComponent<PlayerMovementVariables>().downwardAcceleration += 15;
            // other.transform.GetComponent<PlayerMovementVariables>().restingAcceleration += 15;
            // other.transform.GetComponent<PlayerMovementVariables>().upwardAcceleration += 15;
            StartCoroutine(Wait());
            Debug.Log(Time.time);
            // other.transform.GetComponent<PlayerMovementVariables>().downwardAcceleration -= 15;
            // other.transform.GetComponent<PlayerMovementVariables>().restingAcceleration -= 15;
            // other.transform.GetComponent<PlayerMovementVariables>().upwardAcceleration -= 15;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.upwardAcceleration   -= 25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.downwardAcceleration -= 25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.restingAcceleration  -= 25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.momentum             -= 25.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.minSpeed             -= 150.0f;
            other.transform.GetComponent<PlayerGameplayController>().movementVariables.restingSpeed         -= 25.0f;

        }
    }

}

