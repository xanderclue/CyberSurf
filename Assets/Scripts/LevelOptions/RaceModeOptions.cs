using UnityEngine;
using TMPro;
using Xander.Debugging;
public class RaceModeOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField] private TextMeshPro raceModeText = null;
    [SerializeField] private GameModes defaultMode = GameModes.Continuous;
    private GameModes tempMode = GameModes.Continuous;
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
            tempMode = GameModes.GameModesSize - 1;
        else
            --tempMode;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        if (tempMode + 1 >= GameModes.GameModesSize)
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
        GameManager.instance.gameMode.currentMode = tempMode;
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempMode = GameManager.instance.gameMode.currentMode;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        raceModeText.SetText(tempMode.ToString());
        switch (tempMode)
        {
            case GameModes.Continuous:
                LevelMenuScript.lapsOptions.DisableGroup();
                LevelMenuScript.aiOptions.EnableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                LevelMenuScript.mirrorTrackOptions.DisableGroup();
                LevelMenuScript.reverseTrackOptions.DisableGroup();
                break;
            case GameModes.Cursed:
                LevelMenuScript.lapsOptions.EnableGroup();
                LevelMenuScript.aiOptions.DisableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                LevelMenuScript.mirrorTrackOptions.EnableGroup();
                LevelMenuScript.reverseTrackOptions.EnableGroup();
                break;
            case GameModes.Free:
                LevelMenuScript.lapsOptions.DisableGroup();
                LevelMenuScript.aiOptions.DisableGroup();
                LevelMenuScript.difficultyOptions.DisableGroup();
                LevelMenuScript.mirrorTrackOptions.DisableGroup();
                LevelMenuScript.reverseTrackOptions.DisableGroup();
                break;
            case GameModes.Race:
                Debug.Log("To add race case RaceModeOptions");
                break;
            default:
                Debug.LogWarning("Switch statement on GameModes enum tempMode in RaceModeOptions.cs is missing case for GameModes." + tempMode.ToString() + this.Info(), this);
                break;
        }
    }
}