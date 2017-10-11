using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FPSTextUpdateScript : MonoBehaviour
{

    TextMeshProUGUI element;
    public TMPro.TMP_FontAsset the_font_asset;

    float frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;

    void Start()
    {
        element = gameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (element.enabled == true)
        {
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0f / updateRate)
            {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= 1.0f / updateRate;
            }

            string textToWrite = " " + fps.ToString("n2") + " ";
            element.SetText(textToWrite);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            element.enabled = !element.enabled;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            element.faceColor = new Color(element.faceColor.r, element.faceColor.g, element.faceColor.b, 0.0f);
        }
     
    }
}