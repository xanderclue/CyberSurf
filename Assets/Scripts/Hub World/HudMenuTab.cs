using UnityEngine;
public class HudMenuTab : MenuTab
{
    public enum HudMenuOption { OverallHud, Reticle, Speed, Timer, ScoreDisplay, Players, Compass, Arrow, LapCounter, Position }
    [SerializeField, LabelOverride("Overall  HUD")] private HudOnOffObject overallHudOnOff = null;
    [SerializeField, LabelOverride("Reticle")] private HudOnOffObject reticleOnOff = null;
    [SerializeField, LabelOverride("Speed")] private HudOnOffObject speedOnOff = null;
    [SerializeField, LabelOverride("Timer")] private HudOnOffObject timerOnOff = null;
    [SerializeField, LabelOverride("Score")] private HudOnOffObject scoreOnOff = null;
    [SerializeField, LabelOverride("Players")] private HudOnOffObject playersOnOff = null;
    [SerializeField, LabelOverride("Compass")] private HudOnOffObject compassOnOff = null;
    [SerializeField, LabelOverride("Arrow")] private HudOnOffObject arrowOnOff = null;
    [SerializeField, LabelOverride("Lap Counter")] private HudOnOffObject lapCounterOnOff = null;
    [SerializeField, LabelOverride("Position")] private HudOnOffObject positionOnOff = null;
    [SerializeField] private HudOptionsOpacityColor colorAndOpacity = null;
    [SerializeField] private EventSelectedObject confirmButton = null;
    [SerializeField] private EventSelectedObject defaultButton = null;
    [SerializeField] private EventSelectedObject revertButton = null;
    [SerializeField, LabelOverride("Active Material")] private Material theActiveMaterial = null;
    [SerializeField, LabelOverride("Inactive Material")] private Material theInactiveMaterial = null;
    public static Material activeMaterial = null;
    public static Material inactiveMaterial = null;
    [SerializeField] private HudPreview hudPreview = null;
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
        overallHudOnOff.OnValueChanged += OverallChange;
        reticleOnOff.OnValueChanged += UpdateHudPreview;
        speedOnOff.OnValueChanged += UpdateHudPreview;
        timerOnOff.OnValueChanged += UpdateHudPreview;
        scoreOnOff.OnValueChanged += UpdateHudPreview;
        playersOnOff.OnValueChanged += UpdateHudPreview;
        compassOnOff.OnValueChanged += UpdateHudPreview;
        arrowOnOff.OnValueChanged += UpdateHudPreview;
        lapCounterOnOff.OnValueChanged += UpdateHudPreview;
        positionOnOff.OnValueChanged += UpdateHudPreview;
        colorAndOpacity.OnValueChanged += UpdateHudPreview;
        confirmButton.OnSelectSuccess += ConfirmAll;
        defaultButton.OnSelectSuccess += DefaultAll;
        revertButton.OnSelectSuccess += ResetAll;
        ResetAll();
    }
    private void OnDisable()
    {
        overallHudOnOff.OnValueChanged -= OverallChange;
        reticleOnOff.OnValueChanged -= UpdateHudPreview;
        speedOnOff.OnValueChanged -= UpdateHudPreview;
        timerOnOff.OnValueChanged -= UpdateHudPreview;
        scoreOnOff.OnValueChanged -= UpdateHudPreview;
        playersOnOff.OnValueChanged -= UpdateHudPreview;
        compassOnOff.OnValueChanged -= UpdateHudPreview;
        arrowOnOff.OnValueChanged -= UpdateHudPreview;
        lapCounterOnOff.OnValueChanged -= UpdateHudPreview;
        positionOnOff.OnValueChanged -= UpdateHudPreview;
        colorAndOpacity.OnValueChanged -= UpdateHudPreview;
        confirmButton.OnSelectSuccess -= ConfirmAll;
        defaultButton.OnSelectSuccess -= DefaultAll;
        revertButton.OnSelectSuccess -= ResetAll;
    }
    private void OverallChange()
    {
        if (overallHudOnOff.IsOn) TurnOnAll(); else TurnOffAll();
        UpdateHudPreview();
    }
    private void UpdateHudPreview()
    {
        hudPreview.reticleOnOff = reticleOnOff.IsOn;
        hudPreview.speedOnOff = speedOnOff.IsOn;
        hudPreview.timerOnOff = timerOnOff.IsOn;
        hudPreview.scoreOnOff = scoreOnOff.IsOn;
        hudPreview.playersOnOff = playersOnOff.IsOn;
        hudPreview.compassOnOff = compassOnOff.IsOn;
        hudPreview.arrowOnOff = arrowOnOff.IsOn;
        hudPreview.lapCounterOnOff = lapCounterOnOff.IsOn;
        hudPreview.positionOnOff = positionOnOff.IsOn;
        hudPreview.color = colorAndOpacity.ColorPreviewValue;
        hudPreview.color.a = colorAndOpacity.OpacityValue;
        hudPreview.UpdatePreview();
    }
    public void TurnOnAll()
    {
        reticleOnOff.TurnOn();
        speedOnOff.TurnOn();
        timerOnOff.TurnOn();
        scoreOnOff.TurnOn();
        playersOnOff.TurnOn();
        compassOnOff.TurnOn();
        arrowOnOff.TurnOn();
        lapCounterOnOff.TurnOn();
        positionOnOff.TurnOn();
    }
    public void TurnOffAll()
    {
        reticleOnOff.TurnOff();
        speedOnOff.TurnOff();
        timerOnOff.TurnOff();
        scoreOnOff.TurnOff();
        playersOnOff.TurnOff();
        compassOnOff.TurnOff();
        arrowOnOff.TurnOff();
        lapCounterOnOff.TurnOff();
        positionOnOff.TurnOff();
    }
    public void ConfirmAll()
    {
        reticleOnOff.ConfirmValue();
        speedOnOff.ConfirmValue();
        timerOnOff.ConfirmValue();
        scoreOnOff.ConfirmValue();
        playersOnOff.ConfirmValue();
        compassOnOff.ConfirmValue();
        arrowOnOff.ConfirmValue();
        lapCounterOnOff.ConfirmValue();
        positionOnOff.ConfirmValue();
        colorAndOpacity.ConfirmValue();
    }
    public void DefaultAll()
    {
        reticleOnOff.DefaultValue();
        speedOnOff.DefaultValue();
        timerOnOff.DefaultValue();
        scoreOnOff.DefaultValue();
        playersOnOff.DefaultValue();
        compassOnOff.DefaultValue();
        arrowOnOff.DefaultValue();
        lapCounterOnOff.DefaultValue();
        positionOnOff.DefaultValue();
        colorAndOpacity.DefaultValue();
    }
    public void ResetAll()
    {
        reticleOnOff.ResetValue();
        speedOnOff.ResetValue();
        timerOnOff.ResetValue();
        scoreOnOff.ResetValue();
        playersOnOff.ResetValue();
        compassOnOff.ResetValue();
        arrowOnOff.ResetValue();
        lapCounterOnOff.ResetValue();
        positionOnOff.ResetValue();
        colorAndOpacity.ResetValue();
    }
}