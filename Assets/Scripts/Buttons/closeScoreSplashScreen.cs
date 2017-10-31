using UnityEngine;
using TMPro;
using Xander.Debugging;
public class closeScoreSplashScreen : SelectedObject
{
    [SerializeField] private TextMeshPro lOne = null;
    [SerializeField] private TextMeshPro lTwo = null;
    [SerializeField] private TextMeshPro lThree = null;
    [SerializeField] private GameObject panel = null;
    [SerializeField] private TopScoreDisplay topScoreDisplay = null;
    private ScoreManager scoreScript = null;
    private GameManager gameManager = null;
    private int lastScoreLocation = 0;
    new private void Start()
    {
        base.Start();
        gameManager = GameManager.instance;
        scoreScript = gameManager.scoreScript;
        if (-1 == gameManager.lastPortalBuildIndex)
        {
            gameObject.SetActive(false);
            panel.SetActive(false);
            if (!keepPlayerStill.tutorialOn)
                GameManager.player.GetComponent<PlayerMenuController>().ToggleMenuMovement(false);
        }
        else if (0 == gameManager.lastPortalBuildIndex)
        {
            gameObject.SetActive(false);
            panel.SetActive(false);
        }
        if (0 != scoreScript.currentName[0])
        {
            lOne.SetText(scoreScript.currentName[0].ToString());
            lTwo.SetText(scoreScript.currentName[1].ToString());
            lThree.SetText(scoreScript.currentName[2].ToString());
        }
    }
    protected override void SuccessFunction()
    {
        GameManager.player.GetComponent<PlayerMenuController>().ToggleMenuMovement(false);
        gameObject.SetActive(false);
        panel.SetActive(false);
        int i, j;
        switch (gameManager.gameMode.currentMode)
        {
            case GameModes.Continuous:
                for (i = 0; i < scoreScript.topContinuousScores.Length; ++i)
                {
                    if (scoreScript.topContinuousScores[i].isLastScoreInput)
                    {
                        lastScoreLocation = i;
                        scoreScript.topContinuousScores[i].isLastScoreInput = false;
                        break;
                    }
                }
                if (gameManager.lastPortalBuildIndex > 1)
                {
                    scoreScript.topContinuousScores[lastScoreLocation].name = lOne.GetParsedText() + lTwo.GetParsedText() + lThree.GetParsedText();
                    scoreScript.currentName[0] = lOne.GetParsedText()[0];
                    scoreScript.currentName[1] = lTwo.GetParsedText()[0];
                    scoreScript.currentName[2] = lThree.GetParsedText()[0];
                }
                break;
            case GameModes.Cursed:
                for (i = 0; i < scoreScript.topCurseScores.Length; ++i)
                {
                    for (j = 0; j < scoreScript.topCurseScores[i].curseScores.Length; ++j)
                        if (scoreScript.topCurseScores[i].curseScores[j].isLastScoreInput)
                        {
                            lastScoreLocation = j;
                            scoreScript.topCurseScores[i].curseScores[j].isLastScoreInput = false;
                            break;
                        }
                    if (j < scoreScript.topCurseScores[i].curseScores.Length - 1)
                        break;
                }
                if (gameManager.lastPortalBuildIndex > 1)
                {
                    scoreScript.topCurseScores[gameManager.lastPortalBuildIndex].curseScores[lastScoreLocation].name = lOne.GetParsedText() + lTwo.GetParsedText() + lThree.GetParsedText();
                    scoreScript.currentName[0] = lOne.GetParsedText()[0];
                    scoreScript.currentName[1] = lTwo.GetParsedText()[0];
                    scoreScript.currentName[2] = lThree.GetParsedText()[0];
                }
                break;
            case GameModes.Free:
                Debug.Log("Free Mode shouldn't display anything" + this.Info(), this);
                break;
            case GameModes.Race:
                Debug.Log("To add Race Case");
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameManager.gameMode.currentMode.ToString("F") + "\"" + this.Info(), this);
                break;
        }
        topScoreDisplay.StartScoreUpdate();
    }
}