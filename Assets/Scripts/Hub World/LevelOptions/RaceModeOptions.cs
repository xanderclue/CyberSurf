using UnityEngine;
using TMPro;
public class RaceModeOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField] private TextMeshPro raceModeText = null;
    [SerializeField] private GameMode defaultMode = GameMode.Continuous;
    private GameMode tempMode = GameMode.Continuous;
    private void OnEnable()
    {
        leftButton.OnButtonPressed += ButtonLeftFunction;
        rightButton.OnButtonPressed += ButtonRightFunction;
    }
    private void OnDisable()
    {
        leftButton.OnButtonPressed -= ButtonLeftFunction;
        rightButton.OnButtonPressed -= ButtonRightFunction;
    }
    private void ButtonLeftFunction()
    {
        if (tempMode - 1 < 0)
            tempMode = GameMode.GameModesSize - 1;
        else
            --tempMode;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        if (tempMode + 1 >= GameMode.GameModesSize)
            tempMode = 0;
        else
            ++tempMode;
        UpdateDisplay();
    }
    public override void DefaultOptions()
    {
        base.DefaultOptions();
        tempMode = defaultMode;
        UpdateDisplay();
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        GameManager.gameMode = tempMode;
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempMode = GameManager.gameMode;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        raceModeText.SetText(tempMode.ToString());
        switch (tempMode)
        {
            case GameMode.Continuous:
                LevelMenuScript.lapsOptions.DisableGroup();
                LevelMenuScript.aiOptions.DisableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                break;
            case GameMode.Cursed:
                LevelMenuScript.lapsOptions.EnableGroup();
                LevelMenuScript.aiOptions.DisableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                break;
            case GameMode.Free:
                LevelMenuScript.lapsOptions.DisableGroup();
                LevelMenuScript.aiOptions.DisableGroup();
                LevelMenuScript.difficultyOptions.DisableGroup();
                break;
            case GameMode.Race:
                LevelMenuScript.lapsOptions.EnableGroup();
                LevelMenuScript.aiOptions.EnableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                break;
        }
    }
}