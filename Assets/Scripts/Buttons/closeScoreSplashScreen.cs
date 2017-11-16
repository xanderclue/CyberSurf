using UnityEngine;
using Xander.Debugging;
public class closeScoreSplashScreen : SelectedObject
{
    [SerializeField] private Name nameDisplay = null;
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
            nameDisplay.NameString = new string(scoreScript.currentName);
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
                    string name = nameDisplay.NameString;
                    scoreScript.topContinuousScores[lastScoreLocation].name = name;
                    scoreScript.currentName[0] = name[0];
                    scoreScript.currentName[1] = name[1];
                    scoreScript.currentName[2] = name[2];
                }
                break;
            case GameModes.Cursed:
                for (i = 0; i < scoreScript.topCursedScores.Length; ++i)
                {
                    for (j = 0; j < scoreScript.topCursedScores[i].cursedScores.Length; ++j)
                        if (scoreScript.topCursedScores[i].cursedScores[j].isLastScoreInput)
                        {
                            lastScoreLocation = j;
                            scoreScript.topCursedScores[i].cursedScores[j].isLastScoreInput = false;
                            break;
                        }
                    if (j < scoreScript.topCursedScores[i].cursedScores.Length - 1)
                        break;
                }
                if (gameManager.lastPortalBuildIndex > 1)
                {
                    string name = nameDisplay.NameString;
                    scoreScript.topCursedScores[gameManager.lastPortalBuildIndex].cursedScores[lastScoreLocation].name = name;
                    scoreScript.currentName[0] = name[0];
                    scoreScript.currentName[1] = name[1];
                    scoreScript.currentName[2] = name[2];
                }
                break;
            case GameModes.Free:
                Debug.Log("Free Mode shouldn't display anything" + this.Info(), this);
                break;
            case GameModes.Race:
                Debug.Log("To add Race Case closeScoreSplashScreen");
                break;
            default:
                Debug.LogWarning("Missing case: \"" + gameManager.gameMode.currentMode.ToString("F") + "\"" + this.Info(), this);
                break;
        }
        topScoreDisplay.StartScoreUpdate();
    }
}