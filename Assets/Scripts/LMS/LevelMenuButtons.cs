using UnityEngine;
public class LevelMenuButtons : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton confirmButton = null, defaultButton = null, exitButton = null;
    new private void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
        exitButton.OnButtonPressed += LevelMenuScript.ExitMenu;
    }
    private void OnDisable()
    {
        exitButton.OnButtonPressed -= LevelMenuScript.ExitMenu;
    }
}