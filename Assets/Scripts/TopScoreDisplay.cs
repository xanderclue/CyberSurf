using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TopScoreDisplay : MonoBehaviour {

    enum Levels { Canyon = 2, MultiEnvironment, BackyardRacetrack, levelCount };
    Levels currentLevel = Levels.Canyon;

    [Header("Sink Effect")]
    [SerializeField] BackBoardSinkEffect sinkEffect;
    [SerializeField] Transform scoreBackBoard;
    bool updateScore = false;

    ManagerClasses.GameMode gameMode;

    [Header("Top Scores")]
    [SerializeField] TextMeshPro[] highScoreTMPS;
    ScoreManager scoreScript;
    int[] scores = new int[10];
    float[] times = new float[10];
    string[] names = new string[10];

    void Start()
    {
        gameMode = GameManager.instance.gameMode;
        scoreScript = GameManager.instance.scoreScript;

        //set our preview to the last level we were in
        if (GameManager.instance.lastPortalBuildIndex > 1)
            currentLevel = (Levels)GameManager.instance.lastPortalBuildIndex;

        UpdateScoreDisplay();
    }

    public void StartScoreUpdate()
    {
        updateScore = true;
        StartCoroutine(sinkEffect.SinkEffectCoroutine(scoreBackBoard));
    }

    void UpdateScoreDisplay()
    {
        switch (gameMode.currentMode)
        {
            case GameModes.Continuous:
                for (int i = 0; i < scoreScript.topContinuousScores.Length; ++i)
                {
                    int cumulativeScore = 0;
                    float totalTime = 0;
                    for (int j = 0; j < scoreScript.topContinuousScores[i].levels.Length; ++j)
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
                for (int i = 0; i < scoreScript.topCurseScores[(int)currentLevel].curseScores.Length; ++i)
                {
                    scores[i] = scoreScript.topCurseScores[(int)currentLevel].curseScores[i].score;
                    times[i] = scoreScript.topCurseScores[(int)currentLevel].curseScores[i].time;
                    names[i] = scoreScript.topCurseScores[(int)currentLevel].curseScores[i].name;
                }
                break;
            case GameModes.Free:
                for (int i = 0; i < 10; ++i)
                {
                    scores[i] = 0;
                    times[i] = 0;
                    names[i] = "NOBODY";
                }
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameMode.currentMode.ToString("F") + "\"");
                break;
        }

        for (int i = 0; i < highScoreTMPS.Length; ++i)
        {
            highScoreTMPS[i].SetText(i + ": " + names[i] + " | " + scores[i] + " | " + times[i].ToString("n2") + " ");
        }
    }

    private void CheckUpdateFlags()
    {
        if (updateScore)
        {
            UpdateScoreDisplay();
            updateScore = false;
        }
    }

    private void OnEnable()
    {
        BackBoardSinkEffect.StartContentUpdate += CheckUpdateFlags;
    }

    private void OnDisable()
    {
        BackBoardSinkEffect.StartContentUpdate -= CheckUpdateFlags;
    }
}
