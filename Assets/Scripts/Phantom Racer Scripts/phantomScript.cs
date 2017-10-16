using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class phantomScript : MonoBehaviour
{
    ScoreManager.scoreStruct scoreInfo;
    int currentPos = 0;

    GameManager gameManager;

	// Use this for initialization
	void Start ()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        gameManager = GameManager.instance;

        switch (GameManager.instance.gameMode.currentMode)
        {
            case GameModes.Continuous:
                ScoreManager.continuousScores[] contScores = gameManager.scoreScript.topContinuousScores;
                for (int i = 0; i < contScores.Length; i++)
                {
                    if (contScores[i].difficulty == gameManager.gameDifficulty.currentDifficulty)
                    {
                        scoreInfo = contScores[i].levels[level];
                        break;
                    }
                }
                //scoreInfo = contScores[0].levels[level];
                break;

            case GameModes.Cursed:
                ScoreManager.levelCurseScores[] levelScores = gameManager.scoreScript.topCurseScores;
                for (int i = 0; i < levelScores[level].curseScores.Length; i++)
                {
                    if (levelScores[level].curseScores[i].difficulty == gameManager.gameDifficulty.currentDifficulty)
                    {
                        scoreInfo = levelScores[level].curseScores[i];
                        break;
                    }
                }
                //scoreInfo = levelScores[level].curseScores[0];
                break;

            case GameModes.Free:
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameManager.gameMode.currentMode.ToString("F") + "\"");
                break;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (scoreInfo.positions != null)
        {
            gameObject.transform.position = scoreInfo.positions[currentPos];
            gameObject.transform.rotation = scoreInfo.rotations[currentPos];
            currentPos++;
            if (currentPos >= scoreInfo.positions.Length)
            {
                currentPos = 0;
            }
        }
	}
}
