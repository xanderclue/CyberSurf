using UnityEngine;
using UnityEngine.SceneManagement;
public class PhantomRacer : MonoBehaviour
{
    private ScoreManager.ScoreData scoreInfo;
    private int currentPos = 0;
    private void Start()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        try
        {
            switch (GameManager.gameMode)
            {
                case GameMode.Continuous:
                    ScoreManager.ContinuousScores[] continuousScores = ScoreManager.topContinuousScores;
                    for (int i = 0; i < continuousScores.Length; ++i)
                    {
                        if (continuousScores[i].difficulty == GameManager.gameDifficulty)
                        {
                            scoreInfo = continuousScores[i].levels[currentLevel];
                            break;
                        }
                    }
                    break;
                case GameMode.Cursed:
                    ScoreManager.ScoreData[] cursedScores = ScoreManager.topCursedScores[currentLevel].cursedScores;
                    for (int i = 0; i < cursedScores.Length; ++i)
                    {
                        if (cursedScores[i].difficulty == GameManager.gameDifficulty)
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