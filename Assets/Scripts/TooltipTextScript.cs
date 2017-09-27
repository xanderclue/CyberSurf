using UnityEngine;

public class TooltipTextScript : MonoBehaviour
{
    private delegate void UpdateTooltipEvent(string str);
    private static UpdateTooltipEvent OnUpdateTooltip;
    TMPro.TextMeshProUGUI textMesh = null;
    public static void SetText(string str = null)
    {
        BuildDebugger.WriteLine("Setting Tooltip Text: \"" + (str ?? "") + "\"");
        if (null != OnUpdateTooltip)
            OnUpdateTooltip(str);
    }
    private void Awake()
    {
        textMesh = GetComponent<TMPro.TextMeshProUGUI>();
        textMesh.SetText("");
        textMesh.enabled = false;
    }
    private void OnEnable()
    {
        OnUpdateTooltip += UpdateTooltip;
    }
    private void OnDisable()
    {
        OnUpdateTooltip -= UpdateTooltip;
    }
    void UpdateTooltip(string str)
    {
        if (null != textMesh)
        {
            if (null == str || "" == str)
            {
                textMesh.SetText("");
                textMesh.enabled = false;
            }
            else
            {
                textMesh.enabled = true;
                textMesh.SetText(str);
            }
        }
    }
}