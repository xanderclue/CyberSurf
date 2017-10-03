using UnityEngine;
public class LevelSelectOptions : LevelMenuObjectGroup
{
    [SerializeField]
    LevelMenuButton leftButton = null, rightButton = null;
    new private void Start()
    {
        base.Start();
        if (null == leftButton)
            Debug.LogWarning("Missing LevelSelectOptions.leftButton");
        if (null == rightButton)
            Debug.LogWarning("Missing LevelSelectOptions.rightButton");
    }
}