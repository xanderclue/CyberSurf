using UnityEngine;

public class LevelMenuObjectGroup : MonoBehaviour
{
    [SerializeField]
    private LevelMenuStuff levelMenuScript = null;
    private void Start()
    {
        if (null == levelMenuScript)
            levelMenuScript = GetComponentInParent<LevelMenuStuff>();
    }
}