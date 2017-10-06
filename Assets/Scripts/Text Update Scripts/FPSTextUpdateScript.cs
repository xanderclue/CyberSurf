using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FPSTextUpdateScript : MonoBehaviour
{

    TextMeshProUGUI element;

    float frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;

    void Update()
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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            element.enabled = !element.enabled;
        }
     
    }

    void OnEnable()
    {
        element = gameObject.GetComponent<TextMeshProUGUI>();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        element.enabled = true;
    }
}