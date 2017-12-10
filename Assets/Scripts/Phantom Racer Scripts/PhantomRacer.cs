using UnityEngine;
using UnityEngine.SceneManagement;
public class PhantomRacer : MonoBehaviour
{
    private ScoreManager.ScoreData scoreInfo;
    private int currentPos = 0;
    private void Start()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        GameManager gameManager = GameManager.instance;
        try
        {
            switch (GameManager.instance.gameMode.currentMode)
            {
                case GameModes.Continuous:
                    ScoreManager.ContinuousScores[] continuousScores = gameManager.scoreScript.topContinuousScores;
                    for (int i = 0; i < continuousScores.Length; ++i)
                    {
                        if (continuousScores[i].difficulty == gameManager.gameDifficulty.currentDifficulty)
                        {
                            scoreInfo = continuousScores[i].levels[currentLevel];
                            break;
                        }
                    }
                    break;
                case GameModes.Cursed:
                    ScoreManager.ScoreData[] cursedScores = gameManager.scoreScript.topCursedScores[currentLevel].cursedScores;
                    for (int i = 0; i < cursedScores.Length; ++i)
                    {
                        if (cursedScores[i].difficulty == gameManager.gameDifficulty.currentDifficulty)
                        {
                            scoreInfo = cursedScores[i];
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        catch
        {
            GameManager.DeleteScoresOnExit();
        }
        if (null == scoreInfo.positions)
            Destroy(this);
    }
    private void FixedUpdate()
    {
        if (currentPos >= scoreInfo.positions.Length)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = scoreInfo.positions[currentPos];
        transform.rotation = scoreInfo.rotations[currentPos];
        ++currentPos;
    }
}