using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public Image theFadeObj;
    public float fadeTime = 4.0f;

    Rigidbody playerRB;
    Transform respawnPoint;
    float countDownTime;
    float alpha;
    float timeIntoFade;

    CameraSplashScreen countdown;
    ManagerClasses.RoundTimer roundTimer;
    float roundTimerStartTime;

    bool isRespawning = false;

    public bool IsRespawning { get { return isRespawning; } }

    private void Start()
    {
        countdown = GetComponentInChildren<CameraSplashScreen>();
        playerRB = GameManager.player.GetComponent<Rigidbody>();
        roundTimer = GameManager.instance.scoreScript.roundTimer;
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
        if (!isRespawning)
        {
            roundTimer.TimeLeft = 0f;

            respawnPoint = rsPoint;
            roundTimerStartTime = startTime;
            countDownTime = countDownFrom;

            alpha = theFadeObj.color.a;           
            StartCoroutine(FadeOut());
        }     
    }

    void UpdateAlpha(float direction)
    {
        //we want to update our alpha based off of how far into the fade time we are in
        alpha += direction * timeIntoFade / fadeTime;
        alpha = Mathf.Clamp01(alpha);

        theFadeObj.color = new Color(0f, 0f, 0f, alpha);
    }

    IEnumerator FadeOut()
    {
        timeIntoFade = 0f;
        isRespawning = true;
        bool movementLockSet = false;

        //keep going until our fade time as elapsed
        while (timeIntoFade < fadeTime)
        {
            //only update our alpha if it isn't 0
            UpdateAlpha(1);

            //once alpha is == 1, lock movement
            if (!movementLockSet && alpha == 1f)
            {
                EventManager.OnSetGameplayMovementLock(true);
                movementLockSet = true;
            }

            timeIntoFade += Time.deltaTime;
            yield return null;
        }

        //once we fade out, move the player
        if (respawnPoint != null)
        {
            playerRB.MovePosition(respawnPoint.position + respawnPoint.forward * 2f);
            playerRB.MoveRotation(Quaternion.Euler(respawnPoint.eulerAngles.x, respawnPoint.eulerAngles.y, 0f));
        }

        //then start to fade in   
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

        while (timeIntoFade < fadeTime)
        {
            UpdateAlpha(-1);

            if (!countdownStarted && timeIntoFade > quarterFadeTime)
            {
                countdown.StartCountdown(countDownTime);
                countdownStarted = true;
            }

            timeIntoFade += Time.deltaTime;
            yield return null;
        }

        //wait for the countdown to finish if we need to
        if (countDownTime - timeIntoFade > 0f)
            yield return new WaitForSeconds(countDownTime - timeIntoFade);

        //unlock movement
        EventManager.OnSetGameplayMovementLock(false);

        //unpause the timer
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

