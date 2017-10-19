using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadelooptest : MonoBehaviour
{
    [SerializeField] bool loopFade = false;
    [SerializeField] float length = 2f;
    [SerializeField] bool reset = false;

    float fadeout = 1f;
    float timeIntoFade = 0f;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (loopFade)
        {
            if (timeIntoFade >= length || timeIntoFade < 0)
            {
                fadeout *= -1;
            }

            timeIntoFade += Time.deltaTime * fadeout;

            float alpha = timeIntoFade / length;
            

            if (fadeout == -1)
             //   alpha = 1f - alpha;

            alpha = Mathf.Clamp01(alpha);
            gameObject.GetComponent<Renderer>().material.SetFloat("_AlphaValue", alpha);
        }
        else if (!loopFade && reset)
        {
            gameObject.GetComponent<Renderer>().material.SetFloat("_AlphaValue", 0);
        }
    }
}
