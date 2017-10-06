using UnityEngine;
public class HudMenuTab : MenuTab
{
    public enum HudMenuOption { OverallHud, Reticle, Speed, Timer, Score, Players, Compass, Arrow, LapCounter, Position, Opacity, Color }
    [SerializeField]
    private HudOnOffObject overallHud = null, reticle = null,
        speed = null, timer = null, score = null,
        players = null, compass = null,
        arrow = null, lapCounter = null, position = null;
    [SerializeField]
    private EventSelectedObject confirmButton = null, defaultButton = null, revertButton = null;
    [SerializeField]
    private Material theActiveMaterial = null, theInactiveMaterial = null;
    public static Material activeMaterial = null, inactiveMaterial = null;
    new private void Awake()
    {
        base.Awake();
        if (null != theActiveMaterial)
            activeMaterial = theActiveMaterial;
        if (null != theInactiveMaterial)
            inactiveMaterial = theInactiveMaterial;
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
        confirmButton.OnSelectSuccess += ConfirmAll;
        defaultButton.OnSelectSuccess += DefaultAll;
        revertButton.OnSelectSuccess += ResetAll;
        ResetAll();
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
        confirmButton.OnSelectSuccess -= ConfirmAll;
        defaultButton.OnSelectSuccess -= DefaultAll;
        revertButton.OnSelectSuccess -= ResetAll;
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
    public void ConfirmAll()
    {
        reticle.ConfirmValue();
        speed.ConfirmValue();
        timer.ConfirmValue();
        score.ConfirmValue();
        players.ConfirmValue();
        compass.ConfirmValue();
        arrow.ConfirmValue();
        lapCounter.ConfirmValue();
        position.ConfirmValue();
    }
    public void DefaultAll()
    {
        reticle.DefaultValue();
        speed.DefaultValue();
        timer.DefaultValue();
        score.DefaultValue();
        players.DefaultValue();
        compass.DefaultValue();
        arrow.DefaultValue();
        lapCounter.DefaultValue();
        position.DefaultValue();
    }
    public void ResetAll()
    {
        reticle.ResetValue();
        speed.ResetValue();
        timer.ResetValue();
        score.ResetValue();
        players.ResetValue();
        compass.ResetValue();
        arrow.ResetValue();
        lapCounter.ResetValue();
        position.ResetValue();
    }
}