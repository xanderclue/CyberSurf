using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour {
    
    IEnumerator Wait()
    {
        Debug.Log(Time.time);

        Debug.Log("I am inside the Wait coroutine");
        yield return new WaitForSeconds(1.5f);
        Debug.Log(Time.time);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
        Debug.Log("hit the collider test");
           //Vector3 newSpeed = other.GetComponent<PlayerGameplayController>().playerRigidbody.velocity *= 2.0f;
            
            other.GetComponent<PlayerGameplayController>().playerRigidbody.velocity *= 1.22f;
             
          //  Mathf.Lerp(other.GetComponent<PlayerGameplayController>().playerRigidbody.AddRelativeForce(0.0f, 0.0f, other.GetComponent<PlayerGameplayController>().currAcceleration * 1.5f, ForceMode.Acceleration))

          //  other.GetComponent<PlayerGameplayController>().playerRigidbody.AddRelativeForce(0.0f, 0.0f, Mathf.Lerp(other.GetComponent<PlayerGameplayController>().currAcceleration, other.GetComponent<PlayerGameplayController>().currAcceleration * 1.5f, 1.0f), ForceMode.Acceleration);
 

             StartCoroutine(Wait());
            other.GetComponent<PlayerGameplayController>().playerRigidbody.velocity *= 0.75f;

        }
    }

}

