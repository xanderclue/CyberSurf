﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image theFadeObj;
    public float fadeSpeed = 0.8f;
    public float fadeTime = 10.0f;

    //private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    //used for scene transitions
    public AsyncOperation loadOpertion;

    //private bool isFading = false;

    // Starts the fade in
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinished;
        EventManager.OnFade += startFadeOutCoroutine;
        StartCoroutine(FadeIn());
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinished;
        EventManager.OnFade -= startFadeOutCoroutine;
    }

    // Starts a fade in when a new level is loaded
    void OnLevelFinished(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    void startFadeOutCoroutine()
    {
        StartCoroutine(FadeOut());
    }
    // Fades alpha from 1.0 to 0.0, use at beginning of scene
    IEnumerator FadeIn()
    {
        fadeTime = BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
        //isFading = false;
    }

    bool checkIfDoneLoading(AsyncOperation operation)
    {
        return operation.isDone;
    }

    // Fades from 0.0 to 1.0, use at end of scene
    IEnumerator FadeOut()
    {
        fadeTime = BeginFade(1);


        yield return new WaitForSeconds(fadeTime);
        loadOpertion = SceneManager.LoadSceneAsync(GameManager.instance.levelScript.nextScene, LoadSceneMode.Single);
        //says wait for operation to finish
        loadOpertion.allowSceneActivation = true;
        while (!loadOpertion.isDone)
        {
            yield return null;
        }

        //loadOpertion.allowSceneActivation = true;
        //isFading = false;
        GameManager.instance.levelScript.fadeing = false;
    }

    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        theFadeObj.color = new Color(0, 0, 0, alpha);
    }

    float BeginFade (int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }
}
