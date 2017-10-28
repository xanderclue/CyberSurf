using UnityEngine;
public class GameplayMenuTab : MenuTab
{
    [SerializeField] private OnOffButtons controller = null, respawn = null, ringPath = null, predictiveTrail = null;
    [SerializeField] private EventSelectedObject confirmButton = null, defaultButton = null, revertButton = null;
    private void OnEnable()
    {
        confirmButton.OnSelectSuccess += ConfirmOptions;
        defaultButton.OnSelectSuccess += DefaultOptions;
        revertButton.OnSelectSuccess += RevertOptions;
        RevertOptions();
    }
    private void OnDisable()
    {
        confirmButton.OnSelectSuccess -= ConfirmOptions;
        defaultButton.OnSelectSuccess -= DefaultOptions;
        revertButton.OnSelectSuccess -= RevertOptions;
    }
    private void ConfirmOptions()
    {
        GameManager.instance.boardScript.UpdateControlsType(controller.CurrentValue);
        GameManager.instance.scoreScript.respawnEnabled = respawn.CurrentValue;
        GameManager.instance.levelScript.RingPathIsOn = ringPath.CurrentValue;
        trailStripCreator.inst.PredictivePathEnabled = predictiveTrail.CurrentValue;
    }
    private void DefaultOptions()
    {
        controller.CurrentValue = true;
        respawn.CurrentValue = true;
        ringPath.CurrentValue = true;
        predictiveTrail.CurrentValue = true;
    }
    private void RevertOptions()
    {
        controller.CurrentValue = GameManager.instance.boardScript.gamepadEnabled;
        respawn.CurrentValue = GameManager.instance.scoreScript.respawnEnabled;
        ringPath.CurrentValue = GameManager.instance.levelScript.RingPathIsOn;
        predictiveTrail.CurrentValue = trailStripCreator.inst.PredictivePathEnabled;
    }
}