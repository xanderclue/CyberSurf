using UnityEngine;
using TMPro;
using Xander.Debugging;
public class lastScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreDisplay = null, timeDisplay = null;
    private void Start()
    {
        int lastScore = 0, lastScoreLocation = 0, i = 0, j = 0;
        float lastTime = 0.0f;
        GameManager gameManager = GameManager.instance;
        ScoreManager scoreScript = gameManager.scoreScript;
        switch (gameManager.gameMode.currentMode)
        {
            case GameModes.Continuous:
                for (i = 0; i < scoreScript.topContinuousScores.Length; ++i)
                {
                    if (scoreScript.topContinuousScores[i].isLastScoreInput)
                    {
                        lastScoreLocation = i;
                        break;
                    }
                }
                if (gameManager.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
                {
                    for (i = 0; i < scoreScript.topContinuousScores[lastScoreLocation].levels.Length; ++i)
                    {
                        lastScore += scoreScript.topContinuousScores[lastScoreLocation].levels[i].score;
                        lastTime += scoreScript.topContinuousScores[lastScoreLocation].levels[i].time;
                    }
                }
                else
                {
                    lastScore = 0;
                    lastTime = 0.0f;
                }
                break;
            case GameModes.Cursed:
                for (i = 0; i < scoreScript.topCursedScores.Length; ++i)
                {
                    for (j = 0; j < scoreScript.topCursedScores[i].cursedScores.Length; ++j)
                    {
                        if (scoreScript.topCursedScores[i].cursedScores[j].isLastScoreInput)
                        {
                            lastScoreLocation = j;
                            break;
                        }
                    }
                    if (j < scoreScript.topCursedScores[i].cursedScores.Length - 1)
                        break;
                }
                if (gameManager.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
                {
                    lastScore = scoreScript.topCursedScores[gameManager.lastPortalBuildIndex].cursedScores[lastScoreLocation].score;
                    lastTime = scoreScript.topCursedScores[gameManager.lastPortalBuildIndex].cursedScores[lastScoreLocation].time;
                }
                else
                {
                    lastScore = 0;
                    lastTime = 0.0f;
                }
                break;
            case GameModes.Free:
                Debug.Log("Free Mode shouldn't display anything" + this.Info(), this);
                break;
            case GameModes.Race:
                for (i = 0; i < scoreScript.topRaceScores.Length; ++i)
                {
                    for (j = 0; j < scoreScript.topRaceScores[i].racescores.Length; ++j)
                    {
                        if (scoreScript.topRaceScores[i].racescores[j].isLastScoreInput)
                        {
                            lastScoreLocation = j;
                            break;
                        }
                    }
                    if (j < scoreScript.topRaceScores[i].racescores.Length - 1)
                        break;
                }
                if (gameManager.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
                {
                    lastScore = scoreScript.topRaceScores[gameManager.lastPortalBuildIndex].racescores[lastScoreLocation].score;
                    lastTime = scoreScript.topRaceScores[gameManager.lastPortalBuildIndex].racescores[lastScoreLocation].time;
                }
                else
                {
                    lastScore = 0;
                    lastTime = 0.0f;
                }
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameManager.gameMode.currentMode.ToString("F") + "\"" + this.Info(), this);
                break;
        }
        scoreDisplay.SetText("Score: " + lastScore);
        timeDisplay.SetText("Time: " + lastTime.ToString("n2"));
        Destroy(this);
    }
}