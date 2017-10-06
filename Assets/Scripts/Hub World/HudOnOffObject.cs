using UnityEngine;
public class HudOnOffObject : MonoBehaviour
{
    public HudMenuTab.HudMenuOption menuOption = HudMenuTab.HudMenuOption.OverallHud;
    public EventSelectedObject onButton = null, offButton = null;
    private MeshRenderer onButtonRenderer = null, offButtonRenderer = null;
    [SerializeField]
    private bool defaultOnOffValue = true;
    private bool tempOnOffValue;
    public bool IsOn { get { return tempOnOffValue; } }
    [SerializeField]
    private Material activeMaterial = null, inactiveMaterial = null;
    private void Start()
    {
        if (null == inactiveMaterial)
            if (null != HudMenuTab.inactiveMaterial)
                inactiveMaterial = HudMenuTab.inactiveMaterial;
            else
                HudMenuTab.inactiveMaterial = inactiveMaterial = (defaultOnOffValue ? offButtonRenderer : onButtonRenderer).material;
        if (null == activeMaterial)
            if (null != HudMenuTab.activeMaterial)
                activeMaterial = HudMenuTab.activeMaterial;
            else
                HudMenuTab.activeMaterial = activeMaterial = (defaultOnOffValue ? onButtonRenderer : offButtonRenderer).material;
        UpdateButtonDisplay();
    }
    protected void Awake()
    {
        onButtonRenderer = onButton.GetComponent<MeshRenderer>();
        offButtonRenderer = offButton.GetComponent<MeshRenderer>();
    }
    protected void OnEnable()
    {
        onButton.OnSelectSuccess += TurnOn;
        offButton.OnSelectSuccess += TurnOff;
        OnValueChanged += UpdateButtonDisplay;
    }
    protected void OnDisable()
    {
        onButton.OnSelectSuccess -= TurnOn;
        offButton.OnSelectSuccess -= TurnOff;
        OnValueChanged -= UpdateButtonDisplay;
    }
    private void UpdateButtonDisplay()
    {
        if (tempOnOffValue)
        {
            onButtonRenderer.material = activeMaterial;
            offButtonRenderer.material = inactiveMaterial;
        }
        else
        {
            onButtonRenderer.material = inactiveMaterial;
            offButtonRenderer.material = activeMaterial;
        }
    }
    public void TurnOn()
    {
        tempOnOffValue = true;
        if (null != OnValueChanged)
            OnValueChanged();
    }
    public void TurnOff()
    {
        tempOnOffValue = false;
        if (null != OnValueChanged)
            OnValueChanged();
    }
    public void DefaultValue()
    {
        if (tempOnOffValue != defaultOnOffValue)
        {
            tempOnOffValue = defaultOnOffValue;
            if (null != OnValueChanged)
                OnValueChanged();
        }
    }
    public void ResetValue()
    {
        if (tempOnOffValue != ActualValue)
        {
            tempOnOffValue = ActualValue;
            if (null != OnValueChanged)
                OnValueChanged();
        }
    }
    public void ConfirmValue()
    {
        ActualValue = tempOnOffValue;
    }
    public delegate void ValueChangedEvent();
    public ValueChangedEvent OnValueChanged;
    private TextElementControllerScript textElementController = null;
    private bool ActualValue
    {
        get
        {
            textElementController = textElementController ?? GameManager.player.GetComponentInChildren<TextElementControllerScript>();
            switch (menuOption)
            {
                case HudMenuTab.HudMenuOption.OverallHud:
                    return textElementController.hudElementsControl.overAllBool;
                case HudMenuTab.HudMenuOption.Speed:
                    return textElementController.hudElementsControl.speedBool;
                case HudMenuTab.HudMenuOption.Timer:
                    return textElementController.hudElementsControl.timerBool;
                case HudMenuTab.HudMenuOption.Score:
                    return textElementController.hudElementsControl.scoreBool;
                case HudMenuTab.HudMenuOption.Arrow:
                    return textElementController.hudElementsControl.arrowBool;
                case HudMenuTab.HudMenuOption.Reticle:
                case HudMenuTab.HudMenuOption.Players:
                case HudMenuTab.HudMenuOption.Compass:
                case HudMenuTab.HudMenuOption.LapCounter:
                case HudMenuTab.HudMenuOption.Position:
                case HudMenuTab.HudMenuOption.Opacity:
                case HudMenuTab.HudMenuOption.Color:
                default:
                    return defaultOnOffValue;
            }
        }
        set
        {
            textElementController = textElementController ?? GameManager.player.GetComponentInChildren<TextElementControllerScript>();
            switch (menuOption)
            {
                case HudMenuTab.HudMenuOption.OverallHud:
                    textElementController.setAll(value);
                    break;
                case HudMenuTab.HudMenuOption.Speed:
                    textElementController.setSpeed(value);
                    textElementController.setSpeedBar(value);
                    break;
                case HudMenuTab.HudMenuOption.Timer:
                    textElementController.setTimer(value);
                    textElementController.setCheckpoint_time(value);
                    textElementController.setCurrentLapTime(value);
                    textElementController.setBestLap(value);
                    textElementController.setTimeDifference(value);
                    break;
                case HudMenuTab.HudMenuOption.Score:
                    textElementController.setScore(value);
                    textElementController.setScoreMulti(value);
                    break;
                case HudMenuTab.HudMenuOption.Arrow:
                    textElementController.setArrow(value);
                    break;
                case HudMenuTab.HudMenuOption.Reticle:
                case HudMenuTab.HudMenuOption.Players:
                case HudMenuTab.HudMenuOption.Compass:
                case HudMenuTab.HudMenuOption.LapCounter:
                case HudMenuTab.HudMenuOption.Position:
                case HudMenuTab.HudMenuOption.Opacity:
                case HudMenuTab.HudMenuOption.Color:
                default:
                    defaultOnOffValue = value;
                    break;
            }
        }
    }
}