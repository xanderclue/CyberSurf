using UnityEngine;
public class TooltipTextScript : MonoBehaviour
{
    private delegate void UpdateTooltipEvent(string str);
    private static event UpdateTooltipEvent OnUpdateTooltip;
    private TMPro.TextMeshProUGUI textMesh = null;
    private float bugFixTimer = 0.0f;
    private const float bugFixTime = 0.1f;
    public static void SetText(string str) => OnUpdateTooltip?.Invoke(str);
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
    private void UpdateTooltip(string str)
    {
        if (null != textMesh)
        {
            if (null == str || "" == str)
            {
                textMesh.SetText("");
                textMesh.enabled = false;
                bugFixTimer = bugFixTime;
            }
            else if (bugFixTimer <= 0.0f)
            {
                textMesh.enabled = true;
                textMesh.SetText(str);
                bugFixTimer = bugFixTime;
            }
        }
    }
    private void Update()
    {
        bugFixTimer -= Time.deltaTime;
    }
}