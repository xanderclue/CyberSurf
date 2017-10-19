using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] GameObject theFadeObj;
    float fadeTime = 1.0f;

    Rigidbody playerRB;
    Transform respawnPoint;
    float countDownTime;
    float alpha;
    float timeIntoFade;
    float timeInPitchBlack = 0.1f;

    CameraSplashScreen countdown;
    ManagerClasses.RoundTimer roundTimer;
    ManagerClasses.GameState gameState;
    float roundTimerStartTime;

    bool isRespawning = false;

    public bool IsRespawning { get { return isRespawning; } }

    private void Start()
    {
        countdown = GetComponentInChildren<CameraSplashScreen>();
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        roundTimer = GameManager.instance.roundTimer;
        gameState = GameManager.instance.gameState;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        //stop our respawn coroutines if we've switched scenes
        if (isRespawning)
        {
            StopAllCoroutines();

            roundTimer.PauseTimer(false);

            isRespawning = false;
            respawnPoint = null;
        }
    }

    public void RespawnPlayer(Transform rsPoint, float startTime, float countDownFrom = 3.25f)
    {
        if (!isRespawning && gameState.currentState == GameStates.GamePlay)
        {
            roundTimer.TimeLeft = 0f;

            respawnPoint = rsPoint;
            roundTimerStartTime = startTime;
            countDownTime = countDownFrom;
      
            StartCoroutine(FadeOut());
        }     
    }

    void UpdateAlpha(bool fadingOut)
    {
        //we want to update our alpha based off of how far into the fade time we are in
        alpha = timeIntoFade / fadeTime;

        if (fadingOut == false)
            alpha = 1f - alpha;

        alpha = Mathf.Clamp01(alpha);
        theFadeObj.GetComponent<Renderer>().material.SetFloat("_AlphaValue", alpha);
    }

    IEnumerator FadeOut()
    {
        timeIntoFade = 0f;
        isRespawning = true;

        //keep going until our fade time as elapsed
        while (timeIntoFade < fadeTime + timeInPitchBlack)
        {
            //only update our alpha if it isn't 0
            UpdateAlpha(true);

            timeIntoFade += Time.deltaTime;
            yield return null;
        }

        //lock gameplay movement once we fade out
        EventManager.OnSetGameplayMovementLock(true);

        //once we fade out, move the player
        if (respawnPoint != null)
        {
            playerRB.MovePosition(respawnPoint.position + respawnPoint.forward * 2f);
            playerRB.MoveRotation(Quaternion.Euler(respawnPoint.eulerAngles.x, respawnPoint.eulerAngles.y, 0f));
        }

        //then start to fade in if we aren't transitioning
        if (gameState.currentState == GameStates.GamePlay)
            StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        timeIntoFade = 0f;

        //start the timer
        roundTimer.TimeLeft = roundTimerStartTime;

        //pause the timer
        roundTimer.PauseTimer(true);

        float quarterFadeTime = fadeTime * 0.25f;
        bool countdownStarted = false;

        while (timeIntoFade < fadeTime && gameState.currentState == GameStates.GamePlay)
        {
            UpdateAlpha(false);

            if (!countdownStarted && timeIntoFade > quarterFadeTime)
            {
                countdown.StartCountdown(countDownTime);
                countdownStarted = true;
            }

            timeIntoFade += Time.deltaTime;
            yield return null;
        }

        //unlock movement, but only if we aren't transitioning
        if (gameState.currentState == GameStates.GamePlay)
        {
            //wait for the countdown to finish if we need to
            if (countDownTime - timeIntoFade > 0f)
                yield return new WaitForSeconds(countDownTime - timeIntoFade);

            yield return null;
            if (gameState.currentState == GameStates.GamePlay)
                EventManager.OnSetGameplayMovementLock(false);
        }

        roundTimer.PauseTimer(false);
        isRespawning = false;
        respawnPoint = null;
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

