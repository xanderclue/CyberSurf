using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

}
