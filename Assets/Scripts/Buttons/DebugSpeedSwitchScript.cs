using TMPro;

public class DebugSpeedSwitchScript : SelectedObject
{
    private TextMeshPro debugSpeedOnOffText;
    private BoardManager theManager;
    private bool IsOn { get { return theManager.debugSpeedEnabled; } set { theManager.UpdateDebugSpeedControls(value); } }

    new private void Start()
    {
        base.Start();
        theManager = GameManager.instance.boardScript;
        debugSpeedOnOffText.SetText(IsOn ? "On" : "Off");
    }

    private void OnEnable()
    {
        debugSpeedOnOffText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    override public void SuccessFunction()
    {
        IsOn = !IsOn;
        debugSpeedOnOffText.SetText(IsOn ? "On" : "Off");
    }
}