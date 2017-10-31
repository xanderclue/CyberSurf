using UnityEngine;
using Xander.NullConversion;
public class OnOffButtons : MonoBehaviour
{
    [SerializeField] private EventSelectedObject onButton = null, offButton = null;
    [SerializeField] private MeshRenderer onButtonRenderer = null, offButtonRenderer = null;
    [SerializeField] private Material activeMaterial = null, inactiveMaterial = null;
    private bool currentValue = false;
    public bool CurrentValue
    {
        get { return currentValue; }
        set
        {
            if (null != onButtonRenderer)
                onButtonRenderer.material = value ? activeMaterial : inactiveMaterial;
            if (null != offButtonRenderer)
                offButtonRenderer.material = value ? inactiveMaterial : activeMaterial;
            currentValue = value;
        }
    }
    private void Awake()
    {
        onButtonRenderer = onButtonRenderer.ConvertNull() ?? onButton.GetComponent<MeshRenderer>();
        offButtonRenderer = offButtonRenderer.ConvertNull() ?? offButton.GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        onButton.OnSelectSuccess += PressedOnButton;
        offButton.OnSelectSuccess += PressedOffButton;
    }
    private void OnDisable()
    {
        onButton.OnSelectSuccess -= PressedOnButton;
        offButton.OnSelectSuccess -= PressedOffButton;
    }
    private void PressedOnButton()
    {
        CurrentValue = true;
    }
    private void PressedOffButton()
    {
        CurrentValue = false;
    }
}