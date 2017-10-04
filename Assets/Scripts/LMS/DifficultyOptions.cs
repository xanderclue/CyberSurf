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
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing DifficultyOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing DifficultyOptions.rightButton");
        if (null == difficultyText)
            Debug.LogWarning("Missing DifficultyOptions.difficultyText");
    }
    new private void OnEnable()
    {
        base.OnEnable();
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
        if (tempDifficulty - 1 < 0)
            tempDifficulty = GameDifficulties.GameDifficultiesSize - 1;
        else
            --tempDifficulty;
        difficultyText.SetText(tempDifficulty.ToString());
    }
    private void ButtonRightFunction()
    {
        if (tempDifficulty + 1 >= GameDifficulties.GameDifficultiesSize)
            tempDifficulty = 0;
        else
            ++tempDifficulty;
        difficultyText.SetText(tempDifficulty.ToString());
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempDifficulty = GameManager.instance.gameDifficulty.currentDifficulty;
        difficultyText.SetText(tempDifficulty.ToString());
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
        difficultyText.SetText(tempDifficulty.ToString());
    }
}