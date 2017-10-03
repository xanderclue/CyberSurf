using UnityEngine;
public class RaceModeOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton leftButton = null, rightButton = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing RaceModeOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing RaceModeOptions.rightButton");
    }
}