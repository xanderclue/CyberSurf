using UnityEngine;
public class closeScoreSplashScreen : SelectedObject
{
    private static char[] currentName = new char[3];
    [SerializeField] private Name nameDisplay = null;
    [SerializeField] private GameObject panel = null;
    [SerializeField] private TopScoreDisplay topScoreDisplay = null;
    private int lastScoreLocation = 0;
    new private void Start()
    {
        base.Start();
        if (-1 == GameManager.lastPortalBuildIndex)
        {
            gameObject.SetActive(false);
            panel.SetActive(false);
            if (!keepPlayerStill.tutorialOn)
                GameManager.player.GetComponent<PlayerMenuController>().ToggleMenuMovement(false);
        }
        if (0 != currentName[0])
            nameDisplay.NameString = new string(currentName);
    }
    protected override void SuccessFunction()
    {
        GameManager.player.GetComponent<PlayerMenuController>().ToggleMenuMovement(false);
        gameObject.SetActive(false);
        panel.SetActive(false);
        int i, j;
        switch (GameManager.gameMode)
        {
            case GameMode.Continuous:
                for (i = 0; i < ScoreManager.topContinuousScores.Length; ++i)
                {
                    if (ScoreManager.topContinuousScores[i].isLastScoreInput)
                    {
                        lastScoreLocation = i;
                        ScoreManager.topContinuousScores[i].isLastScoreInput = false;
                        break;
                    }
                }
                if (GameManager.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
                {
                    string name = nameDisplay.NameString;
                    ScoreManager.topContinuousScores[lastScoreLocation].name = name;
                    currentName[0] = name[0];
                    currentName[1] = name[1];
                    currentName[2] = name[2];
                }
                break;
            case GameMode.Cursed:
                for (i = 0; i < ScoreManager.topCursedScores.Length; ++i)
                {
                    for (j = 0; j < ScoreManager.topCursedScores[i].cursedScores.Length; ++j)
                        if (ScoreManager.topCursedScores[i].cursedScores[j].isLastScoreInput)
                        {
                            lastScoreLocation = j;
                            ScoreManager.topCursedScores[i].cursedScores[j].isLastScoreInput = false;
                            break;
                        }
                    if (j < ScoreManager.topCursedScores[i].cursedScores.Length - 1)
                        break;
                }
                if (GameManager.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
                {
                    string name = nameDisplay.NameString;
                    ScoreManager.topCursedScores[GameManager.lastPortalBuildIndex].cursedScores[lastScoreLocation].name = name;
                    currentName[0] = name[0];
                    currentName[1] = name[1];
                    currentName[2] = name[2];
                }
                break;
            case GameMode.Race:
                for (i = 0; i < ScoreManager.topRaceScores.Length; ++i)
                {
                    for (j = 0; j < ScoreManager.topRaceScores[i].racescores.Length; ++j)
                        if (ScoreManager.topRaceScores[i].racescores[j].isLastScoreInput)
                        {
                            lastScoreLocation = j;
                            ScoreManager.topRaceScores[i].racescores[j].isLastScoreInput = false;
                            break;
                        }
                    if (j < ScoreManager.topRaceScores[i].racescores.Length - 1)
                        break;
                }
                if (GameManager.lastPortalBuildIndex >= LevelSelectOptions.LevelBuildOffset)
                {
                    string name = nameDisplay.NameString;
                    ScoreManager.topRaceScores[GameManager.lastPortalBuildIndex].racescores[lastScoreLocation].name = name;
                    currentName[0] = name[0];
                    currentName[1] = name[1];
                    currentName[2] = name[2];
                }
                break;
        }
        topScoreDisplay.StartScoreUpdate();
    }
}