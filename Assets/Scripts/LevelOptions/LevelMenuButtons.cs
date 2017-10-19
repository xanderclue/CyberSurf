using UnityEngine;
public class LevelMenuButtons : LevelMenuObjectGroup
{
    [SerializeField]
    TopScoreDisplay topScoreDisplay;

    [SerializeField]
    private LevelMenuButton confirmButton = null, defaultButton = null, exitButton = null;

    private void OnEnable()
    {
        confirmButton.OnButtonPressed += ButtonConfirmFunction;
        defaultButton.OnButtonPressed += ButtonDefaultFunction;
        exitButton.OnButtonPressed += ButtonExitFunction;
    }
    private void OnDisable()
    {
        confirmButton.OnButtonPressed -= ButtonConfirmFunction;
        defaultButton.OnButtonPressed -= ButtonDefaultFunction;
        exitButton.OnButtonPressed -= ButtonExitFunction;
    }
    private void ButtonConfirmFunction()
    {
        topScoreDisplay.StartScoreUpdate();
        LevelMenuScript.ConfirmOptions();
    }
    private void ButtonDefaultFunction()
    {
        topScoreDisplay.StartScoreUpdate();
        LevelMenuScript.DefaultOptions();
    }
    private void ButtonExitFunction()
    {
        topScoreDisplay.StartScoreUpdate();
        LevelMenuScript.ExitMenu();
    }
}