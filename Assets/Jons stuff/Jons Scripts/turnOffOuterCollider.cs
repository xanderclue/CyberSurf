using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOffOuterCollider : MonoBehaviour
{
    GameObject outerCollider;
    // Use this for initialization
    void Start()
    {
        outerCollider = GameObject.FindGameObjectWithTag("invisibleWalls");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Board")
        {
            outerCollider.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Board")
        {
            outerCollider.gameObject.SetActive(true);
        }
    }
    // private void OnTriggerEnter(Collision collision)
    // {
    //     
    // }
}
