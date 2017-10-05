using UnityEngine;
public class HudMenuTab : MenuTab
{
    [SerializeField]
    HudOnOffObject overallHud = null, reticle = null, speed = null, timer = null, score = null, players = null, compass = null, arrow = null, lapCounter = null, position = null;
    private void Start()
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