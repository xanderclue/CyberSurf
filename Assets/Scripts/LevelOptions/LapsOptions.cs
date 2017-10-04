using UnityEngine;
using TMPro;
public class LapsOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton plusButton = null, minusButton = null;
    [SerializeField]
    private TextMeshPro lapsText = null;
    [SerializeField]
    private int defaultValue = 1;
    private int tempValue;
    private static int ActualValue { get; set; } // replace with game's value
    new private void Start()
    {
        base.Start();
        if (null == plusButton)
            Debug.LogWarning("Missing LapsOptions.plusButton");
        if (null == minusButton)
            Debug.LogWarning("Missing LapsOptions.minusButton");
        if (null == lapsText)
            Debug.LogWarning("Missing LapsOptions.lapsText");
        if (ActualValue < 1)
            ActualValue = 1;
    }
    new private void OnEnable()
    {
        base.OnEnable();
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
        lapsText.SetText(tempValue.ToString());
    }
}