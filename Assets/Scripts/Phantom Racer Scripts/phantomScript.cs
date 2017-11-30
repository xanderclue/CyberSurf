using UnityEngine;
using UnityEngine.SceneManagement;
using Xander.Debugging;
public class phantomScript : MonoBehaviour
{
    private ScoreManager.ScoreData scoreInfo;
    private int currentPos = 0;
    private void Start()
    {
        try { TryStart(); }
        catch { Debug.Log("scores corrupt.. scores file will be reset on exit"); GameManager.DeleteScoresOnExit(); }
    }
    private void TryStart()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        GameManager gameManager = GameManager.instance;
        switch (GameManager.instance.gameMode.currentMode)
        {
            case GameModes.Continuous:
                ScoreManager.ContinuousScores[] contScores = gameManager.scoreScript.topContinuousScores;
                for (int i = 0; i < contScores.Length; i++)
                {
                    if (contScores[i].difficulty == gameManager.gameDifficulty.currentDifficulty)
                    {
                        scoreInfo = contScores[i].levels[level];
                        break;
                    }
                }
                break;
            case GameModes.Cursed:
                ScoreManager.CursedScores[] levelScores = gameManager.scoreScript.topCursedScores;
                for (int i = 0; i < levelScores[level].cursedScores.Length; i++)
                {
                    if (levelScores[level].cursedScores[i].difficulty == gameManager.gameDifficulty.currentDifficulty)
                    {
                        scoreInfo = levelScores[level].cursedScores[i];
                        break;
                    }
                }
                break;
            case GameModes.Free:
                break;
            case GameModes.Race:
                Debug.Log("To add, Race Case PhantomScript");
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameManager.gameMode.currentMode.ToString("F") + "\"" + this.Info(), this);
                break;
        }
    }
    void FixedUpdate()
    {
        if (null != scoreInfo.positions)
        {
            transform.position = scoreInfo.positions[currentPos];
            transform.rotation = scoreInfo.rotations[currentPos];
            ++currentPos;
            if (currentPos >= scoreInfo.positions.Length)
                currentPos = 0;
        }
    }
}