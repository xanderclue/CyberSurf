using UnityEngine;

public class LevelMenuObjectGroup : MonoBehaviour
{
    [SerializeField]
    private LevelMenuStuff levelMenuScript = null;
    protected LevelMenuStuff LevelMenuScript { get { return levelMenuScript; } }
    private Vector3 activeLocalPosition, inactiveLocalPosition;
    private float tVal = 1.0f;
    private const float sinkDuration = 0.75f;
    private const float invSinkDuration = 1.0f / sinkDuration;
    private bool groupEnabled = true;
    protected void Start()
    {
        if (null == levelMenuScript)
            levelMenuScript = GetComponentInParent<LevelMenuStuff>();
        inactiveLocalPosition = activeLocalPosition = transform.localPosition;
        inactiveLocalPosition.z = -activeLocalPosition.z;
    }
    public void EnableGroup()
    {
        groupEnabled = true;
    }
    public void DisableGroup()
    {
        groupEnabled = false;
    }
    protected void Update()
    {
        tVal = Mathf.Clamp01(tVal += (groupEnabled ? Time.deltaTime : -Time.deltaTime) * invSinkDuration);
        transform.localPosition = Vector3.Lerp(inactiveLocalPosition, activeLocalPosition, tVal);
    }
}