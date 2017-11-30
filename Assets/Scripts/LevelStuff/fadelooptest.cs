using UnityEngine;
public class fadelooptest : MonoBehaviour
{
    [SerializeField] private bool loopFade = false;
    [SerializeField] private float length = 2.0f;
    [SerializeField] private bool reset = false;
    private float fadeout = 1.0f, timeIntoFade = 0.0f, alpha = 0.0f;
    private Renderer theRenderer = null;
    private void Awake()
    {
        theRenderer = GetComponent<Renderer>();
    }
    private void FixedUpdate()
    {
        if (loopFade)
        {
            if (timeIntoFade >= length || timeIntoFade < 0.0f)
                fadeout = -fadeout;
            timeIntoFade += Time.deltaTime * fadeout;
            alpha = timeIntoFade / length;
            if (-1.0f == fadeout)
                alpha = Mathf.Clamp01(alpha);
            theRenderer.material.SetFloat("_AlphaValue", alpha);
        }
        else if (reset && !loopFade)
            theRenderer.material.SetFloat("_AlphaValue", 0.0f);
    }
}