using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] Image theFadeObj;

    float alpha = 1.0f;
    float fadeTime = 0.5f;
    float timeIntoFade = 0f;

    AsyncOperation asyncOp;

    //used for scene transitions
    ManagerClasses.GameState gameState;

    private void LateUpdate()
    {
        //print(theFadeObj.color);
    }

    private void Start()
    {
        gameState = GameManager.instance.gameState;
    }

    // Fades alpha from 1.0 to 0.0, use at beginning of scene
    IEnumerator FadeIn()
    {
        timeIntoFade = 0f;

        //don't start the fade in until loading is done
        while (asyncOp != null && !asyncOp.isDone)
            yield return null;

        while(timeIntoFade < fadeTime)
        {
            timeIntoFade += Time.deltaTime;
            UpdateAlpha(false);

            yield return null;
        }

        //only unlock player movement when we are in a gameplay scene
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
            EventManager.OnSetGameplayMovementLock(false);
    }

    // Fades from 0.0 to 1.0, use at end of scene
    IEnumerator FadeOut()
    {
        timeIntoFade = 0f;

        //lock player gameplay movement
        EventManager.OnSetGameplayMovementLock(true);

        //make sure our state updates
        gameState.currentState = GameStates.SceneTransition;

        while (timeIntoFade < fadeTime)
        {
            timeIntoFade += Time.deltaTime;
            UpdateAlpha(true);

            yield return null;
        }

        asyncOp = SceneManager.LoadSceneAsync(GameManager.instance.levelScript.nextScene, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = true;
    }

    void UpdateAlpha(bool fadingOut)
    {
        //we want to update our alpha based off of how far into the fade time we are in
        alpha = timeIntoFade / fadeTime;

        if (fadingOut == false)
            alpha = 1f - alpha;

        alpha = Mathf.Clamp01(alpha);
        theFadeObj.material.color = new Color(0f, 0f, 0f, alpha);
    }

    // Starts a fade in when a new level is loaded
    void OnLevelFinished(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void startFadeOutCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinished;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinished;
    }
}
