using UnityEngine;
using TMPro;
public class AiOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton plusButton = null, minusButton = null;
    [SerializeField]
    private TextMeshPro aiText = null;
    [SerializeField]
    private int defaultValue = 0;
    private int tempValue;
    private static int ActualValue { get; set; } // replace with game's value
    new private void Start()
    {
        base.Start();
        if (ActualValue < 0)
            ActualValue = 0;
    }
    private void OnEnable()
    {
        plusButton.OnButtonPressed += ButtonPlusFunction;
        minusButton.OnButtonPressed += ButtonMinusFunction;
    }
    private void OnDisable()
    {
        plusButton.OnButtonPressed -= ButtonPlusFunction;
        minusButton.OnButtonPressed -= ButtonMinusFunction;
    }
    public override void EnableGroup()
    {
        base.EnableGroup();
        plusButton.enabled = true;
        minusButton.enabled = true;
    }
    public override void DisableGroup()
    {
        base.DisableGroup();
        plusButton.enabled = false;
        minusButton.enabled = false;
    }
    private void ButtonPlusFunction()
    {
        ++tempValue;
        UpdateDisplay();
    }
    private void ButtonMinusFunction()
    {
        if (tempValue > 0)
            --tempValue;
        UpdateDisplay();
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        ActualValue = tempValue;
    }
    public override void DefaultOptions()
    {
        base.DefaultOptions();
        tempValue = defaultValue;
        UpdateDisplay();
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempValue = ActualValue;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        aiText.SetText(tempValue.ToString());
    }
}