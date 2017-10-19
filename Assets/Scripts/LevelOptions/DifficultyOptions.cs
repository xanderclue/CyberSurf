using UnityEngine;
using TMPro;
public class DifficultyOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField]
    private TextMeshPro difficultyText = null;
    [SerializeField]
    private GameDifficulties defaultDifficulty = GameDifficulties.Normal;
    private GameDifficulties tempDifficulty;
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
    public override void EnableGroup()
    {
        base.EnableGroup();
        leftButton.enabled = true;
        rightButton.enabled = true;
    }
    public override void DisableGroup()
    {
        base.DisableGroup();
        leftButton.enabled = false;
        rightButton.enabled = false;
    }
    private void ButtonLeftFunction()
    {
        if (tempDifficulty - 1 < 0)
            tempDifficulty = GameDifficulties.GameDifficultiesSize - 1;
        else
            --tempDifficulty;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        if (tempDifficulty + 1 >= GameDifficulties.GameDifficultiesSize)
            tempDifficulty = 0;
        else
            ++tempDifficulty;
        UpdateDisplay();
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempDifficulty = GameManager.instance.gameDifficulty.currentDifficulty;
        UpdateDisplay();
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        GameManager.instance.gameDifficulty.currentDifficulty = tempDifficulty;
    }
    public override void DefaultOptions()
    {
        base.ConfirmOptions();
        tempDifficulty = defaultDifficulty;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        difficultyText.SetText(tempDifficulty.ToString());
    }
}