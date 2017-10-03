using UnityEngine;
public class LevelMenuButtons : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton confirmButton = null, defaultButton = null, exitButton = null;
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
    private void OnEnable()
    {
        exitButton.OnButtonPressed += LevelMenuScript.ExitMenu;
    }
    private void OnDisable()
    {
        exitButton.OnButtonPressed -= LevelMenuScript.ExitMenu;
    }
}