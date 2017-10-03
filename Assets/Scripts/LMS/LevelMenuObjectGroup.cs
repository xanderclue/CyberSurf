using UnityEngine;

public class LevelMenuObjectGroup : MonoBehaviour
{
    [SerializeField]
    private LevelMenuStuff levelMenuScript = null;
    protected LevelMenuStuff LevelMenuScript { get { return levelMenuScript; } }
    private Vector3 activeLocalPosition, inactiveLocalPosition;
    protected void Start()
    {
        if (null == levelMenuScript)
            levelMenuScript = GetComponentInParent<LevelMenuStuff>();
        inactiveLocalPosition = activeLocalPosition = transform.localPosition;
        inactiveLocalPosition.z = -activeLocalPosition.z;
    }
    public void EnableGroup()
    {
        transform.localPosition = activeLocalPosition;
    }
    public void DisableGroup()
    {
        transform.localPosition = inactiveLocalPosition;
    }
}