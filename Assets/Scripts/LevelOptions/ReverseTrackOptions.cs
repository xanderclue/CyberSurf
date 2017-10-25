using UnityEngine;
public class ReverseTrackOptions : LevelMenuObjectGroup
{
    [SerializeField]
    private LevelMenuButton onButton = null, offButton = null;
    [SerializeField]
    private Material activeMaterial = null;
    private Material inactiveMaterial = null;
    private MeshRenderer onButtonRenderer = null, offButtonRenderer = null;
    [SerializeField]
    private bool defaultValue = false;
    private bool tempValue;
    private static bool ActualValue { get; set; } // replace with game's value
    new private void Start()
    {
        base.Start();
        onButtonRenderer = onButton.GetComponent<MeshRenderer>();
        offButtonRenderer = offButton.GetComponent<MeshRenderer>();
        inactiveMaterial = onButtonRenderer.material;
        if (null == activeMaterial)
            activeMaterial = inactiveMaterial;
    }
    private void OnEnable()
    {
        onButton.OnButtonPressed += ButtonOnFunction;
        offButton.OnButtonPressed += ButtonOffFunction;
    }
    private void OnDisable()
    {
        onButton.OnButtonPressed -= ButtonOnFunction;
        offButton.OnButtonPressed -= ButtonOffFunction;
    }
    public override void EnableGroup()
    {
        base.EnableGroup();
        onButton.enabled = true;
        offButton.enabled = true;
    }
    public override void DisableGroup()
    {
        base.DisableGroup();
        onButton.enabled = false;
        offButton.enabled = false;
    }
    private void ButtonOnFunction()
    {
        tempValue = true;
        UpdateDisplay();
    }
    private void ButtonOffFunction()
    {
        tempValue = false;
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        if (tempValue)
        {
            onButtonRenderer.material = activeMaterial;
            offButtonRenderer.material = inactiveMaterial;
        }
        else
        {
            onButtonRenderer.material = inactiveMaterial;
            offButtonRenderer.material = activeMaterial;
        }
    }
    public override void ConfirmOptions()
    {
        base.ConfirmOptions();
        ActualValue = tempValue;
    }
    public override void DefaultOptions()
    {
        base.DefaultOptions();
        tempValue = defaultValue;
        UpdateDisplay();
    }
    public override void ResetOptions()
    {
        base.ResetOptions();
        tempValue = ActualValue;
        UpdateDisplay();
    }
}