using UnityEngine;
public class HudMenuTab : MenuTab
{
    public enum HudMenuOption { OverallHud, Reticle, Speed, Timer, Score, Players, Compass, Arrow, LapCounter, Position, Opacity, Color }
    [SerializeField]
    HudOnOffObject overallHud = null, reticle = null,
        speed = null, timer = null, score = null,
        players = null, compass = null,
        arrow = null, lapCounter = null, position = null;
    private void Start()
    {
        ShowNullWarnings();
    }
    private void OnEnable()
    {
        overallHud.OnValueChanged += TurnOnAll;
        overallHud.OnValueChanged += UpdateHudPreview;
        reticle.OnValueChanged += UpdateHudPreview;
        speed.OnValueChanged += UpdateHudPreview;
        timer.OnValueChanged += UpdateHudPreview;
        score.OnValueChanged += UpdateHudPreview;
        players.OnValueChanged += UpdateHudPreview;
        compass.OnValueChanged += UpdateHudPreview;
        arrow.OnValueChanged += UpdateHudPreview;
        lapCounter.OnValueChanged += UpdateHudPreview;
        position.OnValueChanged += UpdateHudPreview;
    }
    private void OnDisable()
    {
        overallHud.OnValueChanged -= TurnOnAll;
        overallHud.OnValueChanged -= UpdateHudPreview;
        reticle.OnValueChanged -= UpdateHudPreview;
        speed.OnValueChanged -= UpdateHudPreview;
        timer.OnValueChanged -= UpdateHudPreview;
        score.OnValueChanged -= UpdateHudPreview;
        players.OnValueChanged -= UpdateHudPreview;
        compass.OnValueChanged -= UpdateHudPreview;
        arrow.OnValueChanged -= UpdateHudPreview;
        lapCounter.OnValueChanged -= UpdateHudPreview;
        position.OnValueChanged -= UpdateHudPreview;
    }
    private void UpdateHudPreview()
    {

    }
    public void TurnOnAll()
    {
        reticle.TurnOn();
        speed.TurnOn();
        timer.TurnOn();
        score.TurnOn();
        players.TurnOn();
        compass.TurnOn();
        arrow.TurnOn();
        lapCounter.TurnOn();
        position.TurnOn();
    }
    public void TurnOffAll()
    {
        reticle.TurnOff();
        speed.TurnOff();
        timer.TurnOff();
        score.TurnOff();
        players.TurnOff();
        compass.TurnOff();
        arrow.TurnOff();
        lapCounter.TurnOff();
        position.TurnOff();
    }
    private void ShowNullWarnings()
    {
        if (null == overallHud)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Overall HUD object");
        if (null == reticle)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Reticle object");
        if (null == speed)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Speed object");
        if (null == timer)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Timer object");
        if (null == score)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Score object");
        if (null == players)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Players object");
        if (null == compass)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Compass object");
        if (null == arrow)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Arrow object");
        if (null == lapCounter)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Lap Counter object");
        if (null == position)
            Debug.LogWarning("Menu System: HudMenuTab missing reference to Position object");
    }
}