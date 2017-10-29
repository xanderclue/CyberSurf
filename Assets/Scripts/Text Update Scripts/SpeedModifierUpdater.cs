using UnityEngine;
using TMPro;
public class SpeedModifierUpdater : MonoBehaviour
{
    private PlayerGameplayController pgc = null;
    private TextMeshProUGUI element = null;
    private int display = 0;
    private void Start()
    {
        pgc = GetComponentInParent<PlayerGameplayController>();
        element = GetComponent<TextMeshProUGUI>();
        element.color = Color.magenta;
    }
    private void Update()
    {
        display = (int)pgc.DebugSpeedIncrease;
        if (0 != display)
        {
            if (!element.IsActive())
                element.enabled = true;
            element.SetText(display.ToString());
        }
        else
        {
            if (element.IsActive())
            {
                element.SetText("");
                element.enabled = false;
            }
        }
    }
}