using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CameraSplashScreen : MonoBehaviour
{
    [SerializeField] GameObject countdownTextElement = null;

    TextMeshPro tmp;
    float maxSeconds;

    private void Start()
    {
        tmp = countdownTextElement.GetComponent<TextMeshPro>();
        maxSeconds = 0f;
    }

    public void StartCountdown(float countdownSeconds)
    {
        countdownTextElement.SetActive(true);
        maxSeconds = countdownSeconds;

        StopAllCoroutines();
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        float currTime = 0f;
        while (currTime < maxSeconds)
        {
            tmp.text = (maxSeconds - currTime).ToString("##");

            currTime += Time.deltaTime;
            yield return null;
        }

        maxSeconds = 0f;
        countdownTextElement.SetActive(false);
    }

    //Don't keep counting down between scenes
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();

        maxSeconds = 0f;
        countdownTextElement.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

}
