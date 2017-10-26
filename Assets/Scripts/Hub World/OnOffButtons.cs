using UnityEngine;
public class OnOffButtons : MonoBehaviour
{
    [SerializeField] private EventSelectedObject onButton = null, offButton = null;
    [SerializeField] private MeshRenderer onButtonRenderer = null, offButtonRenderer = null;
    [SerializeField] private Material activeMaterial = null, inactiveMaterial = null;
    public delegate void ButtonPressedEvent(bool isOn);
    public ButtonPressedEvent OnButtonPressed;
    private void Awake()
    {
        onButtonRenderer = onButtonRenderer ?? onButton.GetComponent<MeshRenderer>();
        offButtonRenderer = offButtonRenderer ?? offButton.GetComponent<MeshRenderer>();
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
    public bool SetValue(bool value)
    {
        if (null != onButtonRenderer)
            onButtonRenderer.material = value ? activeMaterial : inactiveMaterial;
        if (null != offButtonRenderer)
            offButtonRenderer.material = value ? inactiveMaterial : activeMaterial;
        return value;
    }
    private void PressedOnButton()
    {
        if (null != OnButtonPressed)
            OnButtonPressed(SetValue(true));
    }
    private void PressedOffButton()
    {
        if (null != OnButtonPressed)
            OnButtonPressed(SetValue(false));
    }
}