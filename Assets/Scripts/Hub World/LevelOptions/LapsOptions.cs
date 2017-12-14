using UnityEngine;
using TMPro;
public class LapsOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton plusButton = null, minusButton = null;
    [SerializeField] private TextMeshPro lapsText = null;
    [SerializeField] private int defaultValue = 1;
    private int tempValue;
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
        if (tempValue > 1)
            --tempValue;
        UpdateDisplay();
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        GameManager.MaxLap = tempValue;
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
        tempValue = GameManager.MaxLap;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        lapsText.SetText(tempValue.ToString());
    }
}