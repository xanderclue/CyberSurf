using UnityEngine;
public class LevelMenuButtons : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton confirmButton = null, defaultButton = null, exitButton = null;
    new private void Start()
    {
        base.Start();
        if (null == confirmButton)
            Debug.LogWarning("Missing LevelMenuButtons.confirmButton");
        if (null == defaultButton)
            Debug.LogWarning("Missing LevelMenuButtons.defaultButton");
        if (null == exitButton)
            Debug.LogWarning("Missing LevelMenuButtons.exitButton");
    }
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