using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    enum TransitionStates { Sinking, Floating };

    enum DisplayToUpdate { TopScores, GameMode, AICount, Difficulty, PortalSelect };

    enum Levels { Canyon = 2, Multi = 3, RaceTrack = 4, levelCount = 5 };
    Levels currentLevel = Levels.Canyon;

    [SerializeField] float transitionTime = 1f;
    [SerializeField] float sinkRate = 0.1f;
    [SerializeField] Transform[] backs;

    ManagerClasses.GameMode gameMode;
    float sinkDistance = 0.2f;

    [Header("Level Options")]
    [SerializeField] TextMeshPro gameModeTMP;

    [Header("Portal Select")]
    [SerializeField] WorldPortalProperties portal;
    [SerializeField] Image preview;
    [SerializeField] Sprite[] previewSprites;

    [Header("Top Scores")]
    ScoreManager scoreScript;
    [SerializeField] TextMeshPro[] highScoreTMPS;
    int[] scores = new int[10];
    float[] times = new float[10];
    string[] names = new string[10];



    void Start()
    {
        gameMode = GameManager.instance.gameMode;
        scoreScript = GameManager.instance.scoreScript;
        gameModeTMP.text = gameMode.currentMode.ToString();

        //set our preview to the last level we were in
        if (GameManager.instance.lastLevel > 1)
        {
            currentLevel = (Levels)GameManager.instance.lastLevel;
            portal.SceneIndex = (int)currentLevel;

            preview.sprite = previewSprites[GameManager.instance.lastLevel - 2];

        }
        else
        {
            updateScoreDisplay();
        }
    }

    //void UpdateScoresDisplay()
    //{
    //    for (int i = 0; i < 10; i++)
    //    {
    //        highScoreTMPS[i].text = scoreManager.topScores[(int)currentLevel].modes[(int)gameMode.currentMode].scores[i].score.ToString();
    //    }
    //}

    public void NextLevel()
    {
        if (currentLevel + 1 == Levels.levelCount)
            currentLevel = Levels.Canyon;
        else
            ++currentLevel;

        portal.SceneIndex = (int)currentLevel;
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.PortalSelect));
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.TopScores));
    }

    public void PreviousLevel()
    {
        if (currentLevel - 1 < Levels.Canyon)
            currentLevel = Levels.levelCount - 1;
        else
            --currentLevel;

        portal.SceneIndex = (int)currentLevel;
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.PortalSelect));
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.TopScores));
    }

    public void NextGameMode()
    {
        gameMode.NextMode();
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.GameMode));
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.TopScores));
    }

    public void PreviousGameMode()
    {
        gameMode.PreviousMode();
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.GameMode));
        StartCoroutine(TransitionCoroutine(DisplayToUpdate.TopScores));
    }

    IEnumerator TransitionCoroutine(DisplayToUpdate display)
    {
        float transitionTimer = 0f, halfTransitionTime = transitionTime / 2f;

        Vector3 originalPosition = backs[(int)display].position, currPosition = backs[(int)display].position;

        Transform t = Instantiate(backs[(int)display].gameObject, backs[(int)display].position, backs[(int)display].rotation).GetComponent<Transform>();

        t.Translate(Vector3.forward * sinkDistance);
        Vector3 sinkToPosition = t.position;
        Destroy(t.gameObject);

        TransitionStates currentTransition = TransitionStates.Sinking;

        while (true)
        {
            switch (currentTransition)
            {
                case TransitionStates.Sinking:
                    currPosition = Vector3.Lerp(currPosition, sinkToPosition, sinkRate);                                
                    break;
                case TransitionStates.Floating:
                    currPosition = Vector3.Lerp(currPosition, originalPosition, sinkRate);
                    break;
            }

            backs[(int)display].position = currPosition;

            transitionTimer += Time.deltaTime;

            if (currentTransition == TransitionStates.Sinking && transitionTimer >= halfTransitionTime)
            {
                currentTransition = TransitionStates.Floating;
                UpdateDisplay(display);
            }

            else if (currentTransition == TransitionStates.Floating && transitionTimer >= transitionTime)
            {
                backs[(int)display].position = originalPosition;
                break;
            }

            yield return null;
        }
    }

    void UpdateDisplay(DisplayToUpdate display)
    {
        switch(display)
        {
            case DisplayToUpdate.TopScores:
                updateScoreDisplay();
                break;
            case DisplayToUpdate.GameMode:
                gameModeTMP.text = gameMode.currentMode.ToString();
                break;
            case DisplayToUpdate.AICount:
                break;
            case DisplayToUpdate.Difficulty:
                break;
            case DisplayToUpdate.PortalSelect:
                preview.sprite = previewSprites[(int)currentLevel - 2];
                break;
        }
    }

    public void updateScoreDisplay()
    {
        switch (gameMode.currentMode)
        {
            case GameModes.Continuous:
                for (int i = 0; i < scoreScript.topContinuousScores.Length; i++)
                {
                    int cumulativeScore = 0;
                    float totalTime = 0;
                    for (int j = 0; j < scoreScript.topContinuousScores[i].levels.Length; j++)
                    {
                        cumulativeScore += scoreScript.topContinuousScores[i].levels[j].score;
                        totalTime += scoreScript.topContinuousScores[i].levels[j].time;
                    }
                    scores[i] = cumulativeScore;
                    times[i] = totalTime;
                    names[i] = scoreScript.topContinuousScores[i].name;
                }

                break;

            case GameModes.Cursed:
                for (int i = 0; i < scoreScript.topCurseScores[(int)currentLevel].curseScores.Length; i++)
                {
                    scores[i] = scoreScript.topCurseScores[(int)currentLevel].curseScores[i].score;
                    times[i] = scoreScript.topCurseScores[(int)currentLevel].curseScores[i].time;
                    names[i] = scoreScript.topCurseScores[(int)currentLevel].curseScores[i].name;
                }

                break;

            case GameModes.Free:
                for (int i = 0; i < 10; i++)
                {
                    scores[i] = 0;
                    times[i] = 0;
                    names[i] = "NOBODY";
                }
                break;
            case GameModes.GameModesSize:
                break;
            default:
                break;
        }

        for (int i = 0; i < highScoreTMPS.Length; i++)
        {
            highScoreTMPS[i].SetText(i + ": " + names[i] + " | " + scores[i] + " | " + times[i].ToString("n2") + " ");
        }

    }

}
