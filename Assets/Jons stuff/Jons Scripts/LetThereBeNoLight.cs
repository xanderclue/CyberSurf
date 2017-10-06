using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetThereBeNoLight : MonoBehaviour {
    
    Object[] objects;
    Material test;
    Light[] lights;
    GameObject[] all;
    public bool[] baked;
    GameObject[] objs;
	// Use this for initialization
	void Start () {
       all = UnityEngine.Object.FindObjectsOfType<GameObject>();
       FindObjectsOfType(typeof(GameObject));
        for(int i = 0; i < all.Length; i++)
        {
            if(all[i].GetComponent<Light>())
            {
            // baked[i] = all[i].GetComponent<Light>().isBaked;
                all[i].GetComponent<Light>().color = new Color(0, 0, 0, 0);

            }
        }
	}
	
    


	// Update is called once per frame
	void Update () {
		
	}
}                            