using UnityEngine;

public class LevelMenuObjectGroup : MonoBehaviour
{
    [SerializeField, Tooltip("Check this box if these options are not available for the game")]
    private bool isGrayedOut = false;
    protected bool IsGrayedOut { get { return isGrayedOut; } }
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
        if (levelMenuScript.forceNoGray)
            isGrayedOut = false;
        inactiveLocalPosition = activeLocalPosition = transform.localPosition;
        inactiveLocalPosition.z = -activeLocalPosition.z;
        if (isGrayedOut)
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
                if (null == mr.GetComponent<TMPro.TextMeshPro>())
                    mr.material = levelMenuScript.grayOutMaterial;
    }
    public virtual void EnableGroup()
    {
        groupEnabled = true;
    }
    public virtual void DisableGroup()
    {
        groupEnabled = false;
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