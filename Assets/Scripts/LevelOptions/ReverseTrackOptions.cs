using UnityEngine;
public class ReverseTrackOptions : LevelMenuObjectGroup
{
    [SerializeField] private LevelMenuButton onButton = null, offButton = null;
    [SerializeField] private Material activeMaterial = null;
    [SerializeField] private MeshRenderer onButtonRenderer = null, offButtonRenderer = null;
    [SerializeField] private bool defaultValue = false;
    private Material inactiveMaterial = null;
    private bool tempValue = false;
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
        LevelManager.reverseMode = tempValue;
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
        tempValue = LevelManager.reverseMode;
        UpdateDisplay();
    }
}