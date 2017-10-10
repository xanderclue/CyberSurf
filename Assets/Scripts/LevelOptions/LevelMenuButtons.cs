using UnityEngine;
public class LevelMenuButtons : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton confirmButton = null, defaultButton = null, exitButton = null;
    new private void OnEnable()
    {
        base.OnEnable();
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
        LevelMenuScript.ConfirmOptions();
    }
    private void ButtonDefaultFunction()
    {
        LevelMenuScript.DefaultOptions();
    }
    private void ButtonExitFunction()
    {
        LevelMenuScript.ExitMenu();
    }
}