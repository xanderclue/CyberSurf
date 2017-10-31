using UnityEngine;
using TMPro;
using Xander.Debugging;
public class TimeOfDayOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton leftButton = null, rightButton = null;
    [SerializeField] private TextMeshPro timeOfDayText = null;
    [SerializeField] private DaylightScript.TimeOfDay defaultTimeOfDay = DaylightScript.TimeOfDay.Evening;
    private DaylightScript.TimeOfDay tempTimeOfDay;

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
        if (0 == tempTimeOfDay)
            tempTimeOfDay = DaylightScript.TimeOfDay.NumTimesOfDay - 1;
        else
            --tempTimeOfDay;
        UpdateDisplay();
    }
    private void ButtonRightFunction()
    {
        ++tempTimeOfDay;
        if (DaylightScript.TimeOfDay.NumTimesOfDay == tempTimeOfDay)
            tempTimeOfDay = 0;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        switch (tempTimeOfDay)
        {
            case DaylightScript.TimeOfDay.Noon:
                timeOfDayText.SetText("12:00 PM (Noon)");
                break;
            case DaylightScript.TimeOfDay.Afternoon:
                timeOfDayText.SetText("4:20 PM (Afternoon)");
                break;
            case DaylightScript.TimeOfDay.Evening:
                timeOfDayText.SetText("8:17 PM (Evening)");
                break;
            case DaylightScript.TimeOfDay.Midnight:
                timeOfDayText.SetText("12:00 AM (Midnight)");
                break;
            case DaylightScript.TimeOfDay.Morning:
                timeOfDayText.SetText("9:00 AM (Morning)");
                break;
            default:
                Debug.LogWarning("Switch statement on TimeOfDay enum tempTimeOfDay in TimeOfDayOptions.cs is missing case for TimeOfDay." + tempTimeOfDay.ToString() + this.Info(), this);
                break;
        }
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        DaylightScript.currentTimeOfDay = tempTimeOfDay;
    }
    public override void DefaultOptions()
    {
        base.DefaultOptions();
        tempTimeOfDay = defaultTimeOfDay;
        UpdateDisplay();
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempTimeOfDay = DaylightScript.currentTimeOfDay;
        UpdateDisplay();
    }
}