using UnityEngine;
public class GameplayMenuTab : MenuTab
{
    [SerializeField] private OnOffButtons controller, respawn, ringPath, predictiveTrail;
    private void OnEnable()
    {
        controller.OnButtonPressed += ControllerOnOff;
        respawn.OnButtonPressed += RespawnOnOff;
        ringPath.OnButtonPressed += RingPathOnOff;
        predictiveTrail.OnButtonPressed += PredictiveTrailOnOff;
        controller.SetValue(GameManager.instance.boardScript.gamepadEnabled);
        respawn.SetValue(GameManager.instance.scoreScript.respawnEnabled);
        ringPath.SetValue(GameManager.instance.levelScript.RingPathIsOn);
        predictiveTrail.SetValue(trailStripCreator.inst.PredictivePathEnabled);
    }
    private void OnDisable()
    {
        controller.OnButtonPressed -= ControllerOnOff;
        respawn.OnButtonPressed -= RespawnOnOff;
        ringPath.OnButtonPressed -= RingPathOnOff;
        predictiveTrail.OnButtonPressed -= PredictiveTrailOnOff;
    }
    private void ControllerOnOff(bool isOn)
    { GameManager.instance.boardScript.UpdateControlsType(isOn); }
    private void RespawnOnOff(bool isOn)
    { GameManager.instance.scoreScript.respawnEnabled = isOn; }
    private void RingPathOnOff(bool isOn)
    { GameManager.instance.levelScript.RingPathIsOn = isOn; }
    private void PredictiveTrailOnOff(bool isOn)
    { trailStripCreator.inst.PredictivePathEnabled = isOn; }
}