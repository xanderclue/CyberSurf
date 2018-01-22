using UnityEngine;
using TMPro;
public class RaceModeOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField] private TextMeshPro raceModeText = null;
    [SerializeField] private GameMode defaultMode = GameMode.Continuous;
    private GameMode tempMode = GameMode.Continuous;
    //[SerializeField] private TooltipObject tooltipDescription;
    //SelectedObject selectObject;
    private void Awake()
    {
    // selectObject = GetComponent<SelectedObject>();

    }
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
                //   selectObject.tooltipText = "Continuously race through each level as you complete a track.";

                LevelMenuScript.lapsOptions.DisableGroup();
                LevelMenuScript.aiOptions.EnableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                break;
            case GameMode.Cursed:
                //  selectObject.tooltipText = "Race the clock to get the high score. Rings add time and score.";

                LevelMenuScript.lapsOptions.EnableGroup();
                LevelMenuScript.aiOptions.DisableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                break;
            case GameMode.Free:
                //  selectObject.tooltipText = "Take to the skies and explore the environments at your leisure.";

                LevelMenuScript.lapsOptions.DisableGroup();
                LevelMenuScript.aiOptions.DisableGroup();
                LevelMenuScript.difficultyOptions.DisableGroup();
                break;
            case GameMode.Race:
                //     selectObject.tooltipText = "Race for position against AI opponents for multiple laps.";

                LevelMenuScript.lapsOptions.EnableGroup();
                LevelMenuScript.aiOptions.EnableGroup();
                LevelMenuScript.difficultyOptions.EnableGroup();
                break;
        }
    }
}