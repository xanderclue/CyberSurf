using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class closeScoreSplashScreen : SelectedObject
{
    [SerializeField] TextMeshPro lOne = null;
    [SerializeField] TextMeshPro lTwo = null;
    [SerializeField] TextMeshPro lThree = null;


    public GameObject panel;


    ScoreManager scoreScript;
    GameManager gameManager;
    int lastLevel;

    int lastScoreLocation;

    private void Start()
    {
        gameManager = GameManager.instance;
        scoreScript = gameManager.scoreScript;
        if (gameManager.lastLevel == -1)
        {
            gameObject.SetActive(false);
            panel.SetActive(false);
        }
    }

    override public void selectSuccessFunction()
    {
        GameManager.player.GetComponent<PlayerMenuController>().ToggleMenuMovement(false);
        gameObject.SetActive(false);
        panel.SetActive(false);

        switch (gameManager.gameMode.currentMode)
        {
            case GameModes.Continuous:
                for (int i = 0; i < scoreScript.topContinuousScores.Length; i++)
                {
                    if (scoreScript.topContinuousScores[i].isLastScoreInput)
                    {
                        lastScoreLocation = i;
                        break;
                    }
                }
                lastLevel = gameManager.lastLevel;
                if (lastLevel != -1)
                {
                    scoreScript.topContinuousScores[lastScoreLocation].name 
                        = lOne.text + lTwo.text + lThree.text;
                }
                break;

            case GameModes.Cursed:

                for (int i = 0; i < scoreScript.topCurseScores.Length; i++)
                {
                    int j = 0;
                    for (; j < scoreScript.topCurseScores[i].curseScores.Length; j++)
                    {
                        if (scoreScript.topCurseScores[i].curseScores[j].isLastScoreInput)
                        {
                            lastScoreLocation = j;
                            break;
                        }
                    }
                    if (j < scoreScript.topCurseScores[i].curseScores.Length - 1)
                    {
                        break;
                    }
                }
                
                lastLevel = gameManager.lastLevel;
                if (lastLevel != -1)
                {
                    scoreScript.topCurseScores[lastLevel].curseScores[lastScoreLocation].name
                        = lOne.text + lTwo.text + lThree.text;
                }
                break;

            case GameModes.Free:
                Debug.Log("Free Mode shouldnt display anything");
                break;

            case GameModes.GameModesSize:
                break;

            default:
                break;
        }
    }
}
