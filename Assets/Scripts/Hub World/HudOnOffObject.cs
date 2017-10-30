using UnityEngine;
public class HudOnOffObject : MonoBehaviour
{
    public HudMenuTab.HudMenuOption menuOption = HudMenuTab.HudMenuOption.OverallHud;
    public EventSelectedObject onButton = null, offButton = null;
    [SerializeField] private bool defaultOnOffValue = true;
    [SerializeField] private Material activeMaterial = null, inactiveMaterial = null;
    private MeshRenderer onButtonRenderer = null, offButtonRenderer = null;
    private bool tempOnOffValue = false;
    private TextElementControllerScript textElementController = null;
    public delegate void ValueChangedEvent();
    public event ValueChangedEvent OnValueChanged;
    public bool IsOn { get { return tempOnOffValue; } }
    protected void Start()
    {
        inactiveMaterial = inactiveMaterial ??
            HudMenuTab.inactiveMaterial ??
            (HudMenuTab.inactiveMaterial = (defaultOnOffValue ? offButtonRenderer : onButtonRenderer).material);
        activeMaterial = activeMaterial ??
            HudMenuTab.activeMaterial ??
            (HudMenuTab.activeMaterial = (defaultOnOffValue ? onButtonRenderer : offButtonRenderer).material);
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
        OnValueChanged?.Invoke();
    }
    public void TurnOff()
    {
        tempOnOffValue = false;
        OnValueChanged?.Invoke();
    }
    public void DefaultValue()
    {
        if (tempOnOffValue != defaultOnOffValue)
        {
            tempOnOffValue = defaultOnOffValue;
            OnValueChanged?.Invoke();
        }
    }
    public void ResetValue()
    {
        if (tempOnOffValue != ActualValue)
        {
            tempOnOffValue = ActualValue;
            OnValueChanged?.Invoke();
        }
    }
    public void ConfirmValue()
    {
        ActualValue = tempOnOffValue;
    }
    private bool ActualValue
    {
        get
        {
            if (null == textElementController)
                textElementController = GameManager.player.GetComponentInChildren<TextElementControllerScript>();
            switch (menuOption)
            {
                case HudMenuTab.HudMenuOption.OverallHud:
                    return false;
                case HudMenuTab.HudMenuOption.Speed:
                    return textElementController.hudElementsControl.speedBool;
                case HudMenuTab.HudMenuOption.Timer:
                    return textElementController.hudElementsControl.timerBool;
                case HudMenuTab.HudMenuOption.Score:
                    return textElementController.hudElementsControl.scoreBool;
                case HudMenuTab.HudMenuOption.Arrow:
                    return textElementController.hudElementsControl.arrowBool;
                case HudMenuTab.HudMenuOption.Players:
                    return textElementController.hudElementsControl.player_listBool;
                case HudMenuTab.HudMenuOption.LapCounter:
                    return textElementController.hudElementsControl.lapBool;
                case HudMenuTab.HudMenuOption.Position:
                    return textElementController.hudElementsControl.positionBool;
                case HudMenuTab.HudMenuOption.Reticle:
                    return textElementController.hudElementsControl.reticleBool;
                case HudMenuTab.HudMenuOption.Compass:
                    return textElementController.hudElementsControl.compassBool;
                default:
                    Debug.LogWarning("Missing case: \"" + menuOption.ToString("F") + "\"");
                    return defaultOnOffValue;
            }
        }
        set
        {
            if (null == textElementController)
                textElementController = GameManager.player.GetComponentInChildren<TextElementControllerScript>();
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
                case HudMenuTab.HudMenuOption.Players:
                    textElementController.setPlayerList(value);
                    break;
                case HudMenuTab.HudMenuOption.LapCounter:
                    textElementController.setLapText(value);
                    break;
                case HudMenuTab.HudMenuOption.Position:
                    textElementController.setPositionText(value);
                    break;
                case HudMenuTab.HudMenuOption.Reticle:
                    textElementController.setReticle(value);
                    break;
                case HudMenuTab.HudMenuOption.Compass:
                    textElementController.setCompass(value);
                    break;
                default:
                    Debug.LogWarning("Missing case: \"" + menuOption.ToString("F") + "\"");
                    break;
            }
        }
    }
}