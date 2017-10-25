public class EventManager
{
    public delegate void IntParamEvent(int sceneIndex);
    public delegate void BoolParamEvent(bool isOn);
    public delegate void VoidParamEvent();
    public static event IntParamEvent OnTransition;
    public static event BoolParamEvent OnToggleMovement, OnSelectionLock, OnToggleHud, OnSetRingPath;
    public static event VoidParamEvent OnUpdateBoardMenuEffects, OnStartRingPulse, OnStopRingPulse;
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