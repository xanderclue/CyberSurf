using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnAndDespawnSphere : MonoBehaviour {

    public static bool sphereState;
    public GameObject ball;
	// Use this for initialization
	void Start () {
        ball = GameObject.FindGameObjectWithTag("ball");
        sphereState = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (sphereState == false)
        {
            //    this.gameObject.SetActive(false);
            // gameObject.GetComponentInChildren<GameObject>().SetActive(false);
            ball.SetActive(false);
        }
        else
            ball.SetActive(true);

//        gameObject.GetComponentInChildren<GameObject>().SetActive(true);

//        this.gameObject.SetActive(true);
	}
}
