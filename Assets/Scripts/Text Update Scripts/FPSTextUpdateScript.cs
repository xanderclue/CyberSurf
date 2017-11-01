using UnityEngine;
using TMPro;
public class FPSTextUpdateScript : MonoBehaviour
{
    private TextMeshProUGUI element = null;
    private uint frameCount = 0u;
    private float dt = 0.0f, fps = 0.0f;
    private const float updateRate = 0.25f;
    private void Start()
    {
        element = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (element.enabled)
        {
            ++frameCount;
            dt += Time.deltaTime;
            if (dt > updateRate)
            {
                fps = frameCount / dt;
                frameCount = 0u;
                dt -= updateRate;
            }
            element.SetText(" " + fps.ToString("n2") + " ");
        }
        if (Input.GetKeyDown(KeyCode.F1))
            element.enabled = !element.enabled;
        if (Input.GetKeyDown(KeyCode.F2))
            element.faceColor = new Color(element.faceColor.r, element.faceColor.g, element.faceColor.b, 0.0f);
    }
}