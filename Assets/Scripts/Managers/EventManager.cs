public class EventManager
{
    public delegate void ToggleMovementEvent(bool locked);
    public delegate void TransitionEvent(int sceneIndex);
    public delegate void SelectionLockEvent(bool locked);
    public delegate void ToggleHudEvent(bool isOn);
    public delegate void ToggleArrowEvent(bool isOn);
    public delegate void SetRingPathEvent(bool isOn);
    public delegate void UpdateBoardMenuEffectsEvent();
    public delegate void RingPulseChangeEvent();
    public static event ToggleMovementEvent OnToggleMovement;
    public static event TransitionEvent OnTransition;
    public static event SelectionLockEvent OnSelectionLock;
    public static event ToggleHudEvent OnToggleHud;
    public static event ToggleArrowEvent OnToggleArrow;
    public static event SetRingPathEvent OnSetRingPath;
    public static event UpdateBoardMenuEffectsEvent OnUpdateBoardMenuEffects;
    public static event RingPulseChangeEvent OnStartRingPulse;
    public static event RingPulseChangeEvent OnStopRingPulse;
    static public void OnSetGameplayMovementLock(bool locked)
    {
        if (null != OnToggleMovement)
            OnToggleMovement(locked);
    }
    static public void OnTriggerTransition(int sceneIndex)
    {
        if (null != OnTransition)
            OnTransition(sceneIndex);
    }
    static public void OnTriggerSelectionLock(bool locked)
    {
        if (null != OnSelectionLock)
            OnSelectionLock(locked);
    }
    static public void OnSetHudOnOff(bool isOn)
    {
        if (null != OnToggleHud)
            OnToggleHud(isOn);
    }
    static public void OnSetArrowOnOff(bool isOn)
    {
        if (null != OnToggleArrow)
            OnToggleArrow(isOn);
    }
    static public void OnCallSetRingPath(bool isOn)
    {
        if (null != OnSetRingPath)
            OnSetRingPath(isOn);
    }
    static public void OnCallBoardMenuEffects()
    {
        if (null != OnUpdateBoardMenuEffects)
            OnUpdateBoardMenuEffects();
    }
    public static void StartRingPulse()
    {
        if (OnStartRingPulse != null)
            OnStartRingPulse();
    }
    public static void StopRingPulse()
    {
        if (OnStopRingPulse != null)
            OnStopRingPulse();
    }
}