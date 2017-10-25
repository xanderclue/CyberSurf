using UnityEngine;

public class LevelMenuObjectGroup : MonoBehaviour
{
    [SerializeField]
    private LevelMenuStuff levelMenuScript = null;
    protected LevelMenuStuff LevelMenuScript { get { return levelMenuScript; } }
    private Vector3 activeLocalPosition, inactiveLocalPosition;
    private float tVal = 1.0f;
    private bool groupEnabled = true;
    protected void Start()
    {
        if (null == levelMenuScript)
            levelMenuScript = GetComponentInParent<LevelMenuStuff>();
        inactiveLocalPosition = activeLocalPosition = transform.localPosition;
        inactiveLocalPosition.z = -activeLocalPosition.z;
    }
    public virtual void EnableGroup()
    {
        groupEnabled = true;
        foreach (SelectedObject button in GetComponentsInChildren<SelectedObject>())
            button.IsDisabled = false;
    }
    public virtual void DisableGroup()
    {
        groupEnabled = false;
        foreach (SelectedObject button in GetComponentsInChildren<SelectedObject>())
            button.IsDisabled = true;
    }
    protected void Update()
    {
        tVal = Mathf.Clamp01(tVal += (groupEnabled ? Time.deltaTime : -Time.deltaTime) / levelMenuScript.SinkDuration);
        transform.localPosition = Vector3.Lerp(inactiveLocalPosition, activeLocalPosition, tVal);
    }
    public virtual void ConfirmOptions() { }
    public virtual void ResetOptions() { }
    public virtual void DefaultOptions() { }
}